using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private string TopScoresURL = "http://ilbeyli.byethost18.com/pacman/topscores.php";
    private string username;
    private int _highscore;
    private int _lowestHigh;
    private bool _scoresRead;

    public class Score
    {
        public string name { get; set; }
        public int score { get; set; }

        public Score(string n, int s)
        {
            name = n;
            score = s;
        }

        public Score(string n, string s)
        {
            name = n;
            score = Int32.Parse(s);
        }
    }

    List<Score> scoreList = new List<Score>(10);

    void OnLevelWasLoaded(int level)
    {
        StartCoroutine("ReadScoresFromDB");

        if (level == 2) StartCoroutine("UpdateGUIText");    // if scores is loaded
        if (level == 1) StartCoroutine("GetHighestScore");  // if game is loaded
    }

    IEnumerator GetHighestScore()
    {
        Debug.Log("GETTING HIGHEST SCORE");
        // wait until scores are pulled from database
        float timeOut = Time.time + 2;
        while (!_scoresRead)
        {
            yield return new WaitForSeconds(0.01f);
            if (Time.time > timeOut)
            {
                Debug.Log("Timed out");
                scoreList.Clear();
                scoreList.Add(new Score("DATABASE CONNECTION TIMED OUT", 0));
                break;
            }
        }

        _highscore = scoreList[0].score;
        _lowestHigh = scoreList[scoreList.Count - 1].score;
    }

    IEnumerator UpdateGUIText()
    {
        // wait until scores are pulled from database
        float timeOut = Time.time + 2;
        while (!_scoresRead)
        {
            yield return new WaitForSeconds(0.01f);
            if (Time.time > timeOut)
            {   
                Debug.Log("TIMEOUT!");
                scoreList.Clear();
                scoreList.Add(new Score("DATABASE CONNECTION TIMED OUT", 99999));
                break;
            }
        }

        GameObject.FindGameObjectWithTag("ScoresText").GetComponent<Scores>().UpdateGUIText(scoreList);
    }

    IEnumerator ReadScoresFromDB()
    {
        WWW GetScoresAttempt = new WWW(TopScoresURL);
        yield return GetScoresAttempt;

        if (GetScoresAttempt.error != null)
        {
            Debug.Log(string.Format("ERROR GETTING SCORES: {0}", GetScoresAttempt.error));
        }
        else
        {
            // ATTENTION: assumes query will find table
            string[] textlist = GetScoresAttempt.text.Split(new string[] { "\n", "\t" },
                StringSplitOptions.RemoveEmptyEntries);

            string[] Names = new string[Mathf.FloorToInt(textlist.Length / 2)];
            string[] Scores = new string[Names.Length];

            //Debug.Log("Textlist length: " + textlist.Length + " DATA: " + textlist[0]);
            for (int i = 0; i < textlist.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Names[Mathf.FloorToInt(i/2)] = textlist[i];
                }
                else Scores[Mathf.FloorToInt(i / 2)] = textlist[i];
            }

            for (int i = 0; i < Names.Length; i++)
            {
                scoreList.Add(new Score(Names[i], Scores[i]));
            }

            _scoresRead = true;
        }

    }

    public int High()
    {
        return _highscore;
    }

    public int LowestHigh()
    {
        return _lowestHigh;
    }
}
