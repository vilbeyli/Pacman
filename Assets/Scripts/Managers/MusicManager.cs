using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    //-------------------------------------------------------------------
    // singleton implementation
    private static MusicManager _instance;

    public static MusicManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MusicManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(gameObject);
        }

    }




}
