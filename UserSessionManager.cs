using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;

public class UserSessionManager : MonoBehaviour
{
    public GameObject loginMenu;
    public GameObject userMenu;
    public TMP_Text userNameText;
    public TMP_Text userEmailText;

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    public FirebaseUser user;

    // Instancia
    public static UserSessionManager Instance { get; private set; }

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

        void Start()
    {
        loginMenu.SetActive(false);
        userMenu.SetActive(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
                user = auth.CurrentUser;
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
            }
        });
    }

    private void ShowLoginMenu()
    {
        loginMenu.SetActive(true);
        userMenu.SetActive(false);
    }

    private void ShowUserMenu()
    {
        loginMenu.SetActive(false);
        userMenu.SetActive(true);
        userEmailText.text = user.Email;

        // Obtener el nombre de usuario del documento de Firestore
        DocumentReference userDoc = db.Collection("users").Document(user.UserId);
        userDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                string username = task.Result.GetValue<string>("username");
                userNameText.text = username;
            }
        });
    }

    public void LoginButtonClicked()
    {
        if (user == null)
        {
            ShowLoginMenu();
        }
        else
        {
            ShowUserMenu();
        }
    }

    public void SaveUserData()
    {
        UserData.Instance.SaveUserData();
    }

    public void Logout()
    {
        auth.SignOut();
        user = null;
        Debug.Log("User logged out successfully.");
        ShowLoginMenu();
    }
}
