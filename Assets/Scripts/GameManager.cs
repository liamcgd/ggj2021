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
            fullscreenDialogue.EndDialogue();
            StartNewWave();
        }
#endif
    }

    public void StartNewWave()
    {
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
                dialogueCoroutine = StartCoroutine(fullscreenDialogue.Display(1));
                heartIndicator.enabled = true;
                break;
        }
    }
}
