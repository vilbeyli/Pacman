using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameGUINavigation : MonoBehaviour {

	//------------------------------------------------------------------
	// Variable declarations
	
	private bool _paused;
    private bool quit;
    private string _errorMsg;
	//public bool initialWaitOver = false;

	public float initialDelay;

	// canvas
	public Canvas PauseCanvas;
	public Canvas QuitCanvas;
	public Canvas ReadyCanvas;
	public Canvas ScoreCanvas;
    public Canvas ErrorCanvas;
    public Canvas GameOverCanvas;
	
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

    public void H_ShowGameOverScreen()
    {
        StartCoroutine("ShowGameOverScreen");
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

    IEnumerator ShowGameOverScreen()
    {
        Debug.Log("Showing GAME OVER Screen");
        GameOverCanvas.enabled = true;
        yield return new WaitForSeconds(2);
        Menu();
    }

	public void getScoresMenu()
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


        Debug.Log("PauseCanvas enabled: " + PauseCanvas.enabled);
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
            QuitCanvas.enabled = true;
			PauseCanvas.enabled = false;
			quit = true;
		}
	}

	public void Menu()
	{
		Application.LoadLevel("menu");
		Time.timeScale = 1.0f;

        // take care of game manager
	    GameManager.DestroySelf();
	}

    IEnumerator AddScore(string name, int score)
    {
        string privateKey = "pKey";
        string AddScoreURL = "http://ilbeyli.byethost18.com/addscore.php?";
        string hash = Md5Sum(name + score + privateKey);

        Debug.Log("Name: " + name + " Escape: " + WWW.EscapeURL(name));

        WWW ScorePost = new WWW(AddScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash );
        yield return ScorePost;

        if (ScorePost.error == null)
        {
            Debug.Log("SCORE POSTED!");

            // take care of game manager
            Destroy(GameObject.Find("Game Manager"));
            GameManager.score = 0;
            GameManager.Level = 0;

            Application.LoadLevel("scores");
            Time.timeScale = 1.0f;
        }
        else
        {
            Debug.Log("Error posting results: " + ScorePost.error);
        }

        yield return new WaitForSeconds(2);
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

	public void SubmitScores()
	{
		// Check username, post to database if its good to go
	    int highscore = GameManager.score;
        string username = ScoreCanvas.GetComponentInChildren<InputField>().GetComponentsInChildren<Text>()[1].text;
        Regex regex = new Regex("^[a-zA-Z0-9]*$");

	    if (username == "")                 ToggleErrorMsg("Username cannot be empty");
        else if (!regex.IsMatch(username))  ToggleErrorMsg("Username can only consist alpha-numberic characters");
        else if (username.Length > 10)      ToggleErrorMsg("Username cannot be longer than 10 characters");
        else                                StartCoroutine(AddScore(username, highscore));
	    
	}

    public void LoadLevel()
    {
        GameManager.Level++;
        Application.LoadLevel("game");
    }

    public void ToggleErrorMsg(string errorMsg)
    {
        if (ErrorCanvas.enabled)
        {
            ScoreCanvas.enabled = true;
            ErrorCanvas.enabled = false;

        }
        else
        {
            ScoreCanvas.enabled = false;
            ErrorCanvas.enabled = true;
            ErrorCanvas.GetComponentsInChildren<Text>()[1].text = errorMsg;

        }
    }
}
