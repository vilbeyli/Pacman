using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostMove : MonoBehaviour {

	public Transform waypoint;			// AI-determined waypoint
	private Queue<Vector3> waypoints;	// waypoints used on Init and Scatter states
	public bool AI = false;

	// direction is set from the AI component
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
			//Debug.Log ("waypoint (" + waypoint.position.x + ", " + waypoint.position.y + ") set! _direction: " + _direction.x + ", " + _direction.y);
		}
	}

	public AI AIComponent;
	public float speed = 0.3f;
	public float scatterLength = 5f;
	private float timeToScatter;

	enum State { Init, Scatter, Chase, Run };
	State state;

	//-----------------------------------------------------------------------------------------
	// Start of Functions
	void Start()
	{
		state = State.Init;
		//_direction = waypoint.position - transform.position;
		InitializeWaypoints(state);

	}

	void FixedUpdate () 
	{
		animate ();

		switch(state)
		{
		case State.Init:
			Init();
			break;

		case State.Scatter:
			Scatter();
			break;

		case State.Chase:
			RunAI();
			break;

		case State.Run:
			RunAway();
			break;
		}

	}

	//-----------------------------------------------------------------------------------
	// Start functions
	void InitializeWaypoints(State st)
	{
		waypoints = new Queue<Vector3>();
		Vector3 wp;

		if(st == State.Init)
		{
			wp = new Vector3(15, 20, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(16, 20, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(16, 23, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(19, 23, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(19, 26, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(22, 26, 0);
			waypoints.Enqueue(wp);
		}

		if(st == State.Scatter)
		{
			wp = new Vector3(27, 26, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(27, 30, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(22, 30, 0);
			waypoints.Enqueue(wp);
			wp = new Vector3(22, 26, 0);
			waypoints.Enqueue(wp);
		}
	}

	//------------------------------------------------------------------------------------
	// Update functions
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
			PlayerController.LoseLife();
		}
	}

	//-----------------------------------------------------------------------------------
	// State functions
	void Init()
	{
		// if the Queue is cleared, do some clean up and change the state
		if(waypoints.Count == 0)
		{
			state = State.Scatter;

			//get direction according to sprite name
			string name = GetComponent<SpriteRenderer>().sprite.name;
			if(name[name.Length-1] == '0' || name[name.Length-1] == '1')	direction = Vector3.right;
			if(name[name.Length-1] == '2' || name[name.Length-1] == '3')	direction = Vector3.left;
			if(name[name.Length-1] == '4' || name[name.Length-1] == '5')	direction = Vector3.up;
			if(name[name.Length-1] == '6' || name[name.Length-1] == '7')	direction = Vector3.down;

			InitializeWaypoints(state);
			timeToScatter = Time.time + scatterLength;

			return;
		}

		// get the next waypoint and move towards it
		waypoint.position = waypoints.Peek();
		if(transform.position != waypoint.position)
		{
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint.position, speed);
			rigidbody2D.MovePosition(p);
		}
		else
		{
			waypoints.Dequeue();
		}
	}

	void Scatter()
	{
		if(Time.time >= timeToScatter)
		{
			waypoints.Clear();
			state = State.Chase;
			return;
		}

		// get the next waypoint and move towards it
		waypoint.position = waypoints.Peek();
		if(transform.position != waypoint.position)
		{
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint.position, speed);
			rigidbody2D.MovePosition(p);
		}
		else
		{
			waypoints.Enqueue(waypoints.Dequeue());
		}
	}

	void RunAI()
	{

		// if not at waypoint
		if(transform.position != waypoint.position)
		{
			AI = false;
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint.position, speed);
			rigidbody2D.MovePosition(p);
		}
		
		else {
			AI = true;
		}
	}

	void RunAway()
	{

	}
}
