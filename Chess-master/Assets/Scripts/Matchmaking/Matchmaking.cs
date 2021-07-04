using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;

public class Matchmaking : MonoBehaviour
{
    public Button leaveButton;
    public Button searchButton;
    public Text searchText;

    GameOnline gameOnline;

    public static string gameUid;
    public static string player1;
    public static string player2;



    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    protected Firebase.Auth.FirebaseAuth auth;


    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        searchButton.onClick.AddListener( MatchmakingAsync);
        
        leaveButton.onClick.AddListener(() => SceneManager.LoadScene("MenuScene"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void MatchmakingAsync()
    {
        searchButton.gameObject.SetActive(false);
        await LaunchMatchmakingAsync();
    }

    async Task LaunchMatchmakingAsync()
    {
        // afficher text
        searchText.enabled = true;

        // vérifier si il y a des games avec 1 joueur en hôte

        CollectionReference colRef = db.Collection("games");
        // on pourrait rajouter un champ datetime pour récupérer la partie la plus ancienne pour
        // éviter de faire patienter le joueur
        QuerySnapshot gamesSolos = await colRef.WhereEqualTo("playerUid2", "").Limit(1).GetSnapshotAsync();

        if(gamesSolos.Count > 0)
        {
            foreach(DocumentSnapshot documentSnapshot in gamesSolos.Documents)
            {
                Dictionary<string, object> getGame = documentSnapshot.ToDictionary();

                foreach (KeyValuePair<string, object> pair in getGame)
                {
                    Debug.Log( "I'm here normalement justin " + pair.Key + " " + pair.Value);
                    // On rejoins la partie
                    gameUid = getGame["uid"].ToString();

                    DocumentReference gameSoloRef = db.Collection("games").Document(gameUid);
                    await gameSoloRef.UpdateAsync("playerUid2", auth.CurrentUser.UserId);
                    player1 = getGame["playerUid1"].ToString();
                    player2 = getGame["playerUid2"].ToString();


                    // on charge la scene du jeu tout en envoyant les données de l'id et des joueurs?
                    SceneManager.LoadScene("OnlineGameScene");

                    //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                }
            }
        }
        else
        {
            Debug.Log("La gamev zevzefzef" + gameOnline + colRef.WhereEqualTo("playerUid2", ""));

            // sinon créer une game en tant que hôte (si le joueur quitte avant, supprimé la game)
            CreateGame();

            // quand 2 joueurs matchent, soit faire pour chacun un bool ready
            // ou lancer directement la scene en récupérant l'id de la game
        }


    }

    public void CreateGame()
    {
        DocumentReference docRef = db.Collection("games").Document();
        gameUid = docRef.Id;
        gameOnline = new GameOnline(docRef.Id, auth.CurrentUser.UserId, "");
        Dictionary<string, object> gameOnlineData = gameOnline.getDictionnary();

        Debug.Log("La game" + gameOnline + gameOnlineData);
        docRef.SetAsync(gameOnlineData).ContinueWithOnMainThread(task => {
            Debug.Log("Added data in the games collection.");

        // écoute si un autre joueur arrive dans la game
            docRef.Listen(snapshot => {
                Debug.Log("Callback received document snapshot.");
                Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> getGame = snapshot.ToDictionary();
                Debug.Log(getGame["playerUid2"]);



                if(getGame["playerUid2"].ToString() != "")
                {
                    Debug.Log("y a un joueur qui est là !");

                    player1 = getGame["playerUid1"].ToString();
                    player2 = getGame["playerUid2"].ToString();
                    // lance la scene en conservant aussi l'id de la salle, peut être tout le dictionnaire getGame
                    // on charge la scene du jeu tout en envoyant les données de l'id et des joueurs?
                    SceneManager.LoadScene("OnlineGameScene");
                }
                else
                {
                    Debug.Log("Attendons !");
                }

            leaveButton.onClick.AddListener(() => {
                SceneManager.LoadScene("MainScene");
                docRef.DeleteAsync();
            });

            });
        });

        //await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        //{
        //    DocumentSnapshot snapshot = task.Result;
        //    if (snapshot.Exists)
        //    {
        //        Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
        //        Dictionary<string, object> getGameData = snapshot.ToDictionary();
        //        foreach (KeyValuePair<string, object> pair in getGameData)
        //        {
        //            Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
        //    }
        //});
    }
}
