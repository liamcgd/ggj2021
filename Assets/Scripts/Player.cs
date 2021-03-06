using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data = default;
    [SerializeField] private ParticleSystem basicAttack = default;
    [SerializeField] private TrailRenderer dashTrail = default;
    [SerializeField] private float basicAttackCooldown = 1;
    [Header("Movement")]
    [SerializeField] private float maxMoveSpeed = 5;
    [SerializeField] private float acceleration = 3;
    [SerializeField] private float deceleration = 3;
    [SerializeField] private float dashDistance = 5;
    [SerializeField] private float dashCooldown = 1;

    private Transform tr;
    private SpriteRenderer rend;
    private Vector2 moveDir = new Vector2();
    private float curMoveSpeed = 0;
    private float curBasicAttackCooldown = 0;
    private float curDashCooldown = 0;
    private float healthRegenCooldown = 0;
    private bool dashThisFrame = false;
    private bool dashing = false;

    // Start is called before the first frame update
    private void Start()
    {
        tr = transform;
        rend = GetComponent<SpriteRenderer>();
        if (data.playerCharacter != null)
            rend.sprite = data.playerCharacter;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();

        if (healthRegenCooldown > 0)
            healthRegenCooldown -= Time.deltaTime;
        else if (data.health.value < 1)
        {
            healthRegenCooldown = 0.4f;
            data.health.value += 0.05f;
        }
    }

    #region Input Registration

    public void ReceiveMoveInput(InputAction.CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<Vector2>();
    }

    public void ReceiveDashInput(InputAction.CallbackContext ctx)
    {
        if (curDashCooldown <= 0)
        {
            dashThisFrame = true;
            curDashCooldown = dashCooldown;
            StartCoroutine(CooldownDash());
        }
    }

    public void ReceiveAttackInput(InputAction.CallbackContext ctx)
    {
        if (curBasicAttackCooldown <= 0)
        {
            basicAttack.Play();
            curBasicAttackCooldown = basicAttackCooldown;
            StartCoroutine(CooldownBasicAttack());
        }
    }

    public void ReceiveWaveAttackInput(InputAction.CallbackContext ctx)
    {

    }

    #endregion

    private void Move()
    {
        if (dashing) return;

        if (moveDir != Vector2.zero)
        {
            if (DOTween.IsTweening("Decel"))
                DOTween.Kill("Decel");

            if (curMoveSpeed != maxMoveSpeed)
            {
                curMoveSpeed = Mathf.Min(curMoveSpeed += Time.deltaTime * acceleration, maxMoveSpeed);
            }

            var moveDirV3 = new Vector3(moveDir.x, moveDir.y, 0);

            if (moveDirV3.x > 0)
                rend.flipX = false;
            else if (moveDirV3.x < 0)
                rend.flipX = true;

            tr.position += moveDirV3 * Time.deltaTime * curMoveSpeed;

            if (dashThisFrame)
            {
                var newPosAfterDash = tr.position + moveDirV3 * dashDistance;
                dashTrail.emitting = true;
                dashing = true;
                tr.DOMove(newPosAfterDash, 0.15f).OnComplete(
                    delegate { dashTrail.emitting = false; dashing = false; });
            }
        }
        else if (curMoveSpeed > 0 && !DOTween.IsTweening("Decel"))
        {
            DOTween.To(() => curMoveSpeed, x => curMoveSpeed = x, 0, maxMoveSpeed / deceleration).SetEase(Ease.Linear).SetId("Decel");
        }
        else if (dashThisFrame)
            curDashCooldown = 0;

        dashThisFrame = false;
    }

    #region Cooldown Coroutines

    private IEnumerator CooldownBasicAttack()
    {
        while (curBasicAttackCooldown > 0)
        {
            curBasicAttackCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CooldownDash()
    {
        while (curDashCooldown > 0)
        {
            curDashCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    public void TakeDamage(float damage)
    {
        data.health.value -= damage;
        healthRegenCooldown = 5;
    }
}
