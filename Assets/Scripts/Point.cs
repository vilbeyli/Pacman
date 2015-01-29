using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour
{

    public float speed;

	// Use this for initialization
	void Start () 
    {
	    Destroy(gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
	    speed -= 0.01f;
    }
}
