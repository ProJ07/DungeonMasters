using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 originalPosition;
    private Vector2 offset = new Vector2(5, -5); // Ajusta este valor según la sensibilidad deseada

    void Start()
    {
        originalPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = originalPosition + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = originalPosition;
    }
}