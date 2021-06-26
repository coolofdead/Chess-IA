using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOnline : MonoBehaviour
{
    public string uid;
    public string playerUid1;
    public string playerUid2;


    // Creation de base
    public GameOnline(string uid, string playerUid1, string playerUid2)
    {
        this.uid = uid;
        this.playerUid1 = playerUid1;
        this.playerUid2 = playerUid2;

    }

    public Dictionary<string, object> getDictionnary()
    {

        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "uid", uid },
            { "playerUid1", playerUid1 },
            { "playerUid2", playerUid2 },
        };

        return user;

    }

}