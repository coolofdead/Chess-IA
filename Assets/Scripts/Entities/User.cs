using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string uid;
    public string nameUser;
    public string imgUrl;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Creation de base
    public User(string uid, string nameUser, string imgUrl)
    {
        this.uid = uid;
        this.nameUser = nameUser;
        this.imgUrl = imgUrl;

    }

    public Dictionary<string, object> getDictionnary()
    {

        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "uid", uid },
            { "Name", nameUser },
            { "ImgUrl", imgUrl },
        };

        return user;

    }

}