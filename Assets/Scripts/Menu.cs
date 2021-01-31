using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Level1";
    [SerializeField] private PlayerData data = default;
    [Header("CanvasGroups")]
    [SerializeField] private CanvasGroup mainMenu = default;
    [SerializeField] private CanvasGroup charSelect = default;
    [Header("Character Select")]
    [SerializeField] private RectTransform charMoveLoc1 = default;
    [SerializeField] private RectTransform charMoveLoc2 = default;
    [SerializeField] private List<Button> characters = default;
    [SerializeField] private TextMeshProUGUI headingText = default;
    [SerializeField] private Image fade = default;

    private GameObject chosenCharacter;
    private GameObject chosenPartner;

    public void StartGame()
    {
        mainMenu.DOFade(0, 1).OnComplete(delegate
        {
            mainMenu.gameObject.SetActive(false);
            charSelect.gameObject.SetActive(true);
            charSelect.DOFade(1, 1);
        });
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ChoosePlayerCharacter(GameObject button)
    {
        chosenCharacter = button;
        Button charButton = chosenCharacter.GetComponent<Button>();
        characters.Remove(charButton);
        Destroy(charButton);

        foreach (Button character in characters)
            character.interactable = false;

        chosenCharacter.transform.DOMove(charMoveLoc1.position, 0.5f).OnComplete(delegate
        {
            headingText.DOFade(0, 0.2f).OnComplete(delegate
            {
                headingText.text = "Select your partner";
                headingText.DOFade(1, 0.2f).OnComplete(delegate
                {
                    foreach (Button character in characters)
                    {
                        character.interactable = true;
                        character.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
                        character.onClick.AddListener(() => ChoosePartnerCharacter(character.gameObject));
                    }
                });
            });
        });
    }

    public void ChoosePartnerCharacter(GameObject button)
    {
        chosenPartner = button;
        Button charButton = chosenPartner.GetComponent<Button>();
        characters.Remove(charButton);
        Destroy(charButton);

        foreach (Button character in characters)
        {
            character.interactable = false;
            character.GetComponent<Image>().DOFade(0, 0.5f);
        }

        chosenPartner.transform.DOMove(charMoveLoc2.position, 0.5f).OnComplete(delegate
        {
            headingText.DOFade(0, 0.2f).OnComplete(delegate
            {
                headingText.text = "Let's Go!";
                headingText.DOFade(1, 0.2f).OnComplete(delegate
                {
                    Invoke(nameof(MoveToGameScene), 1.5f);
                });
            });
        });
    }

    private void MoveToGameScene()
    {
        fade.DOFade(1, 1).OnComplete(delegate
        {
            data.playerCharacter = chosenCharacter.GetComponent<Image>().sprite;
            data.partnerCharacter = chosenPartner.GetComponent<Image>().sprite;
            SceneManager.LoadScene(sceneToLoad);
        });
    }
}
