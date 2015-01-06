using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour {

	public Transform waypoint;

	public bool AI = false;

	public Vector3 _direction;
	public Vector3 direction 
	{
		get
		{
			return _direction;
		}

		set
		{
			_direction = value;
			waypoint.position = transform.position + _direction;
			Debug.Log ("waypoint (" + waypoint.position.x + ", " + waypoint.position.y + ") set! _direction: " + _direction.x + ", " + _direction.y);
		}
	}

	public AI AIComponent;

	public float speed = 0.3f;

	void Start()
	{
		_direction = waypoint.position - transform.position;
	}

	void FixedUpdate () 
	{
		// if not at waypoint
		if(transform.position != waypoint.position)
		{
			//Debug.Log ("WP: " + waypoints[cur].position);
			AI = false;
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint.position, speed);
			rigidbody2D.MovePosition(p);
		}

		else {
			AI = true;
		}

		animate ();
	}

	void animate()
	{
		Vector3 dir = waypoint.position - transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "pacman")
		{
			//Destroy(other.gameObject);
		}
	}
}
