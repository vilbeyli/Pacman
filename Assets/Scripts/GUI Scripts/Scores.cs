using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class Scores : MonoBehaviour {

	private class Score
	{
		public string name {get; set;}
		public int score {get; set;}

		public Score(string n, int s)
		{
			name = n;
			score = s;
		}
	}

	List<Score> scoreList = new List<Score>(10);
	Text scores_txt;

	// Use this for initialization
	void Start () 
	{
		// read scores from file
		string path = Application.dataPath + "/Data/scores.txt";
		StreamReader stream = new StreamReader(path);

		while(!stream.EndOfStream)
		{
			string line = stream.ReadLine();
			string[] values = line.Split(' ');
			scoreList.Add(new Score(values[1], Int32.Parse(values[0])));
		}

		stream.Close();

		// get text handle
		scores_txt = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		string s = "";
		foreach(Score sc in scoreList)
		{
			if(sc.score < 1000)
				s += sc.score + "\t\t" + sc.name + "\n";
			else
				s += sc.score + "\t" + sc.name + "\n";
		}

		scores_txt.text = s;
	}

	
	// reads highest score from file
	static public int High()
	{

		// open file
		string path = Application.dataPath + "/Data/scores.txt";
		StreamReader stream = new StreamReader(path);

		// read first line and split
		string line = stream.ReadLine();
		string[] values = line.Split(' ');

		// close file
		stream.Close();

		// parse the first value as the highest score
		return Int32.Parse(values[0]);
	}
}
