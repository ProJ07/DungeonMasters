using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;

public class UserData : MonoBehaviour
{
    public int coins = 10;
    public int gems = 0;
    public int damageLevel = 0;
    public int healthLevel = 0;
    public int speedLevel = 0;
    public bool mediumUnlocked = false;
    public bool hardUnlocked = false;

    private TMP_Text coinsText;
    private TMP_Text gemsText;

    public FirebaseAuth auth;
    public FirebaseFirestore db;
    public FirebaseUser user;

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
                Debug.Log("Firebase initialized successfully.");

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

    private void Update()
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }

        if (gemsText != null)
        {
            gemsText.text = gems.ToString();
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

    public void SetTexts()
    {
        coinsText = GameObject.Find("CoinNumber")?.GetComponent<TextMeshProUGUI>();
        gemsText = GameObject.Find("GemNumber")?.GetComponent<TextMeshProUGUI>();
    }

    public void SetValuesToDefault()
    {
        coins = 10;
        gems = 0;
        damageLevel = 0;
        healthLevel = 0;
        speedLevel = 0;
        mediumUnlocked = false;
        hardUnlocked = false;
    }
}
