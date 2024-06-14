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

    public void Login()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (auth == null || db == null)
        {
            StartCoroutine(UpdateFeedbackUI("Firebase is not initialized yet. Please try again later."));
            return;
        }

        // Look up the email based on the username in Firestore
        db.Collection("users").WhereEqualTo("username", username).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                StartCoroutine(UpdateFeedbackUI("Login canceled."));
                return;
            }
            if (task.IsFaulted)
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

            // Ensure the snapshot has documents
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    string email = document.GetValue<string>("email");

                    // Authenticate the user with Firebase Authentication
                    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(authTask =>
                    {
                        if (authTask.IsCanceled)
                        {
                            StartCoroutine(UpdateFeedbackUI("Login canceled."));
                            return;
                        }
                        if (authTask.IsFaulted)
                        {
                            StartCoroutine(UpdateFeedbackUI("Invalid Username or Password"));
                            return;
                        }

                        UserSessionManager.Instance.user = authTask.Result.User;
                        StartCoroutine(UpdateFeedbackUI("User logged in successfully!"));
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
