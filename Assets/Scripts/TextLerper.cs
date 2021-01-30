using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextLerper : MonoBehaviour
{
    [SerializeField, TextArea(5, 20)] private string[] dialogueSets = default;
    [SerializeField] private float timePerLetter = 0.1f;

    private TextMeshProUGUI textbox;

    private IEnumerator Start()
    {
        textbox = GetComponent<TextMeshProUGUI>();

        yield return new WaitForSecondsRealtime(1.5f);

        foreach (string dialogueSet in dialogueSets)
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
                else if(dialogueSet[i] == '/')
                {
                    curText += "<i>";
                    continue;
                }

                curText += dialogueSet[i];
                textbox.text = curText;

                float t = timePerLetter;
                while (t > 0)
                {
                    yield return null;
                    t -= Time.unscaledDeltaTime;
                }
            }

            yield return new WaitForSecondsRealtime(1.5f);
            textbox.DOFade(0, 2.5f).SetUpdate(true).OnComplete(delegate
            {
                textbox.text = "";
                textbox.color = Color.white;
            });
            yield return new WaitForSecondsRealtime(3);
        }
    }
}
