using UnityEngine;

public class SomeOtherScript : MonoBehaviour
{
    public MenuManager menuManager;

    public void OnButtonClick(int menuIndex)
    {
        // Mostrar el menú cuando se haga clic en un botón
        menuManager.ShowMenu(menuIndex);
    }

    public void CloseMenus()
    {
        // Cerrar todos los menús
        menuManager.HideMenus();
    }
}