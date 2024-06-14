using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int baseDamage;
    [SerializeField] private float baseSpeed;
    [SerializeField] private GameObject healthBarPrefab; // Prefab de la barra de vida
    [SerializeField] private GameObject coinPrefab; // Prefab de la moneda
    [SerializeField] private GameObject gemPrefab; // Prefab de la gema
    [SerializeField] private GameObject heartPrefab; // Prefab del corazón

    private int currentHealth;
    private int currentDamage;
    private float currentSpeed;

    private Transform playerTransform;
    private GameObject healthBar;
    private Slider healthBarSlider; // Slider de la barra de vida

    public delegate void EnemyDeath();
    public event EnemyDeath OnEnemyDeath;

    void Start()
    {
        currentHealth = baseHealth;
        currentDamage = baseDamage;
        currentSpeed = baseSpeed;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Instanciar la barra de vida
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
            healthBarSlider = healthBar.GetComponentInChildren<Slider>();

            if (healthBarSlider != null)
            {
                healthBarSlider.maxValue = baseHealth;
                healthBarSlider.value = currentHealth;
            }
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            MoveTowardsPlayer();
        }

        // Actualizar la posición de la barra de vida
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 1, 0);
        }
    }

    public void AdjustStats(float multiplier)
    {
        currentHealth = Mathf.RoundToInt(baseHealth * multiplier);
        currentDamage = Mathf.RoundToInt(baseDamage * multiplier);
        currentSpeed = baseSpeed * multiplier;

        //Debug.Log("Salud: " + currentHealth + ", Daño: " + currentDamage + ", Velocidad: " + currentSpeed);

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = currentHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(PlayerStats.Instance.damage);
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(currentDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar(); // Actualizar la barra de vida cuando recibe daño

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        DropCollectible();
        GameStats.Instance.AddEnemyDefeated();
        Destroy(gameObject);
    }

    private void DropCollectible()
    {
        Vector3 dropPosition = transform.position + (Vector3)(Random.insideUnitCircle * 0.5f);

        // Dropear moneda siempre
        Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        if (Random.Range(0, 100) < 25) // 25% de probabilidad de dropear una segunda moneda
        {
            Instantiate(coinPrefab, dropPosition + new Vector3(0.1f, 0.1f, 0), Quaternion.identity);
        }

        // Dropear gema con 10% de probabilidad
        if (Random.Range(0, 100) < 10)
        {
            Instantiate(gemPrefab, dropPosition + new Vector3(0.2f, 0.2f, 0), Quaternion.identity);
        }

        // Dropear corazón con 25% de probabilidad
        if (Random.Range(0, 100) < 25)
        {
            Instantiate(heartPrefab, dropPosition + new Vector3(0.3f, 0.3f, 0), Quaternion.identity);
        }
    }
}