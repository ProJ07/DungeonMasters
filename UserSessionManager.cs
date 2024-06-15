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

    private UserData userData;

    void Start()
    {
        loginMenu.SetActive(false);
        userMenu.SetActive(false);

        userData = UserData.Instance;
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
        userEmailText.text = userData.user.Email;

        // Obtener el nombre de usuario del documento de Firestore
        DocumentReference userDoc = userData.db.Collection("users").Document(userData.user.UserId);
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
        if (userData.user == null)
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
        userData.SaveUserData();
    }

    public void Logout()
    {
        userData.auth.SignOut();
        userData.user = null;
        userData.SetValuesToDefault();
        Debug.Log("User logged out successfully.");
        ShowLoginMenu();
    }
}
