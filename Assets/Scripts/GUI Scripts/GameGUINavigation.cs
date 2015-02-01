using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameGUINavigation : MonoBehaviour {

	//------------------------------------------------------------------
	// Variable declarations
	
	private bool _paused;
	private bool quit = false;
	//public bool initialWaitOver = false;

	public float initialDelay;

	// canvas
	public Canvas PauseCanvas;
	public Canvas QuitCanvas;
	public Canvas ReadyCanvas;
	public Canvas ScoreCanvas;
	
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
			// if scores are show, go back to main menu
			if(GameManager.gameState == GameManager.GameState.Scores)
				Menu();

			// if in game, toggle pause or quit dialogue
			else
			{
				if(quit == true)
					ToggleQuit();
				else
					TogglePause();
			}
		}
	}

	// public handle to show ready screen coroutine call
	public void H_ShowReadyScreen()
	{
		StartCoroutine("ShowReadyScreen", initialDelay);
	}

	IEnumerator ShowReadyScreen(float seconds)
	{
		//initialWaitOver = false;
		GameManager.gameState = GameManager.GameState.Init;
		ReadyCanvas.enabled = true;
		yield return new WaitForSeconds(seconds);
		ReadyCanvas.enabled = false;
		GameManager.gameState = GameManager.GameState.Game;
		//initialWaitOver = true;
	}

	public void getScores()
	{
		Time.timeScale = 0f;		// stop the animations
		GameManager.gameState = GameManager.GameState.Scores;
		MenuButton.enabled = false;
		ScoreCanvas.enabled = true;
	}

	//------------------------------------------------------------------
	// Button functions

	public void TogglePause()
	{
		// if paused before key stroke, unpause the game
		if(_paused)
		{
			Time.timeScale = 1;
			PauseCanvas.enabled = false;
			_paused = false;
			MenuButton.enabled = true;
		}
		
		// if not paused before key stroke, pause the game
		else
		{
			PauseCanvas.enabled = true;
			Time.timeScale = 0.0f;
			_paused = true;
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

        // take care of game manager
        Destroy(GameObject.Find("Game Manager"));
	    GameManager.score = 0;
	    GameManager.Level = 0;
	}

	public void SubmitScores()
	{
		// SUBMIT SCORES TO DATABASE


        // take care of game manager
        Destroy(GameObject.Find("Game Manager"));
        GameManager.score = 0;
        GameManager.Level = 0;

		Application.LoadLevel("scores");
		Time.timeScale = 1.0f;
	}

    public void LoadLevel()
    {
        GameManager.Level++;
        Application.LoadLevel("game");
    }
}
