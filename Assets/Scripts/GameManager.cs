using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField, FormerlySerializedAs("startingDialogue")] private TextLerper fullscreenDialogue = default;
    [SerializeField] private List<GameObject> wave1Enemies = new List<GameObject>();
    [SerializeField] private GameObject heartPiece1 = default;
    [SerializeField] private GameObject heartPiece2 = default;
    [SerializeField] private GameObject heartPiece3 = default;
    [SerializeField] private GameObject heartPiece4 = default;
    [SerializeField] private Image heartIndicator = default;
    [SerializeField] private Sprite[] heartIndicatorSprites = default;
    [SerializeField] private GameObject hpBar = default;
    [SerializeField] private Transform cam = default;
    [SerializeField] private SpriteRenderer fade = default;

    private Coroutine dialogueCoroutine;
    private int waveNumber = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        Time.timeScale = 0;
        dialogueCoroutine = StartCoroutine(fullscreenDialogue.Display(0));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame && dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
            StartCoroutine(StartNewWave());
        }
#endif
    }

    public IEnumerator StartNewWave()
    {
        fade.DOFade(0, 2.5f).SetUpdate(true);
        cam.DOLocalMoveY(0, 2f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(2.5f);
        hpBar.SetActive(true);
        dialogueCoroutine = null;
        Time.timeScale = 1;
        waveNumber++;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        switch (waveNumber)
        {
            case 1:
                wave1Enemies.Remove(enemy);
                if (wave1Enemies.Count == 0)
                {
                    heartPiece1.SetActive(true);
                }
                break;
        }
    }

    public void CollectHeartShard()
    {
        switch (waveNumber)
        {
            case 1:
                Time.timeScale = 0;
                hpBar.SetActive(false);
                fade.DOFade(1, 1).SetUpdate(true);
                cam.DOLocalMoveY(1.2f, 2f).SetUpdate(true).OnComplete(delegate
                {
                    dialogueCoroutine = StartCoroutine(fullscreenDialogue.Display(1));
                });
                heartIndicator.enabled = true;
                break;
        }
    }
}
