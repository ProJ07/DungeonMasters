using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public int currentRound = 1;

    public float multiplier;

    public float difficultyMultiplier = 1;
    public float roundMultiplier = 0;
    public readonly float roundIncreaseMultiplier = 0.05f;

    public static GameData Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set the difficulty multiplier and load the game scene
    public void SetDifficultyMultiplierAndLoadGame(float value)
    {
        difficultyMultiplier = value;
        SceneManager.LoadScene("Game");
    }

    public void UpdateMultiplier()
    {
        multiplier = difficultyMultiplier + roundMultiplier;
    }

    // Increment the round and update the round multiplier
    public void NextRound()
    {
        if (currentRound > 20 && difficultyMultiplier == 1f)
        {
            UserData.Instance.mediumUnlocked = true;
        }

        if (currentRound > 20 && difficultyMultiplier == 1.5f)
        {
            UserData.Instance.hardUnlocked = true;
        }

        currentRound++;
        if (currentRound > 1)
        {
            roundMultiplier += roundIncreaseMultiplier;
        }
    }
}