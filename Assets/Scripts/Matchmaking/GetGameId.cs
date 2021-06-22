using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;

public class GetGameId : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = Matchmaking.gameUid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
