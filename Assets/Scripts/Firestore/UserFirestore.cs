using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class UserFirestore : MonoBehaviour
{
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    User user;

    protected Firebase.Auth.FirebaseAuth auth;
    // Firebase User keyed by Firebase Auth.
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    protected Firebase.Auth.FirebaseUser currentUser;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        AddUser();
        ShowUser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void AddUser()
    {
        DocumentReference docRef = db.Collection("users").Document(auth.CurrentUser.UserId);
        user = new User(auth.CurrentUser.UserId, "Toto", "l'imagedeToto");
        Dictionary<string, object> userData= user.getDictionnary();

        Debug.Log("looser" + user + userData);
        docRef.SetAsync(userData).ContinueWithOnMainThread(task => {
            Debug.Log("Added data in the users collection.");
        });
    }

    void ShowUser()
    {
        CollectionReference usersRef = db.Collection("users");
        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Debug.Log(string.Format("User: {0}", document.Id));
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Debug.Log(string.Format("Name: {0}", documentDictionary["Name"]));
                if (documentDictionary.ContainsKey("Middle"))
                {
                    Debug.Log(string.Format("Middle: {0}", documentDictionary["Middle"]));
                }

                Debug.Log(string.Format("Image: {0}", documentDictionary["ImgUrl"]));
            }

            Debug.Log("Read all data from the users collection.");
        });
    }

}
