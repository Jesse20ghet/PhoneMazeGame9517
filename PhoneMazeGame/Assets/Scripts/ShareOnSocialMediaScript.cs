using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareOnSocialMediaScript : MonoBehaviour {

	public GameObject timeUpdater;
	public GameObject actionController;
	public GameObject puzzleController;

	string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
	string TWEET_LANGUAGE = "en";

	string AppID = "679792638888652";
	string facebookLink = "https://play.google.com/store/apps/details?id=com.WiJediStudios.Roctagons&hl=en";
	string facebookCaption = "Test Caption";
	string facebookdescription = "Test Description";

	public void shareOnTwitter()
	{
		var moves = actionController.GetComponent<ActionControllerScript>().GetActionCount().ToString();
		var time = timeUpdater.GetComponent<TimeUpdaterScript>().GetTimeAsExtendedString();
		var category = puzzleController.GetComponent<PuzzleControllerScript>().CurrentPuzzleCategory;
		var puzzleName = puzzleController.GetComponent<PuzzleControllerScript>().CurrentPuzzleName;

		var textToDisplay = "Just beat " + puzzleName + " of " + category + ". Check out the game: " + facebookLink; 

		Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(textToDisplay) + "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
	}

	public void shareOnFacebook()
	{
		Application.OpenURL("https://www.facebook.com/dialog/feed?" + "app_id=" + AppID + 
			"&link=" + facebookLink + "&caption=" + facebookCaption + "&description=" + facebookdescription);
	}
}
