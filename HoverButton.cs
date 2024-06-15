using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Referencia al objeto de texto que deseas mostrar
    public GameObject hoverUI;
    public TMP_Text hoverText;

    // M�todo llamado cuando el puntero entra en el bot�n
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name.Equals("MediumButton") && !UserData.Instance.mediumUnlocked)
        {
            hoverText.text = "Reach Round 20 in Easy Difficulty";
            hoverUI.SetActive(true);
        }

        if (gameObject.name.Equals("HardButton") && !UserData.Instance.hardUnlocked)
        {
            hoverText.text = "Reach Round 20 in Medium Difficulty";
            hoverUI.SetActive(true);
        }
    }

    // M�todo llamado cuando el puntero sale del bot�n
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverUI.SetActive(false);
        }
    }
}
