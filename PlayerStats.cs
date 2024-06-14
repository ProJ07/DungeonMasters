using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int damage;
    public int maxHealth;
    public int currentHealth;
    public float speed;
    public float invulnerabilityDuration = 1.0f; // Duración de invulnerabilidad después de recibir daño

    private float lastDamageTime;
    private bool isGameOver = false; // Estado de "Game Over"
    private GameOverManager gameOverManager;

    // Instancia
    public static PlayerStats Instance { get; private set; }

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

    private void Start()
    {
        UpdateStats();
        currentHealth = maxHealth;
        lastDamageTime = -invulnerabilityDuration; // Asegura que el jugador pueda recibir daño inmediatamente al empezar
    }

    private void Update()
    {
        // No actualizar estadísticas si el juego ha terminado
        if (!isGameOver)
        {
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        damage = 20 + UserData.Instance.damageLevel;
        maxHealth = 100 + (UserData.Instance.healthLevel * 5);
        speed = 3f + (UserData.Instance.speedLevel * 0.4f);
    }

    public void TakeDamage(int damage)
    {
        if (Time.time >= lastDamageTime + invulnerabilityDuration)
        {
            currentHealth -= damage;
            lastDamageTime = Time.time;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        isGameOver = true;

        gameOverManager = FindObjectOfType<GameOverManager>();
        gameOverManager.ShowGameOver();

        // Desactivar los scripts de movimiento y ataque
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
    }

    public void ResetPlayer()
    {
        isGameOver = false;
        currentHealth = maxHealth;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        transform.position = Vector3.zero; // Posición (0, 0)
    }
}