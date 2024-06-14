using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> menus = new List<GameObject>(); // Lista de men�s
    public PlayerMovement playerMovement; // Referencia al script de movimiento del jugador

    void Awake()
    {
        // Aqu� asigna manualmente los men�s desactivados
        GameObject[] menuArray = GameObject.FindGameObjectsWithTag("Menu");
        menus = new List<GameObject>(menuArray);
    }

    public void ShowMenu(int index)
    {
        if (index < 0 || index >= menus.Count)
        {
            Debug.LogError("Index out of range");
            return;
        }

        // Desactivar todos los men�s
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }

        // Activar el men� seleccionado
        menus[index].SetActive(true);

        // Desactivar el movimiento del jugador cuando un men� est� activo
        playerMovement.enabled = false;
    }

    public void HideMenus()
    {
        // Desactivar todos los men�s
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }

        // Activar el movimiento del jugador cuando todos los men�s est�n desactivados
        playerMovement.enabled = true;
    }
}