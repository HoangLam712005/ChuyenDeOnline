using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterSelection : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameInput;
    //public TextMeshProUGUI previewName;
    public Button nextButton;
    public Button previousButton;
    public Button confirmButton;

    [Header("Character Selection")]
    public List<GameObject> characterPrefabs;
    public Transform characterSpawnPoint;

    private int currentIndex = 0;
    private GameObject currentCharacterInstance;

    void Start()
    {
        ShowCharacter(currentIndex);
        nextButton.onClick.AddListener(NextCharacter);
        previousButton.onClick.AddListener(PreviousCharacter);
        confirmButton.onClick.AddListener(ConfirmCharacter);
        nameInput.onValueChanged.AddListener(UpdatePreviewName);
    }

    void ShowCharacter(int index)
    {
        if (currentCharacterInstance != null)
            Destroy(currentCharacterInstance);

        currentCharacterInstance = Instantiate(characterPrefabs[index], characterSpawnPoint.position, Quaternion.identity);
        currentCharacterInstance.transform.SetParent(characterSpawnPoint, true);
        currentCharacterInstance.AddComponent<RotateAvatar_M3>(); // Script cho xoay nhẹ
        currentCharacterInstance.transform.localScale = Vector3.one * 100f;
    }

    void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterPrefabs.Count;
        ShowCharacter(currentIndex);
    }

    void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + characterPrefabs.Count) % characterPrefabs.Count;
        ShowCharacter(currentIndex);
    }

    void UpdatePreviewName(string value)
    {
        //previewName.text = value;
    }

    void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.SetString("PlayerName", nameInput.text);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameplayScene");
    }
}