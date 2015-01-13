using UnityEngine;
using System.Collections;

public class PacdotSpawner : MonoBehaviour {

	public GameObject pacdot;
	public float interval;
	public float startOffset;

	private float startTime;

	// Use this for initialization
	void Start () 
	{
		startTime = Time.time + startOffset;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > startTime + interval)
		{
			GameObject obj = (GameObject)Instantiate(pacdot, transform.position, Quaternion.identity);
			obj.transform.parent = transform;

			startTime = Time.time;
		}
	}
}
