using System;
using System.Collections.Generic;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Collections;

public class UserDataUI : MonoBehaviour
{
    public Text pseudoText;
    public Text eloText;

    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;


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
        StartCoroutine(UpdateUserProfil());
       // StartCoroutine(DownloadImage(TextureURL));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Update the user's display name with the currently selected display name.
    IEnumerator UpdateUserProfil()
    {
        yield return new WaitForSeconds(1);
        test = auth.CurrentUser.PhotoUrl;

        DocumentReference docRef = db.Collection("users").Document(auth.CurrentUser.UserId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> user = snapshot.ToDictionary();
                pseudoText.text = "Pseudo: " + user["Name"].ToString();
                eloText.text = "Elo: " + user["Elo"].ToString();

                foreach (KeyValuePair<string, object> pair in user)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });



    }

}
