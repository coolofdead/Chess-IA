using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EmailConnexion : MonoBehaviour
{

    Button emailButton;
    public Text emailText;
    public Text passwordText;

    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    string email;
    string password;


    // Start is called before the first frame update
    void Start()
    {
        emailButton = GetComponent<Button>();
        emailButton.onClick.AddListener(ConnexionEmail);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ConnexionEmail()
    {

        email = emailText.text;
        password = passwordText.text;

        Debug.Log("email" + email);
        Debug.Log("pass" + password);


        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

    }
}
