using System;
using UnityEngine;

public class GameUtil : MonoBehaviour
{
    //Player (Levels, currency, stats)
    private readonly int[] maxLevel = { 15, 15, 5 }; // Damage, Health, Speed
    private readonly int[,] coinCost = { { 5, 10, 15, 25, 35, 50, 65, 85, 105, 130, 155, 185, 215, 250, 300, -1 },
                                        { 5, 10, 15, 25, 35, 50, 65, 85, 105, 130, 155, 185, 215, 250, 300, -1 }};

    private readonly int[] gemCost = { 5, 10, 15, 20, 25, -1};

    //Instancia
    public static GameUtil Instance { get; private set; }

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

    public int GetMaxLevel(int level) { 
        if (level >= 0 && level < maxLevel.Length)
        {
            return maxLevel[level];
        } else
        {
            throw new ArgumentOutOfRangeException("Level index out of range.");
        }
    }
    public int GetCoinCost(int type, int level)
    {
        if (type >= 0 && type < coinCost.GetLength(0) &&
            level >= 0 && level < coinCost.GetLength(1))
        {
            return coinCost[type, level];
        }
        else
        {
            throw new ArgumentOutOfRangeException("Type or level is out of range.");
        }
    }

    public int GetGemCost(int level)
    {
        if (level >= 0 && level < gemCost.Length)
        {
            return gemCost[level];
        }
        else
        {
            throw new ArgumentOutOfRangeException("Level index out of range.");
        }
    }
}