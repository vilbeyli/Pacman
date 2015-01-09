using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	private bool paused = false;
	private bool quit = false;

	public Canvas PauseCanvas;
	public Canvas QuitCanvas;
	public Button MenuButton;

	// Use this for initialization
	void Start () 
	{
	
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
}
