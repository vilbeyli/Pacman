using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	//------------------------------------------------------------------
	// Variable declarations

	private bool paused = false;
	private bool quit = false;

	// canvas
	public Canvas PauseCanvas;
	public Canvas QuitCanvas;

	// buttons
	public Button MenuButton;

	//------------------------------------------------------------------
	// Singleton implementation


	//------------------------------------------------------------------
	// Function Definitions
	
	void Awake () 
	{
	
	}

	void Update () 
	{
		if(Application.loadedLevelName == "game")
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				if(quit == true)
					ToggleQuit();
				else
					TogglePause();
			}
		}
	}

	void OnLevelWasLoaded(int level)
	{
		switch(level)
		{
		case 0:		// menu

			break;

		case 1:		// game
			paused = quit = false;
			Time.timeScale = 1.0f;
			break;
		}
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

	public void Quit()
	{
		Application.Quit();
	}

	public void Menu()
	{
		Application.LoadLevel("menu");
		Time.timeScale = 1.0f;
	}

	public void Play()
	{
		Application.LoadLevel("game");
	}

	public void HighScores()
	{
		Application.LoadLevel("scores");

	}

	public void SourceCode()
	{
		Application.OpenURL("https://github.com/vilbeyli/Pacman-Clone/");
	}
}
