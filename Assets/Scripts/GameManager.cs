using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private TextLerper startingDialogue = default;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(startingDialogue.Display());
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }
}
