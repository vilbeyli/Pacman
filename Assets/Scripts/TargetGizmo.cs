using UnityEngine;
using System.Collections;

public class TargetGizmo : MonoBehaviour {

	public GameObject ghost;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(ghost.GetComponent<AI>().targetTile != null)
		{
			Vector3 pos = new Vector3(ghost.GetComponent<AI>().targetTile.x, 
										ghost.GetComponent<AI>().targetTile.y, 0f);
			transform.position = pos;
		}
	}
}
