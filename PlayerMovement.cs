using System.Collections;
using System.Collections.Generic; // Necesario para List
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private bool inGame;

    public List<GameObject> menus; // Lista de menús para verificar si están activos

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!enabled) return;

        // Si no hay menús activos, permitir movimiento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (!enabled) return;
        playerRB.MovePosition(playerRB.position + moveInput * PlayerStats.Instance.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            // Lógica para recoger monedas
        }
    }
}