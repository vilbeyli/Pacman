using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static int lives = 3;
	public float speed = 0.4f;
	Vector2 dest = Vector2.zero;

	// script handles
	static UIScript UI;
	static GameGUINavigation GUINav;
	GameManager GM;

	private bool deadPlaying = false;

	// Use this for initialization
	void Start () 
	{
		UI = GameObject.Find("UI").GetComponent<UIScript>();
		GM = GameObject.Find ("Game Manager").GetComponent<GameManager>();
		GUINav = GameObject.Find ("UI Manager").GetComponent<GameGUINavigation>();
		dest = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		switch(GameManager.gameState)
		{
		case GameManager.GameState.Game:
			readInputAndMove();
			animate ();
			break;

		case GameManager.GameState.Dead:
			if(!deadPlaying)	
				StartCoroutine("PlayDeadAnimation");
			break;
		}
	
	}

	IEnumerator PlayDeadAnimation()
	{
		deadPlaying = true;
		GetComponent<Animator>().SetBool("Die", true);
		yield return new WaitForSeconds(1);
		GetComponent<Animator>().SetBool("Die", false);
		deadPlaying = false;

		if(lives <= 0)	GUINav.getScores();
		else			GM.ResetScene();
	}

	void animate()
	{
		Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	bool valid(Vector2 dir)
	{
		// cast line from 'next to pacman' to pacman
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos+dir, pos);
		return hit.collider.name == "pacdot" ? true : (hit.collider == collider2D);
	}

	static public void LoseLife()
	{
		lives--;
		UI.UpdateLives(lives);

		GameManager.gameState = GameManager.GameState.Dead;
	}

	public void resetDestination()
	{
		dest = new Vector2(15f, 11f);
		GetComponent<Animator>().SetFloat("DirX", 1);
		GetComponent<Animator>().SetFloat("DirY", 0);
	}

	void readInputAndMove()
	{
		// move closer to destination
		Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
		rigidbody2D.MovePosition(p);
		
		// Check for Input if not moving
		if ((Vector2)transform.position == dest) {
			if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
				dest = (Vector2)transform.position + Vector2.up;
			if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
				dest = (Vector2)transform.position + Vector2.right;
			if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
				dest = (Vector2)transform.position - Vector2.up;
			if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
				dest = (Vector2)transform.position - Vector2.right;
		}
	}

}
