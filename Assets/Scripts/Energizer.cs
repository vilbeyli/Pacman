using UnityEngine;
using System.Collections;

public class Energizer : MonoBehaviour {

    public GameManager gm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "pacman")
        {
            gm.ScareGhosts();
            Destroy(gameObject);
        }
    }
}
