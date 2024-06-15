using UnityEngine;
using UnityEngine.UI;

public class DifficultyContainer : MonoBehaviour
{
    public Button easyButton;

    public Button mediumButton;
    public Image mediumLockImage;

    public Button hardButton;
    public Image hardLockImage;

    private readonly Color mediumColor = new (1f, 170 / 255f, 0f);
    private readonly Color hardColor = new (200 / 255f, 7 / 255f, 0f);
    private readonly Color grayColor = new (100 / 255f, 100 / 255f, 100 / 255f);

    // Start is called before the first frame update
    void Start()
    {
        ButtonCheck();
        easyButton.onClick.AddListener(OnEasyButtonClick);
        mediumButton.onClick.AddListener(OnMediumButtonClick);
        hardButton.onClick.AddListener(OnHardButtonClick);
        
    }

    void ButtonCheck()
    {
        if (!UserData.Instance.mediumUnlocked)
        {
            mediumButton.image.color = grayColor;
            mediumLockImage.gameObject.SetActive(true);
            mediumButton.interactable = false;
        }
        else
        {
            mediumButton.image.color = mediumColor;
            mediumLockImage.gameObject.SetActive(false);
            mediumButton.interactable = true;
        }

        if (!UserData.Instance.hardUnlocked)
        {
            hardButton.image.color = grayColor;
            hardLockImage.gameObject.SetActive(true);
            hardButton.interactable = false;
        }
        else
        {
            hardButton.image.color = hardColor;
            hardLockImage.gameObject.SetActive(false);
            hardButton.interactable = true;
        }
    }

    void OnEasyButtonClick()
    {
        GameData.Instance.SetDifficultyMultiplierAndLoadGame(1f);
    }

    void OnMediumButtonClick()
    {
        GameData.Instance.SetDifficultyMultiplierAndLoadGame(1.5f);
    }

    void OnHardButtonClick()
    {
        GameData.Instance.SetDifficultyMultiplierAndLoadGame(2f);
    }

}
