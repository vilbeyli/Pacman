using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GhostMove : MonoBehaviour {

	private Vector3 waypoint;			// AI-determined waypoint
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
			waypoint = transform.position + _direction;
			//Debug.Log ("waypoint (" + waypoint.position.x + ", " + waypoint.position.y + ") set! _direction: " + _direction.x + ", " + _direction.y);
		}
	}

	public AI AIComponent;
	public float speed = 0.3f;

	public float scatterLength = 5f;
	public float waitLength = 0.0f;

	private float timeToEndScatter;
	private float timeToEndWait;

	enum State { Wait, Init, Scatter, Chase, Run };
	State state;

	// handles
	public GameGUINavigation GUINav;

	//-----------------------------------------------------------------------------------------
	// variables end, functions begin
	void Start()
	{
		InitializeGhost();
	}

	void FixedUpdate () 
	{
		if(GameManager.gameState == GameManager.GameState.Game){
			animate ();

			switch(state)
			{
			case State.Wait:
				Wait();
				break;

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

	}

	//-----------------------------------------------------------------------------------
	// Start() functions

	public void InitializeGhost()
	{
		waypoint = transform.position;	// to avoid flickering animation
		state = State.Wait;
		timeToEndWait = Time.time + waitLength + GUINav.initialDelay;
		InitializeWaypoints(state);
	}

	void InitializeWaypoints(State st)
	{
		//-----------------
		// File Format: Init and Scatter coordinates separated by empty line
		// Init X,Y 
		// Init X,Y
		// 
		// Scatter X,Y
		// Scatter X,Y

		string path = Application.dataPath + "/Data/" + transform.name + "_wps.txt";
		StreamReader stream = new StreamReader(path);

		waypoints = new Queue<Vector3>();
		Vector3 wp;

		if(st == State.Init)
		{ 
			while(!stream.EndOfStream)
			{
				// stop reading if empty line is reached
				string line = stream.ReadLine();
				if(line.Length == 0) 	break;

				string[] values = line.Split(' ');
				float x = float.Parse(values[0]);
				float y = float.Parse(values[1]);

				wp = new Vector3(x,y,0);
				waypoints.Enqueue(wp);
			}
		}

		if(st == State.Scatter)
		{
			// skip until empty line is reached, read coordinates afterwards
			bool scatterWps = false;	// Scatter waypoints
			while(!stream.EndOfStream)
			{
				string line = stream.ReadLine();
				if(line.Length == 0)
				{
					scatterWps = true;	// we reached the scatter waypoints
					continue;	// do not read empty line, go to next line
				}

				if(scatterWps)
				{
					string[] values = line.Split(' ');
					int x = Int32.Parse(values[0]);
					int y = Int32.Parse(values[1]);
					
					wp = new Vector3(x,y,0);
					waypoints.Enqueue(wp);
				}
			}
		}

		// if in wait state, patrol vertically
		if(st == State.Wait)
		{
			Vector3 pos = transform.position;

			// inky and clyde start going down and then up
			if(transform.name == "inky" || transform.name == "clyde")
			{
				waypoints.Enqueue(new Vector3(pos.x, pos.y-0.5f, 0f));
				waypoints.Enqueue(new Vector3(pos.x, pos.y+0.5f, 0f));
			}
			// while pinky start going up and then down
			else
			{
				waypoints.Enqueue(new Vector3(pos.x, pos.y+0.5f, 0f));
				waypoints.Enqueue(new Vector3(pos.x, pos.y-0.5f, 0f));
			}
		}

		stream.Close();
	}

	//------------------------------------------------------------------------------------
	// Update functions
	void animate()
	{
		Vector3 dir = waypoint - transform.position;
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
	void Wait()
	{
		if(Time.time >= timeToEndWait)
		{
			state = State.Init;
			waypoints.Clear();
			InitializeWaypoints(state);
		}

		// get the next waypoint and move towards it
		MoveToWaypoint(true);
	}

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
			timeToEndScatter = Time.time + scatterLength;

			return;
		}

		// get the next waypoint and move towards it
		MoveToWaypoint();
	}

	void Scatter()
	{
		if(Time.time >= timeToEndScatter)
		{
			waypoints.Clear();
			state = State.Chase;
			return;
		}

		// get the next waypoint and move towards it
		MoveToWaypoint(true);

	}

	void RunAI()
	{

		// if not at waypoint, move towards it
		if(transform.position != waypoint)
		{
			AI = false;
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
			rigidbody2D.MovePosition(p);
		}

		// if at waypoint, run AI module
		else  AI = true;

	}

	void RunAway()
	{

	}

	//------------------------------------------------------------------------------
	// Utility functions
	void MoveToWaypoint(bool loop = false)
	{
		waypoint = waypoints.Peek();		// get the waypoint
		if(transform.position != waypoint)	// if its not reached
		{									// move towards it
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
			rigidbody2D.MovePosition(p);
		}
		else 	// if waypoint is reached, remove it from the queue
		{
			if(loop)	waypoints.Enqueue(waypoints.Dequeue());
			else		waypoints.Dequeue();
		}
	}
}
