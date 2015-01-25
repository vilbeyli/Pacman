using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameState { Init, Game, Dead, Scores }
	public static GameState gameState;

	public GameObject pacman;
	public GameObject blinky;
	public GameObject pinky;
	public GameObject inky;
	public GameObject clyde;
	public GameObject UIManager;

	private GameGUINavigation guiNav;

	private bool scared = false;

	public float scareLength;
	private float timeToCalm;

	// Use this for initialization
	void Start () 
	{
		guiNav = UIManager.GetComponent<GameGUINavigation>();
		gameState = GameState.Init;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(scared == true && timeToCalm <= Time.time)
			CalmGhosts();
	}

	public void ResetScene()
	{
		pacman.transform.position = new Vector3(15f, 11f, 0f);
		blinky.transform.position = new Vector3(15f, 20f, 0f);
		pinky.transform.position = new Vector3(14.5f, 17f, 0f);
		inky.transform.position = new Vector3(16.5f, 17f, 0f);
		clyde.transform.position = new Vector3(12.5f, 17f, 0f);

		pacman.GetComponent<PlayerController>().resetDestination();
		blinky.GetComponent<GhostMove>().InitializeGhost();
		pinky.GetComponent<GhostMove>().InitializeGhost();
		inky.GetComponent<GhostMove>().InitializeGhost();
		clyde.GetComponent<GhostMove>().InitializeGhost();

		guiNav.H_ShowReadyScreen();
	}

	public void ToggleScare()
	{
		if(!scared)	ScareGhosts();
		else 		CalmGhosts();
	}

	public void ScareGhosts()
	{
		scared = true;
		blinky.GetComponent<GhostMove>().Frighten();
		pinky.GetComponent<GhostMove>().Frighten();
		inky.GetComponent<GhostMove>().Frighten();
		clyde.GetComponent<GhostMove>().Frighten();
		timeToCalm = Time.time + scareLength;
	}

	public void CalmGhosts()
	{
		scared = false;
		blinky.GetComponent<GhostMove>().Calm();
		pinky.GetComponent<GhostMove>().Calm();
		inky.GetComponent<GhostMove>().Calm();
		clyde.GetComponent<GhostMove>().Calm();
	}
}
