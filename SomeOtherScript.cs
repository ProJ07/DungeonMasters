using UnityEngine;

public class SomeOtherScript : MonoBehaviour
{
    public MenuManager menuManager;

    public void OnButtonClick(int menuIndex)
    {
        // Mostrar el men� cuando se haga clic en un bot�n
        menuManager.ShowMenu(menuIndex);
    }

    public void CloseMenus()
    {
        // Cerrar todos los men�s
        menuManager.HideMenus();
    }
}