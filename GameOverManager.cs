using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public TMP_Text enemiesDefeatedText;
    public TMP_Text coinsCollectedText;
    public TMP_Text gemsCollectedText;

    public void ShowGameOver()
    {
        Debug.Log("Game Over");
        UpdateGameOverStats();
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    private void UpdateGameOverStats()
    {
        enemiesDefeatedText.text = GameStats.Instance.enemiesDefeated.ToString();
        coinsCollectedText.text = GameStats.Instance.coins.ToString();
        gemsCollectedText.text = GameStats.Instance.gems.ToString();
    }

    public void BackToVillage()
    {
        Time.timeScale = 1f; // Reanudar el juego
        UserData.Instance.SaveUserData();
        Destroy(GameObject.Find("AuthManager"));
        SceneManager.LoadScene("Village");
    }
}