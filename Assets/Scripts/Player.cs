using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour
{
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
    private Vector2 moveDir = new Vector2();
    private float curMoveSpeed = 0;
    private float curBasicAttackCooldown = 0;
    private float curDashCooldown = 0;
    private bool dashThisFrame = false;

    // Start is called before the first frame update
    private void Start()
    {
        tr = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
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
        if (moveDir != Vector2.zero)
        {
            if (DOTween.IsTweening("Decel"))
                DOTween.Kill("Decel");

            if (curMoveSpeed != maxMoveSpeed)
            {
                curMoveSpeed = Mathf.Min(curMoveSpeed += Time.deltaTime * acceleration, maxMoveSpeed);
            }

            var moveDirV3 = new Vector3(moveDir.x, moveDir.y, 0);

            tr.position += moveDirV3 * Time.deltaTime * curMoveSpeed;

            if (dashThisFrame)
            {
                var newPosAfterDash = tr.position + moveDirV3 * dashDistance;
                dashTrail.emitting = true;
                tr.DOMove(newPosAfterDash, 0.15f).OnComplete(
                    delegate { dashTrail.emitting = false; });
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
}
