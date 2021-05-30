using System;
using System.Collections.Generic;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;


public class UserDataUI : MonoBehaviour
{
    public Text nameText;
    public Text emailText;
    protected Firebase.Auth.FirebaseAuth auth;
    private string TextureURL;
    System.Uri test;

    // Firebase User keyed by Firebase Auth.
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        UpdateUserProfil();
       // StartCoroutine(DownloadImage(TextureURL));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Update the user's display name with the currently selected display name.
    public void UpdateUserProfil()
    {
        nameText.text = auth.CurrentUser.DisplayName;
        emailText.text = auth.CurrentUser.Email;
        test = auth.CurrentUser.PhotoUrl;



    }

}
