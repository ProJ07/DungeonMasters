using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContainer : MonoBehaviour
{

    public GameObject itemPrefab; // Prefab 
    public RectTransform container; // RectTransform del contenedor
    public TMP_Text levelText; // Nivel de mejora
    public TMP_Text maxLevelText; // Nivel máximo
    public TMP_Text costText; // Costo de mejora
    public int menuNumber; // Número de menu

    private int level = 0; // Número de elementos a colorear
    private List<GameObject> allItems = new List<GameObject>(); // Lista para mantener todos los items
    private int maxLevel;
    private int costNumber;
    private int numberOfItems; // Número de objetos a añadir
    private bool isTransitioning = false;

    // Color
    private readonly Color coloredItemColor = new Color(1.0f, 170 / 255f, 0f); 
    private readonly Color resetColor = new Color(132 / 255f, 87 / 255f, 0f);

    void Start()
    {
        maxLevel = GameUtil.Instance.GetMaxLevel(menuNumber);
        numberOfItems = maxLevel;
        maxLevelText.text = maxLevel.ToString();
        level = 1;

        for (int i = 0; i < numberOfItems; i++)
        {
            GameObject newItem = AddItem();
            allItems.Add(newItem); // Almacena referencia al nuevo item
            if (i < level)
            {
                ColorItem(newItem);
            }
        }
    }

    void Update()
    {
        switch (menuNumber)
        {
            case 0:
                level = UserData.Instance.damageLevel;
                costNumber = GameUtil.Instance.GetCoinCost(menuNumber, level);
                break;
            case 1:
                level = UserData.Instance.healthLevel;
                costNumber = GameUtil.Instance.GetCoinCost(menuNumber, level);
                break;
            case 2:
                level = UserData.Instance.speedLevel;
                costNumber = GameUtil.Instance.GetGemCost(level);
                break;
        }

        levelText.text = level.ToString();

        if (costNumber != -1)
        {
            costText.text = costNumber.ToString();
        }
        else
        {
            costText.text = "Max";
        }

        // Actualizar los colores de los elementos según el nivel actual
        UpdateItemColors();
    }

    void UpdateItemColors()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            GameObject item = allItems[i];
            Image image = item.GetComponent<Image>();
            if (image != null)
            {
                image.color = i < level ? coloredItemColor : resetColor;
            }
        }
    }

    GameObject AddItem()
    {
        if (itemPrefab && container)
        {
            GameObject newItem = Instantiate(itemPrefab, container);
            return newItem;
        }
        return null;
    }

    void ColorItem(GameObject item)
    {
        if (item != null)
        {
            Image image = item.GetComponent<Image>();
            if (image != null)
            {
                image.color = coloredItemColor;
            }
        }
    }

    public void Upgrade()
    {
        switch (menuNumber)
        {
            case 0:
                if (GameUtil.Instance.GetMaxLevel(0) > UserData.Instance.damageLevel && UserData.Instance.coins >= costNumber)
                {
                    UserData.Instance.damageLevel++;
                    UserData.Instance.coins -= costNumber;
                } else if (UserData.Instance.coins <= costNumber && !isTransitioning)
                {
                    StartCoroutine(ChangeTextColor());
                }
                break;
            case 1:
                if (GameUtil.Instance.GetMaxLevel(1) > UserData.Instance.healthLevel && UserData.Instance.coins >= costNumber)
                {
                    UserData.Instance.healthLevel++;
                    UserData.Instance.coins -= costNumber;
                } else if (UserData.Instance.coins <= costNumber && !isTransitioning)
                {
                    StartCoroutine(ChangeTextColor());
                }
                break;
            case 2:
                if (GameUtil.Instance.GetMaxLevel(2) > UserData.Instance.speedLevel && UserData.Instance.gems >= costNumber)
                {
                    UserData.Instance.speedLevel++;
                    UserData.Instance.gems -= costNumber;
                } else if (UserData.Instance.gems <= costNumber && !isTransitioning)
                {
                    StartCoroutine(ChangeTextColor());
                }
                break;
        }
    }

    IEnumerator ChangeTextColor()
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeToColor(Color.red, 0.2f)); // Cambia a rojo
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeToColor(Color.white, 0.2f)); // Cambia a blanco
        isTransitioning = false;
    }

    IEnumerator FadeToColor(Color targetColor, float duration)
    {
        Color startColor = costText.color;
        float time = 0;

        while (time < duration)
        {
            costText.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null; // Espera hasta el próximo frame antes de continuar
        }

        costText.color = targetColor; // Asegura que el color final es exactamente el objetivo
    }
}