using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignOut : MonoBehaviour
{

    Button buttonSignOut;
    protected Firebase.Auth.FirebaseAuth auth;
    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        buttonSignOut = GetComponent<Button>();
        buttonSignOut.onClick.AddListener(() => auth.SignOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
