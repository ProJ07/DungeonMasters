using UnityEngine;

public class TriggerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject buttonUI;  // Referencia al botón de la UI para activar
    [SerializeField] private GameObject menu;  // Referencia al menu de la UI para activar
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (menu.activeSelf)
        {
            buttonUI.SetActive(false);
        } else if (other.CompareTag("Player"))  // Asegúrate de que el jugador tiene el tag "Player"
        {
            buttonUI.SetActive(true);  // Activa el botón cuando el jugador entra en el trigger
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buttonUI.SetActive(false);  // Desactiva el botón cuando el jugador sale del trigger
        }
    }
}