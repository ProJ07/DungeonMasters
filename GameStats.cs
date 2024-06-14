using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{

    public int coins = 0;
    public int gems = 0;
    public int enemiesDefeated = 0;

    public static GameStats Instance;

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

    public void AddEnemyDefeated()
    {
        enemiesDefeated++;
    }

    public void AddCoin()
    {
        UserData.Instance.coins++;
        coins ++;
    }

    public void AddGem()
    {
        UserData.Instance.gems++;
        gems++;
    }
}
