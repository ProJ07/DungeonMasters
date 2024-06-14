using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> menus = new List<GameObject>(); // Lista de menús
    public PlayerMovement playerMovement; // Referencia al script de movimiento del jugador

    void Awake()
    {
        // Aquí asigna manualmente los menús desactivados
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

        // Desactivar todos los menús
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }

        // Activar el menú seleccionado
        menus[index].SetActive(true);

        // Desactivar el movimiento del jugador cuando un menú está activo
        playerMovement.enabled = false;
    }

    public void HideMenus()
    {
        // Desactivar todos los menús
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }

        // Activar el movimiento del jugador cuando todos los menús están desactivados
        playerMovement.enabled = true;
    }
}