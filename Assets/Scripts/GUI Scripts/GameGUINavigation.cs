using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameGUINavigation : MonoBehaviour {

	//------------------------------------------------------------------
	// Variable declarations
	
	private bool paused = false;
	private bool quit = false;
	public bool initialWaitOver = false;

	public float initialDelay;

	// canvas
	public Canvas PauseCanvas;
	public Canvas QuitCanvas;
	public Canvas ReadyCanvas;
	
	// buttons
	public Button MenuButton;

	//------------------------------------------------------------------
	// Function Definitions

	// Use this for initialization
	void Start () 
	{
		StartCoroutine("ShowReadyScreen", initialDelay);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(quit == true)
				ToggleQuit();
			else
				TogglePause();
		}
	}

	IEnumerator ShowReadyScreen(float seconds)
	{
		initialWaitOver = false;
		ReadyCanvas.enabled = true;
		yield return new WaitForSeconds(seconds);
		ReadyCanvas.enabled = false;
		initialWaitOver = true;
	}

	//------------------------------------------------------------------
	// Button functions

	public void TogglePause()
	{
		// if paused before key stroke, unpause the game
		if(paused)
		{
			Time.timeScale = 1;
			PauseCanvas.enabled = false;
			paused = false;
			MenuButton.enabled = true;
		}
		
		// if not paused before key stroke, pause the game
		else
		{
			PauseCanvas.enabled = true;
			Time.timeScale = 0.0f;
			paused = true;
			MenuButton.enabled = false;
		}
	}
	
	public void ToggleQuit()
	{
		if(quit)
		{
			PauseCanvas.enabled = true;
			QuitCanvas.enabled = false;
			quit = false;
		}
		
		else
		{
			PauseCanvas.enabled = false;
			QuitCanvas.enabled = true;
			quit = true;
		}
	}

	public void Menu()
	{
		Application.LoadLevel("menu");
		Time.timeScale = 1.0f;
	}
}
