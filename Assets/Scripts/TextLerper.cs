using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;

public class TextLerper : MonoBehaviour
{
    [System.Serializable] internal struct DialogueSet
    {
        [TextArea(5, 20)] public string[] dialogueLines;
    }

    [SerializeField] private DialogueSet[] dialogueSets = default;
    [SerializeField] private float timePerLetter = 0.1f;
    [SerializeField] private CanvasGroup fade = default;

    private TextMeshProUGUI textbox;

    private void Start()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator Display(int setIndex)
    {
        if (fade.alpha == 0)
            fade.DOFade(1, 1).SetUpdate(true);

        yield return new WaitForSecondsRealtime(1.5f);

        foreach (string dialogueSet in dialogueSets[setIndex].dialogueLines)
        {
            string curText = "";

            for (int i = 0; i < dialogueSet.Length; i++)
            {
                if (dialogueSet[i] == '|')
                {
                    yield return new WaitForSecondsRealtime(1);
                    continue;
                }
                else if (dialogueSet[i] == '.')
                {
                    curText += dialogueSet[i];
                    textbox.text = curText;
                    yield return new WaitForSecondsRealtime(0.3f);
                    continue;
                }
                else if (dialogueSet[i] == '/')
                {
                    curText += "<i>";
                    continue;
                }

                curText += dialogueSet[i];
                textbox.text = curText;

                yield return new WaitForSecondsRealtime(timePerLetter);
            }

            yield return new WaitForSecondsRealtime(1.5f);

            if (dialogueSet != dialogueSets[setIndex].dialogueLines.Last())
            {
                textbox.DOFade(0, 2.5f).SetUpdate(true).OnComplete(delegate
                {
                    textbox.text = "";
                    textbox.color = Color.white;
                });
                yield return new WaitForSecondsRealtime(3);
            }
        }

        fade.DOFade(0, 2.5f).SetUpdate(true).OnComplete(delegate
        {
            GameManager.Instance.StartNewWave();
        });
    }

    public void EndDialogue()
    {
        fade.DOFade(0, 2.5f).SetUpdate(true);
    }
}
