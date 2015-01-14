using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

	static public int score;
	int high;

	public Image[] lives;

	Text txt_score, txt_high;
	
	// Use this for initialization
	void Start () 
	{
		txt_score = GetComponentsInChildren<Text>()[1];
		txt_high = GetComponentsInChildren<Text>()[0];
		
		high = Scores.High();
		score = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// update score text
		txt_score.text = "Score\n" + score;
		txt_high.text = "High Score\n" + high;

	}

	public void UpdateLives(int index)
	{
		if(index >= 0)	lives[index].enabled = false;
		else 			Debug.Log ("GAME OVER");
	}

}
