using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpHandler : MonoBehaviour
{
    public InputField emailTextBox;
    public InputField passwordTextBox;
    public InputField confirmPasswordTextBox;
    public Button backButton;
    public Button signupButton;
    public Text errorText;
    protected Firebase.Auth.FirebaseAuth auth;
    protected string displayName = "";

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        signupButton.onClick.AddListener(() => canSubmit());
        backButton.onClick.AddListener(() => SceneManager.LoadScene("SignInScene"));
    }

    private void canSubmit()
    {
        errorText.enabled = false;
        if (passwordTextBox.text != confirmPasswordTextBox.text)
        {
            errorText.text = "La confirmation de mot de passe ne correspond pas.";
            errorText.enabled = true;
        }
        else
        {
            CreateUserWithEmailAsync();
        }
    }

    // Create a user with the email and password.
    public Task CreateUserWithEmailAsync()
    {
        string email = emailTextBox.text;
        string password = passwordTextBox.text;

        Debug.Log(String.Format("Attempting to create user {0}...", email));
        DisableUI();

        // This passes the current displayName through to HandleCreateUserAsync
        // so that it can be passed to UpdateUserProfile().  displayName will be
        // reset by AuthStateChanged() when the new user is created and signed in.
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
          .ContinueWithOnMainThread((task) => {
              EnableUI();
              LogTaskCompletion(task, "User Creation");
              return task;
          }).Unwrap();
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    GetErrorMessage((Firebase.Auth.AuthError)firebaseEx.ErrorCode);
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }
        return complete;
    }

    void DisableUI()
    {
        emailTextBox.DeactivateInputField();
        passwordTextBox.DeactivateInputField();
        confirmPasswordTextBox.DeactivateInputField();
        backButton.interactable = false;
        signupButton.interactable = false;
        errorText.enabled = false;
    }

    void EnableUI()
    {
        emailTextBox.ActivateInputField();
        passwordTextBox.ActivateInputField();
        confirmPasswordTextBox.ActivateInputField();
        backButton.interactable = true;
        signupButton.interactable = true;
    }

    // Update the user's display name with the currently selected display name.
    public Task UpdateUserProfileAsync(string newDisplayName = null)
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to update user profile");
            return Task.FromResult(0);
        }
        displayName = newDisplayName ?? displayName;
        Debug.Log("Updating user profile " + displayName);
        return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
        {
            DisplayName = displayName,
            PhotoUrl = auth.CurrentUser.PhotoUrl,
        });
    }

    private void GetErrorMessage(AuthError errorCode)
    {
        switch (errorCode)
        {
            case AuthError.MissingPassword:
                errorText.text = "Veuillez insérer un mot passe.";
                errorText.enabled = true;
                break;
            case AuthError.WeakPassword:
                errorText.text = "Le mot de passe est trop fragile.";
                errorText.enabled = true;
                break;
            case AuthError.InvalidEmail:
                errorText.text = "L'email n'est pas valide.";
                errorText.enabled = true;
                break;
            case AuthError.MissingEmail:
                errorText.text = "Veuillez insérer un email.";
                errorText.enabled = true;
                break;
            case AuthError.UserNotFound:
                errorText.text = "Utilisateur non trouvé.";
                errorText.enabled = true;
                break;
            case AuthError.EmailAlreadyInUse:
                errorText.text = "L'email est déjà utilisé.";
                errorText.enabled = true;
                break;
            default:
                errorText.text = "Une erreur inconnu s'est produit.";
                errorText.enabled = true;
                break;
        }
    }
}