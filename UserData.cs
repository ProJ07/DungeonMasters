using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;
using UnityEngine.SceneManagement;

public class UserData : MonoBehaviour
{
    public int coins = 20;
    public int gems = 5;
    public int damageLevel = 0;
    public int healthLevel = 0;
    public int speedLevel = 0;
    public bool mediumUnlocked = false;
    public bool hardUnlocked = false;

    private TMP_Text coinsText;
    private TMP_Text gemsText;
    private TMP_Text damageLevelText;
    private TMP_Text healthLevelText;
    private TMP_Text speedLevelText;

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private FirebaseUser user;

    public bool menuUpgradeActive = false;
    public bool inGame;

    // Instancia
    public static UserData Instance { get; private set; }

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
        SetTexts();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
                user = auth.CurrentUser;

                if (user != null)
                {
                    LoadUserData();
                }
                else
                {
                    Debug.LogError("User is not logged in.");
                }
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
            }
        });
    }

    public void Update()
    {
        coinsText.text = coins.ToString();
        gemsText.text = gems.ToString();

        if (menuUpgradeActive)
        {
            damageLevelText.text = damageLevel.ToString();
            healthLevelText.text = healthLevel.ToString();
            speedLevelText.text = speedLevel.ToString();
        }
    }

    public void SaveUserData()
    {
        if (user != null)
        {
            DocumentReference docRef = db.Collection("users").Document(user.UserId).Collection("data").Document("userData");
            Dictionary<string, object> userData = new()
            {
                { "coins", coins },
                { "gems", gems },
                { "damageLevel", damageLevel },
                { "healthLevel", healthLevel },
                { "speedLevel", speedLevel },
                { "mediumUnlocked", mediumUnlocked },
                { "hardUnlocked", hardUnlocked }
            };

            docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("User data saved successfully.");
                }
                else
                {
                    Debug.LogError("Error saving user data: " + task.Exception);
                }
            });
        }
    }

    public void LoadUserData()
    {

        if (user != null)
        {
            DocumentReference docRef = db.Collection("users").Document(user.UserId).Collection("data").Document("userData");

            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {

                if (task.IsFaulted)
                {
                    Debug.LogError("Error loading user data: " + task.Exception);
                    return;
                }

                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {

                    Dictionary<string, object> userData = snapshot.ToDictionary();
                    coins = (int)(long)userData["coins"];
                    gems = (int)(long)userData["gems"];
                    damageLevel = (int)(long)userData["damageLevel"];
                    healthLevel = (int)(long)userData["healthLevel"];
                    speedLevel = (int)(long)userData["speedLevel"];
                    mediumUnlocked = (bool)userData["mediumUnlocked"];
                    hardUnlocked = (bool)userData["hardUnlocked"];
                    Debug.Log("User data loaded successfully.");
                }
                else
                {
                    Debug.Log("No user data found.");
                }
            });
        }
    }

    public void SetUpgradeMenuVar(bool var)
    {
        menuUpgradeActive = var;
    }

    public void SetUpgradeMenuTexts()
    {
        damageLevelText = GameObject.Find("DamageLevelNumber").GetComponent<TextMeshProUGUI>();
        healthLevelText = GameObject.Find("HealthLevelNumber").GetComponent<TextMeshProUGUI>();
        speedLevelText = GameObject.Find("SpeedLevelNumber").GetComponent<TextMeshProUGUI>();
    }

    public void SetTexts()
    {
        coinsText = GameObject.Find("CoinNumber").GetComponent<TextMeshProUGUI>();
        gemsText = GameObject.Find("GemNumber").GetComponent<TextMeshProUGUI>();
    }
}
