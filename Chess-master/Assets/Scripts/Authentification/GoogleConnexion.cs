//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using UnityEngine;

//public class GoogleConnexion : MonoBehaviour
//{

//    Button googleButton;

//    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;



//    // Start is called before the first frame update
//    void Start()
//    {
//        googleButton = GetComponent<Button>();
//        googleButton.onClick.AddListener(ConnexionGoogle);
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    void ConnexionGoogle()
//    {
//        Firebase.Auth.Credential credential =
//        Firebase.Auth.GoogleAuthProvider.GetCredential(null, null);
//            auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
//                if (task.IsCanceled)
//                {
//                    Debug.LogError("SignInWithCredentialAsync was canceled.");
//                    return;
//                }
//                if (task.IsFaulted)
//                {
//                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
//                    return;
//                }

//                Firebase.Auth.FirebaseUser newUser = task.Result;
//                Debug.LogFormat("User signed in successfully: {0} ({1})",
//                    newUser.DisplayName, newUser.UserId);
//            });

//    }

//}
