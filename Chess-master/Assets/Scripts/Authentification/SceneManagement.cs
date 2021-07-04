using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{

    public Button matchmakingButton;
    public Button multiLocalButton;
    public Button iaButton;
    // Start is called before the first frame update
    void Start()
    {
        matchmakingButton.onClick.AddListener(() => SceneManager.LoadScene("MatchmakingScene"));
        multiLocalButton.onClick.AddListener(() => SceneManager.LoadScene("InGame"));
        iaButton.onClick.AddListener(() => SceneManager.LoadScene("InGameIa"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
