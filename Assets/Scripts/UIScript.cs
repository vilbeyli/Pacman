using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	int high, score;

	public Image[] lives;

	Text txt_score, txt_high, txt_level;
	
	// Use this for initialization
	void Start () 
	{
		txt_score = GetComponentsInChildren<Text>()[1];
		txt_high = GetComponentsInChildren<Text>()[0];
        txt_level = GetComponentsInChildren<Text>()[2];

		high = Scores.High();

        // update lives
        for (int i = 0; i < lives.Length; ++i)
            lives[i].enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
        // update score text
        score = GameManager.score;
		txt_score.text = "Score\n" + score;
		txt_high.text = "High Score\n" + high;
	    txt_level.text = "Level\n" + (GameManager.Level + 1);

        // update lives
	    for (int i = 0; i < PlayerController.lives; ++i)
	        lives[i].enabled = true;
	}


}
