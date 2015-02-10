using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    public ScoreManager scoreManager;

    Text scores_txt;

    private void Start()
    {
        // get text handle
        scores_txt = GetComponent<Text>();
    }

    public void UpdateGUIText(List<ScoreManager.Score> scoreList)
    {
        string s = "";
        foreach (ScoreManager.Score sc in scoreList)
        {
            if (sc.score < 1000)
                s += sc.score + "\t\t" + sc.name + "\n";
            else
                s += sc.score + "\t" + sc.name + "\n";
        }

        scores_txt.text = s;
    }

}
