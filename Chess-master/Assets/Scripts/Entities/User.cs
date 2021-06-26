using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string uid;
    public string nameUser;
    public string imgUrl;
    public int elo;


    // Creation de base
    public User(string uid, string nameUser, string imgUrl, int elo)
    {
        this.uid = uid;
        this.nameUser = nameUser;
        this.imgUrl = imgUrl;
        this.elo = elo;


    }

    public Dictionary<string, object> getDictionnary()
    {

        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "uid", uid },
            { "Name", nameUser },
            { "ImgUrl", imgUrl },
            { "Elo", elo },
        };

        return user;

    }

}