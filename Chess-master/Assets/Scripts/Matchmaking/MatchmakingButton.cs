using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MatchmakingButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MatchmakingScene"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
