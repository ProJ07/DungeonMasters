using UnityEditor.SceneManagement;
using UnityEngine;

public class AnimateImage : MonoBehaviour
{
    public RectTransform imageRectTransform;
    private Vector2 newPosition = new Vector2(5, -5);
    private Vector2 originalPosition;
    private bool movingToNewPosition = true;

    void Start()
    {
        if (imageRectTransform == null)
            imageRectTransform = GetComponent<RectTransform>();

        originalPosition = imageRectTransform.localPosition;
        InvokeRepeating("MoveImage", 0, 0.5f);
    }

    void MoveImage()
    {
        if (movingToNewPosition)
            imageRectTransform.localPosition = newPosition;
        else
            imageRectTransform.localPosition = originalPosition;

        movingToNewPosition = !movingToNewPosition;
    }
}