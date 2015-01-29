using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

	int high, score;

	public Image[] lives;

	Text txt_score, txt_high;
	
	// Use this for initialization
	void Start () 
	{
		txt_score = GetComponentsInChildren<Text>()[1];
		txt_high = GetComponentsInChildren<Text>()[0];
		
		high = Scores.High();
	}
	
	// Update is called once per frame
	void Update () 
	{
        // update score text
        score = GameManager.score;
		txt_score.text = "Score\n" + score;
		txt_high.text = "High Score\n" + high;

	}

	public void UpdateLives(int index)
	{
		if(index >= 0)	lives[index].enabled = false;
		else 			Debug.Log ("GAME OVER");
	}

}
