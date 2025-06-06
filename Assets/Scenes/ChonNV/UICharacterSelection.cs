using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class UICharacterSelection : SimulationBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameInput;
    //public TextMeshProUGUI previewName;
    //public Button nextButton;
    //public Button previousButton;
    public Button confirmButton;
    public List<Image> characterIcons;

    [Header("Character Selection")]
    public List<GameObject> characterPrefabs;
    public Transform characterSpawnPoint;

    private int currentIndex = 0;
    private GameObject currentCharacterInstance;



    //Hienthongtin
    [Header("Tooltip UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageText;



    void Start()
    {
        ShowCharacter(currentIndex);
        //nextButton.onClick.AddListener(NextCharacter);
        //previousButton.onClick.AddListener(PreviousCharacter);
        confirmButton.onClick.AddListener(ConfirmCharacter);
        nameInput.onValueChanged.AddListener(UpdatePreviewName);
        for (int i = 0; i < characterIcons.Count; i++)
        {
            int index = i;
            characterIcons[i].GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(index));
        }
    }

    void ShowCharacter(int index)
    {
        if (currentCharacterInstance != null)
            Destroy(currentCharacterInstance);

        currentCharacterInstance = Instantiate(characterPrefabs[index], characterSpawnPoint.position, Quaternion.identity);
        currentCharacterInstance.transform.SetParent(characterSpawnPoint, true);
        currentCharacterInstance.AddComponent<RotateAvatar_M3>(); // Script cho xoay nhẹ
        currentCharacterInstance.transform.localScale = Vector3.one * 1f;
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

    //void ConfirmCharacter()
    //{
    //    PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
    //    PlayerPrefs.SetString("PlayerName", nameInput.text);
    //    Debug.Log("CurrentIndex" + currentIndex);
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("example");
    //}
    void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.SetString("PlayerName", nameInput.text);

        if (currentCharacterInstance != null)
            Destroy(currentCharacterInstance);

        UnityEngine.SceneManagement.SceneManager.LoadScene("example");
    }

    void OnCharacterSelected(int index)
    {
        currentIndex = index;
        ShowCharacter(currentIndex);
    }


    //hienthongtin
    public void ShowCharacterStats(int index)
    {
        if (index < 0 || index >= characterPrefabs.Count) return;

        var statsHolder = characterPrefabs[index].GetComponent<CharacterStatsHolder>();
        if (statsHolder == null || statsHolder.stats == null) return;

        var stats = statsHolder.stats;

        tooltipPanel.SetActive(true);
        nameText.text = stats.characterName;
        healthText.text = "Health: " + stats.health;
        speedText.text = "Speed: " + stats.speed;
        damageText.text = "Damage: " + stats.damage;
    }

    public void HideCharacterStats()
    {
        tooltipPanel.SetActive(false);
    }
}