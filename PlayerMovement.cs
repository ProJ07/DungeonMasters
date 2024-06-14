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

    public List<GameObject> menus; // Lista de men�s para verificar si est�n activos

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Village")
        {
            inGame = false;
            //menus = new List<GameObject>
            //{
            //    GameObject.Find("MenuAccountLogin"),
            //    GameObject.Find("MenuAccountRegister"),
            //    GameObject.Find("MenuAccountInfo"),
            //    GameObject.Find("MenuUpgrade"),
            //    GameObject.Find("MenuAccountLogin")
            //};
        }
        else
        {
            inGame = true;
        }

        bool isAnyMenuActive = false;

        if (!inGame)
        {
            foreach (GameObject menu in menus)
            {
                if (menu.activeSelf)
                {
                    isAnyMenuActive = true;
                    break;
                }
            }
        }

        if (isAnyMenuActive)
        {
            // Si alg�n men� est� activo, no permitir movimiento
            moveInput = Vector2.zero;
            playerAnimator.SetFloat("Horizontal", 0);
            playerAnimator.SetFloat("Vertical", 0);
            playerAnimator.SetFloat("Speed", 0);
        }
        else
        {
            // Si no hay men�s activos, permitir movimiento
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(moveX, moveY).normalized;

            playerAnimator.SetFloat("Horizontal", moveX);
            playerAnimator.SetFloat("Vertical", moveY);
            playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
        }
    }

    private void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + moveInput * PlayerStats.Instance.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            // L�gica para recoger monedas
        }
    }
}