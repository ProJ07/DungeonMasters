using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Coin, Gem, Heart }
    public CollectibleType collectibleType;
    private bool canBeCollected = false;

    void Start()
    {
        StartCoroutine(EnableCollectionAfterDelay(0.5f)); //Espera 0.5s para poder recoger (que no sea instantaneo)
    }

    private IEnumerator EnableCollectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBeCollected = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canBeCollected && other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Collect(playerStats);
                Destroy(gameObject);
            }
        }
    }

    private void Collect(PlayerStats playerStats)
    {
        switch (collectibleType)
        {
            case CollectibleType.Coin:
                GameStats.Instance.AddCoin();
                break;
            case CollectibleType.Gem:
                GameStats.Instance.AddGem();
                break;
            case CollectibleType.Heart:
                playerStats.Heal((int)(playerStats.maxHealth * 0.4f)); //Cura 40% de la vida máxima
                break;
        }
    }

    public void MakeCollectibleAfterDelay(float delay)
    {
        StartCoroutine(EnableCollectionAfterDelay(delay));
    }
}