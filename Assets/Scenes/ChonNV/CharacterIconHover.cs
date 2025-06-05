using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int characterIndex;
    public UICharacterSelection uiManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiManager.ShowCharacterStats(characterIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiManager.HideCharacterStats();
    }
}