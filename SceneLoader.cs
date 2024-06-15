using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Esta funci�n se llama cada vez que se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        // Verificar el nombre de la escena
        switch (scene.name)
        {
            case "Village":
                HandleVillageScene();
                break;
            case "Game":
                HandleGameScene();
                break;
            default:
                break;
        }
    }

    // Define aqu� las funciones que deseas ejecutar para cada escena
    private void HandleVillageScene()
    {
        Debug.Log("Loading Village...");
        PlayerStats.Instance.ResetPlayer();
        UserData.Instance.SetTexts();
        Destroy(GameObject.Find("GameStats"));
        Destroy(GameObject.Find("RoundManager"));
        UserData.Instance.LoadUserData();
    }

    private void HandleGameScene()
    {
        Debug.Log("Loading Game...");
        // L�gica espec�fica para la escena Game
        // Por ejemplo, iniciar el juego, configurar enemigos, etc.
    }
}