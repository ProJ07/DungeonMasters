using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text roundText;
    public TMP_Text enemiesText;
    public TMP_Text multiplierText;

    public HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(PlayerStats.Instance.maxHealth);
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
    }

    void Update()
    {
        roundText.text = GameData.Instance.currentRound.ToString();
        enemiesText.text = RoundManager.Instance.enemiesAlive.ToString();
        multiplierText.text = GameData.Instance.multiplier.ToString();
        healthBar.SetHealth(PlayerStats.Instance.currentHealth);
    }
}
