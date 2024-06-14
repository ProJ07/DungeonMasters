using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnimator;
    private bool isAttacking = false;
    private bool inGame;

    void Start()
    {
        // Inicializa el componente Animator
        playerAnimator = GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        // Verifica si la escena actual es "Village"
        if (SceneManager.GetActiveScene().name == "Village")
        {
            inGame = false;
        }
        else
        {
            inGame = true;
        }

        // Inicia el ataque si se cumple la condición
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && inGame)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        // Verifica si el Animator está inicializado antes de usarlo
        if (playerAnimator == null)
        {
            yield break;
        }

        isAttacking = true;

        // Determinar la dirección del mouse en relación con el jugador
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Activar la animación correspondiente
        if (direction.x >= 0 && direction.y >= 0) // Arriba derecha
        {
            playerAnimator.SetTrigger("AttackUpRight");
        }
        else if (direction.x >= 0 && direction.y < 0) // Abajo derecha
        {
            playerAnimator.SetTrigger("AttackDownRight");
        }
        else if (direction.x < 0 && direction.y >= 0) // Arriba izquierda
        {
            playerAnimator.SetTrigger("AttackUpLeft");
        }
        else if (direction.x < 0 && direction.y < 0) // Abajo izquierda
        {
            playerAnimator.SetTrigger("AttackDownLeft");
        }

        yield return new WaitForSeconds(0.5f);  // Espera la duración de la animación

        isAttacking = false;
    }
}