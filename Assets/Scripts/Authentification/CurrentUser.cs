using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUserAuth : MonoBehaviour
{

    protected Firebase.Auth.FirebaseAuth auth;
    // Firebase User keyed by Firebase Auth.
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    protected Firebase.Auth.FirebaseUser currentUser;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        currentUser = auth.CurrentUser;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
