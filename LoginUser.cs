using System.Collections;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;

public class LoginUser : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public GameObject feedbackUI;
    public TMP_Text feedbackText;

    public GameObject loginMenu;

    private UserData userData;

    void Start()
    {
        feedbackUI.SetActive(false);
        userData = UserData.Instance;
    }

    public void Login()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (userData.auth == null || userData.db == null)
        {
            StartCoroutine(UpdateFeedbackUI("Firebase is not initialized yet. Please try again later."));
            return;
        }

        // Look up the email based on the username in Firestore
        userData.db.Collection("users").WhereEqualTo("username", username).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                StartCoroutine(UpdateFeedbackUI("Error: " + task.Exception?.Flatten().Message));
                return;
            }

            QuerySnapshot snapshot = task.Result;
            if (snapshot.Count == 0)
            {
                StartCoroutine(UpdateFeedbackUI("Username not found."));
                return;
            }

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    string email = document.GetValue<string>("email");

                    // Authenticate the user with Firebase Authentication
                    userData.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(authTask =>
                    {
                        if (authTask.IsCanceled || authTask.IsFaulted)
                        {
                            StartCoroutine(UpdateFeedbackUI("Invalid Username or Password"));
                            return;
                        }

                        userData.user = authTask.Result.User;
                        userData.LoadUserData();
                        StartCoroutine(UpdateFeedbackUI("User logged in successfully!"));
                        loginMenu.SetActive(false);
                    });
                    return;
                }
            }

            StartCoroutine(UpdateFeedbackUI("No matching username found."));
        });
    }

    private IEnumerator UpdateFeedbackUI(string message)
    {
        feedbackUI.SetActive(true);
        feedbackText.text = message;
        yield return new WaitForSeconds(2);
        feedbackUI.SetActive(false);
    }
}
