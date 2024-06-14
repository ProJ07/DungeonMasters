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

    private FirebaseAuth auth;
    private FirebaseFirestore db;

    void Start()
    {
        feedbackUI.SetActive(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
            }
        });
    }

    public void Register()
    {
        string username = usernameInputField.text.ToString();
        string email = emailInputField.text.ToString();
        string password = passwordInputField.text.ToString();
        string confirmPassword = confirmPasswordInputField.text.ToString();

        if (password != confirmPassword)
        {
            StartCoroutine(UpdateFeedbackUI("Passwords do not match."));
            return;
        }

        if (auth == null)
        {
            Debug.Log("Firebase Auth is not initialized.");
            return;
        }

        if (db == null)
        {
            Debug.Log("Firebase Firestore is not initialized.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                StartCoroutine(UpdateFeedbackUI("Registration canceled."));
                return;
            }
            if (task.IsFaulted)
            {
                StartCoroutine(UpdateFeedbackUI("Error: " + task.Exception?.Flatten().Message));
                return;
            }

            if (task.Result == null || task.Result.User == null)
            {
                StartCoroutine(UpdateFeedbackUI("Error: User creation result is null."));
                return;
            }

            FirebaseUser newUser = task.Result.User;

            DocumentReference userDoc = db.Collection("users").Document(newUser.UserId);
            Dictionary<string, object> userData = new()
            {
                { "username", username },
                { "email", email },
                { "password", password }
            };

            userDoc.SetAsync(userData).ContinueWithOnMainThread(storeTask =>
            {
                if (storeTask.IsFaulted)
                {
                    StartCoroutine(UpdateFeedbackUI("Error storing user data"));
                    return;
                }
                UserSessionManager.Instance.user = newUser;
                UserData.Instance.LoadUserData();
                StartCoroutine(UpdateFeedbackUI("User registered successfully!"));
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