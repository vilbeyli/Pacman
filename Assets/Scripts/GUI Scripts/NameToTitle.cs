using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameToTitle : MonoBehaviour {

	public Text title;


	void OnMouseEnter()
	{
		switch(name)
		{
		case "Pac-Man":
			title.color = Color.yellow;
			break;

		case "Blinky":
			title.color = Color.red;
			break;

		case "Pinky":
			title.color = new Color(254f/255f, 152f/255f, 203f/255f);
			break;

		case "Inky":
			title.color = Color.cyan;
			break;

		case "Clyde":
			title.color = new Color(254f/255f, 203f/255f, 51f/255f);
			break;
		}
		
		title.text = name;
	}

	void OnMouseExit()
	{
		title.text = "Pac-Man Clone";
		title.color = Color.white;
	}
}
