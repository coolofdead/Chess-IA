using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDataUI : MonoBehaviour
{
    public Text TextGameId;

    public Text TextPlayer1;
    public Text TextPlayer2;

    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    protected Firebase.Auth.FirebaseAuth auth;


    // Start is called before the first frame update
    void Start()
    {
        getNamePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getNamePlayer()
    {
        Debug.Log("le joueur 1" + Matchmaking.player1);
        Debug.Log("le joueur 2" + Matchmaking.player2);


        // player 1
        DocumentReference docRef = db.Collection("users").Document(Matchmaking.player1);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> player1Data = snapshot.ToDictionary();
                TextPlayer1.text = player1Data["Name"].ToString();
            }
            else
            {
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            }

        });

        // player 2
        DocumentReference docRef2 = db.Collection("users").Document(Matchmaking.player2);
        docRef2.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> player2Data = snapshot.ToDictionary();
                TextPlayer2.text = player2Data["Name"].ToString();
            }
            else
            {
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            }

        });

        TextGameId.text = "Id de la game: " + Matchmaking.gameUid;


    }


}
