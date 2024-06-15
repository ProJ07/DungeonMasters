using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;

public class RegisterUser : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    public GameObject feedbackUI;
    public TMP_Text feedbackText;

    public GameObject registerMenu;

    private UserData userData;

    void Start()
    {
        feedbackUI.SetActive(false);
        userData = UserData.Instance;
    }

    public void Register()
    {
        string username = usernameInputField.text;
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        if (password != confirmPassword)
        {
            StartCoroutine(UpdateFeedbackUI("Passwords do not match."));
            return;
        }

        if (userData.auth == null || userData.db == null)
        {
            Debug.Log("Firebase is not initialized.");
            return;
        }

        userData.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                StartCoroutine(UpdateFeedbackUI("Error: " + task.Exception?.Flatten().Message));
                return;
            }

            FirebaseUser newUser = task.Result.User;

            DocumentReference userDoc = userData.db.Collection("users").Document(newUser.UserId);
            Dictionary<string, object> data = new()
            {
                { "username", username },
                { "email", email }
            };

            userDoc.SetAsync(data).ContinueWithOnMainThread(storeTask =>
            {
                if (storeTask.IsFaulted)
                {
                    StartCoroutine(UpdateFeedbackUI("Error storing user data"));
                    return;
                }
                userData.user = newUser;
                userData.LoadUserData();
                StartCoroutine(UpdateFeedbackUI("User registered successfully!"));
                registerMenu.SetActive(false);
            });
        });
    }

    private IEnumerator UpdateFeedbackUI(string message)
    {
        feedbackUI.SetActive(true);
        feedbackText.text = message;
        yield return new WaitForSeconds(3);
        feedbackUI.SetActive(false);
    }
}
