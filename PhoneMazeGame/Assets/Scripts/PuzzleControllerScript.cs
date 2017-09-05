using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Assets.Code.ConcreteClasses;
using System.IO;
using Assets.Code;
using Assets.Code.Utilities;
using System;
using Assets.Code.Save;
using UnityEngine.SceneManagement;

public class PuzzleControllerScript : MonoBehaviour
{
	private List<Puzzle> PuzzleList;
	private List<GameObject> OctagonsList = new List<GameObject>();
	private int currentPuzzleIterator = 0;

	private int currentOctagonNumber = 0;
	public string CurrentPuzzleName;
	public string CurrentPuzzleCategory;
	private bool puzzleBeaten = false;

	public GameObject puzzleContainer;
	public GameObject puzzleNameRef;
	public GameObject puzzleCategoryRef;
	public Text timerRef;
	public GameObject movesRef;
	public GameObject actionController;
	public GameObject dynamicPuzzleStatus;
	public GameObject winDialog;
	public GameObject resetButton;
	public GameObject hintButton;

	// WinDialogTexts
	public Text winDialogCurrentTime;
	public Text winDialogCurrentMoves;
	public Text winDialogBestTime;
	public Text winDialogBestMoves;
	public Text winDialogHintsText;
	public Text winDialogCelebratoryText;

	public Sprite rotateSprite;
	public Sprite swapSprite;
	public Sprite lockedSprite;
	public Sprite correctSprite;
	
	public Sprite NonWallSprite;

	private GameObject adController;

	// Use this for initialization
	void Start()
	{
		puzzleContainer = GameObject.Find("CurrentPuzzleContainer");
		adController = GameObject.Find("AdController");

		CurrentPuzzleCategory = PlayerPrefs.GetString("Category");
		CurrentPuzzleName = PlayerPrefs.GetString("PuzzleName");

		winDialog.transform.Find("CanvasGroup").gameObject.SetActive(false);

		PuzzleList = new TextPuzzleParser().ParsePuzzles("Puzzles/" + CurrentPuzzleCategory);

		currentPuzzleIterator = PuzzleList.FindIndex(x => x.PuzzleName == CurrentPuzzleName);
		InstantiatePuzzleInGameWorld(PuzzleList[currentPuzzleIterator]);
		ResetLevelToDefaults();
	}

	private void ResetLevelToDefaults()
	{
		//winDialog.transform.Find("CanvasGroup").gameObject.SetActive(false);
		winDialog.GetComponent<Animator>().SetBool("Show", false);
		resetButton.GetComponent<Button>().interactable = true;
		SetPuzzleStatusText(false);
		actionController.GetComponent<ActionControllerScript>().Reset();
		actionController.GetComponent<ActionControllerScript>().EnableActions();
		timerRef.GetComponent<TimeUpdaterScript>().ResetTimer();
		timerRef.GetComponent<TimeUpdaterScript>().Resume();

		var x = winDialog.transform.Find("CanvasGroup").Find("NextButton").gameObject;
		x.GetComponent<Button>().interactable = true;
		x.transform.Find("Text").GetComponent<Text>().text = "Next Puzzle";

		PlayerPrefs.SetString("PuzzleName", PuzzleList[currentPuzzleIterator].PuzzleName);

		puzzleBeaten = false;
		adController.GetComponent<InterstitialAdScript>().PrepareInterstitalAd();
		hintButton.transform.Find("Text").GetComponent<Text>().text = "Hint: " + HintSystem.GetAvailableHints();
		winDialogHintsText.gameObject.SetActive(false);

	}

	public void UpdateText()
	{
		puzzleNameRef.GetComponent<Text>().text = CurrentPuzzleName;
		puzzleCategoryRef.GetComponent<Text>().text = CurrentPuzzleCategory;
	}

	public void InstantiatePuzzleInGameWorld(Puzzle puzzleToInstantiate)
	{
		// Clear old puzzle if one exists
		foreach (Transform child in puzzleContainer.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		// Clear octagons list because we will fill it up with new octagons from the new puzzle
		OctagonsList.Clear();

		// Allow user to click
		//actionController.GetComponent<ActionControllerScript>().EnableActions();

		puzzleToInstantiate.Octagons = puzzleToInstantiate.Octagons.OrderBy(x => x.YCoordinate).ThenBy(x => x.XCoordinate).ToArray();

		// Set columns and rows of grid container
		var dynamicGridRef = puzzleContainer.GetComponent<DynamicGridScript>();
		dynamicGridRef.ResizeGrid(puzzleToInstantiate.Width, puzzleToInstantiate.Height);

		// Now go through and instantiate all octagons for this puzzle
		foreach (var octagon in puzzleToInstantiate.Octagons)
		{
			// Create appropriate game object type - Could be 3 exit, 4 exit, swap
			var octagonGameObject = GetAppropriateGameObjectOctagon(octagon.Type);
			var octagonGameObjectControllerScriptRef = octagonGameObject.GetComponent<OctagonControllerScript>();

			octagonGameObjectControllerScriptRef.XCoordinate = octagon.XCoordinate;
			octagonGameObjectControllerScriptRef.YCoordinate = octagon.YCoordinate;
			octagonGameObjectControllerScriptRef.octagonType = octagon.Type;
			octagonGameObjectControllerScriptRef.OctagonId = currentOctagonNumber++;

			while (octagon.Rotations >= 8)
				octagon.Rotations -= 8;

			octagonGameObjectControllerScriptRef.CorrectRotation = octagon.Rotations * 45;
			octagonGameObjectControllerScriptRef.CorrectSprite = correctSprite;

			// Set the gameobject to the appropriate octagon action
			if (octagonGameObjectControllerScriptRef != null)
			{
				octagonGameObjectControllerScriptRef.octagonAction = octagon.Action;
				octagonGameObjectControllerScriptRef.octagonColor = octagon.Color;
			}

			// Set actual color of octagon
			if (octagon.Type != Enumerations.OctagonType.Empty)
				SetVisibleColor(octagonGameObject, octagonGameObjectControllerScriptRef);

			var positionToInstantiate = new Vector3(octagon.XCoordinate, octagon.YCoordinate, 0);
			var objectRotation = GetOctagonRotation(); // Might be trickyish, start rotating could be a 45x, x being the offset, up to 315

			var gameObjectRef = (GameObject)Instantiate(octagonGameObject, positionToInstantiate, objectRotation);
			OctagonsList.Add(gameObjectRef);

			var gameObjectWidth = gameObjectRef.GetComponent<RectTransform>().rect.width;
			gameObjectRef.transform.SetParent(puzzleContainer.transform, false);
			gameObjectRef.transform.localScale = GetScale(gameObjectRef, puzzleToInstantiate.Width, gameObjectWidth);

			ShowAppropriateIcon(gameObjectRef);

			RemoveWalls(gameObjectRef, octagon);
		}

		// Now that all the octagons are in the game world, go through them and wire up their associated tiles
		foreach (var octagonGameObject in OctagonsList)
		{
			var octagonGameObjectScriptRef = octagonGameObject.GetComponent<OctagonControllerScript>();
			if (octagonGameObjectScriptRef.octagonColor != Enumerations.OctagonColor.Default &&
				octagonGameObjectScriptRef.octagonColor != Enumerations.OctagonColor.Locked)
			{
				octagonGameObjectScriptRef.linkedOctagons.AddRange(OctagonsList.Where(x => x.GetComponent<OctagonControllerScript>().octagonColor == octagonGameObjectScriptRef.octagonColor
																&& x != octagonGameObject));
			}
		}

		puzzleContainer.GetComponent<GridLayoutGroup>().enabled = true;

		UpdateText();
	}

	private void ShowAppropriateIcon(GameObject gameObjectRef)
	{
		var octControllerScript = gameObjectRef.GetComponent<OctagonControllerScript>();
		switch (octControllerScript.octagonType)
		{
			case Enumerations.OctagonType.Normal:
			case Enumerations.OctagonType.CorrectPath:

				ChangeAlpha(gameObjectRef, false);

				if (octControllerScript.octagonColor == Enumerations.OctagonColor.Locked)
				{
					gameObjectRef.transform.Find("Image").GetComponent<Image>().sprite = lockedSprite;
					gameObjectRef.transform.Find("Image").GetComponent<Image>().enabled = true;
					break;
				}

				switch (octControllerScript.octagonAction)
				{
					case Enumerations.OctagonAction.Turn:
						// If they have icons turned on, show the icons. If not, don't.
						if (SettingsPersistence.GetValueBySetting(Enumerations.Setting.OctagonShowIcons) == 1)
						{
							gameObjectRef.transform.Find("Image").GetComponent<Image>().sprite = rotateSprite;
							ChangeAlpha(gameObjectRef, true);
						}
						break;

					case Enumerations.OctagonAction.Swap:
						ChangeAlpha(gameObjectRef, true);
						gameObjectRef.transform.Find("Image").GetComponent<Image>().sprite = swapSprite;
						break;
				}
				break;
		}
	}

	public void ShowHint()
	{
		var correctPathOctagonsNotCorrected = OctagonsList.Where(x => x.GetComponent<OctagonControllerScript>().octagonType == Enumerations.OctagonType.CorrectPath
																&& x.GetComponent<OctagonControllerScript>().IsCorrect == false).ToList();

		if (HintSystem.GetAvailableHints() <= 0)
			return; // CHANGE THIS TO A WAY TO GET MORE HINTS

		// If there are no more octagons to show correctly or there are no available hints
		if (correctPathOctagonsNotCorrected.Count == 0 )
			return;

		// Get a correct path octagon that hasn't already been made into a hint
		var randomInt = UnityEngine.Random.Range(0, correctPathOctagonsNotCorrected.Count); // 2nd parameter of random.range is exlcusive so no - 1
		var octagonToCorrect = correctPathOctagonsNotCorrected[randomInt];

		octagonToCorrect.GetComponent<OctagonControllerScript>().CorrectRotationFromHint();

		actionController.GetComponent<ActionControllerScript>().IncreaseActionCountByValue(1);
		HintSystem.DecrementHints(1);
		hintButton.transform.Find("Text").GetComponent<Text>().text = "Hint: " + HintSystem.GetAvailableHints();
	}

    public bool CheckForWin()
	{
		var endPoints = OctagonsList.Where(x => x.GetComponent<OctagonControllerScript>().octagonType == Enumerations.OctagonType.Endpoint).ToList();

		var nonEmptyOctagons = OctagonsList.Where(x => x.GetComponent<OctagonControllerScript>().octagonType != Enumerations.OctagonType.Empty).ToList();
		var pathTraverser = new PathTraverser(endPoints[0], endPoints[1], nonEmptyOctagons);

		var levelWon = pathTraverser.CanTraverse();

		if (levelWon)
		{
			LevelWon(pathTraverser.GetCorrectPath());
			//winDialog.GetComponent<Animator>().SetBool("Show", true);

			if (currentPuzzleIterator == PuzzleList.Count - 1)
			{
				var x = winDialog.transform.Find("CanvasGroup").Find("NextButton").gameObject;
				x.transform.Find("Text").GetComponent<Text>().text = "Category Complete";
			}

			return true;
		}
        else if(false) // TODO: GET THIS SHIT TO WORK WILL
        {
            // Delete old pathing lines
            foreach (var octagon in OctagonsList)
                foreach (var lr in octagon.GetComponents<LineRenderer>())
                    Destroy(lr);

            // Trace lines from the endpoint the path traverser already used
            TraceCurrentPaths(pathTraverser.GetConnectedPathways());

            // Trace lines from the other endpoint
            pathTraverser = new PathTraverser(endPoints[1], endPoints[0], nonEmptyOctagons);
            pathTraverser.CanTraverse();
            TraceCurrentPaths(pathTraverser.GetConnectedPathways());
        }

		return false;
	}

    private void TraceCurrentPaths(Dictionary<GameObject, List<Vector3>> nodeConnectionDict)
    {
        foreach (var kvp in nodeConnectionDict)
        {
            var octagon = kvp.Key;
            var center = octagon.GetComponent<Renderer>().bounds.center;
            
            foreach (var endpoint in kvp.Value)
            {
                var lr = octagon.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.material = new Material(Shader.Find("Particles/Additive"));
                lr.startColor = Color.white;
                lr.endColor = Color.white;
                lr.startWidth = 10;
                lr.endWidth = 10;

                // THESE RETURN FUCKING ZEROES
                Debug.Log("start");
                Debug.Log(center);
                Debug.Log("end");
                Debug.Log(endpoint);

                lr.SetPosition(0, center);
                lr.SetPosition(1, endpoint);
            }
        }
    }

    public void PreviousPuzzle()
	{
		if (currentPuzzleIterator > 0)
		{
			currentPuzzleIterator--;

			var puzzleToInstantiate = PuzzleList[currentPuzzleIterator];
			CurrentPuzzleName = puzzleToInstantiate.PuzzleName;
			InstantiatePuzzleInGameWorld(puzzleToInstantiate);
			ResetLevelToDefaults();
		}
	}

	private void SetPuzzleStatusText(bool status)
	{
		if (status)
		{
			dynamicPuzzleStatus.GetComponent<Text>().text = "Complete";
			dynamicPuzzleStatus.GetComponent<Text>().color = Color.green;
		}
		else
		{
			dynamicPuzzleStatus.GetComponent<Text>().text = "Incomplete";
			dynamicPuzzleStatus.GetComponent<Text>().color = Color.red;
		}
	}

	public void NextPuzzle()
	{
		if (currentPuzzleIterator < PuzzleList.Count - 1)
		{
			currentPuzzleIterator++;

			var puzzleToInstantiate = PuzzleList[currentPuzzleIterator];
			CurrentPuzzleName = puzzleToInstantiate.PuzzleName;
			//winDialog.transform.Find("CanvasGroup").gameObject.SetActive(false);
			InstantiatePuzzleInGameWorld(puzzleToInstantiate);

			if (puzzleBeaten)
			{
				if (AdSystem.ShouldWeShowAd(CurrentPuzzleCategory))
					GameObject.Find("AdController").GetComponent<InterstitialAdScript>().LaunchInterstitalAd();
			}

			ResetLevelToDefaults();
		}
		else if(puzzleBeaten && currentPuzzleIterator == PuzzleList.Count - 1)// Just beat the last puzzle of the category, send them back to category menu.
		{
			PlayerPrefs.SetString("MenuToShow", "PuzzleCategorySelectorCanvas");
			SceneManager.LoadScene("Main");
		}
	}

	public void ResetPuzzle()
	{
		foreach (var octagon in OctagonsList)
		{
			var octagonControllerScript = octagon.GetComponent<OctagonControllerScript>();

			if (octagonControllerScript.IsCorrect)
				continue;

			switch (octagonControllerScript.octagonAction)
			{
				case Enumerations.OctagonAction.Turn:
					octagon.AddComponent<OctagonRotationResetScript>();
					break;

				case Enumerations.OctagonAction.Swap:
					var octagonSwapScript = octagon.AddComponent<OctagonSwapResetScript>();
					octagonSwapScript.positionToResetTo = octagon.GetComponent<OctagonControllerScript>().startingPositionInWorldSpace;
					break;
			}
		}
	}

	private void LevelWon(List<GameObject> correctPath)
	{
		SetPuzzleStatusText(true);
		puzzleBeaten = true;
		actionController.GetComponent<ActionControllerScript>().DisableActions();

		if (gameObject.GetComponent<HighlightPathScript>() == null)
		{
			gameObject.AddComponent<HighlightPathScript>();
			gameObject.GetComponent<HighlightPathScript>().correctPath = correctPath;
			gameObject.GetComponent<HighlightPathScript>().otherOctagons = GetOtherOctagons(correctPath);
		}

		resetButton.GetComponent<Button>().interactable = false;
		timerRef.GetComponent<TimeUpdaterScript>().Pause();

		SetupWinDialog();

		if (HintSystem.UserGetsAnotherHint(CurrentPuzzleName, CurrentPuzzleCategory))
		{
			HintSystem.IncrementHints(1);
			winDialogHintsText.gameObject.SetActive(true);
		}

		SaveCompletedLevel();
		ProcessCelebratoryText();
	}

	private void ProcessCelebratoryText()
	{
		var randomInt = UnityEngine.Random.Range(0, 10);
		if(randomInt >= 3)
		{
			winDialogCelebratoryText.gameObject.SetActive(true);

			var time = timerRef.gameObject.GetComponent<TimeUpdaterScript>().GetTime();

			if (time > 300) // 300 being 5 minutes
			{
				var phraseToUseIndex = UnityEngine.Random.Range(0, (Constants.WonPuzzlePhraseList_Demeaning.Length - 1));
				winDialogCelebratoryText.text = Constants.WonPuzzlePhraseList_Demeaning[phraseToUseIndex];
			}
			else
			{
				var phraseToUseIndex = UnityEngine.Random.Range(0, (Constants.WonPuzzlePhraseList_Encouraging.Length - 1));
				winDialogCelebratoryText.text = Constants.WonPuzzlePhraseList_Encouraging[phraseToUseIndex];
			}
		}
		else
		{
			winDialogCelebratoryText.gameObject.SetActive(false);
		}
	}

	private List<GameObject> GetOtherOctagons(List<GameObject> correctPath)
	{
		var returnList = new List<GameObject>();

		foreach(var octagon in OctagonsList)
		{
			if (!correctPath.Contains(octagon))
				returnList.Add(octagon);
		}

		return returnList;
	}

	private void SetupWinDialog()
	{
		winDialogCurrentTime.text = TimeConverter.ConvertSecondsToTimeString(timerRef.GetComponent<TimeUpdaterScript>().GetTime());
		winDialogCurrentMoves.text = actionController.GetComponent<ActionControllerScript>().GetActionCount().ToString();

		var bestRun = PuzzlePersistence.GetBestPuzzle(CurrentPuzzleCategory, CurrentPuzzleName);

		if (bestRun != null)
		{
			winDialogBestMoves.text = bestRun.TurnCount.ToString();
			winDialogBestTime.text = TimeConverter.ConvertSecondsToTimeString(bestRun.TimeSpent);
		}
		else
		{
			winDialogBestMoves.text = "---";
			winDialogBestTime.text = "---";
		}

	}

	private void SaveCompletedLevel()
	{
		var newPuzzleClass = new PuzzleSaveClass()
		{
			CategoryName = CurrentPuzzleCategory,
			PuzzleName = CurrentPuzzleName,
			TimeSpent = timerRef.GetComponent<TimeUpdaterScript>().GetTime(),
			TurnCount = actionController.GetComponent<ActionControllerScript>().GetActionCount()
		};

		PuzzlePersistence.Save(newPuzzleClass);
	}

	private void SetVisibleColor(GameObject octagonGameObject, OctagonControllerScript octagonGameObjectControllerScriptRef)
	{
		octagonGameObject.transform.Find("Background").GetComponent<Image>().color =
			EnumToColorConverter.SwapBackgroundColor(octagonGameObjectControllerScriptRef.octagonColor);

		var walls = GetWalls(octagonGameObject);
		foreach (var wall in walls)
		{
			wall.GetComponent<Image>().color = EnumToColorConverter.SwapWallColor(octagonGameObjectControllerScriptRef.octagonColor);
		}
	}

	private void RemoveWalls(GameObject gameObjectRef, Octagon octagon)
	{
		var walls = GetWalls(gameObjectRef);
		foreach (var wall in walls)
		{
			// Check to see if this wall is in the list of this octagons walls.. If not, turn it off
			var wallName = wall.name.Split('-')[0];
			var wallNameEnum = (Enumerations.Wall)Enum.Parse(typeof(Enumerations.Wall), wallName);
			if (!octagon.Walls.Contains(wallNameEnum))
			{
				wall.gameObject.GetComponent<Image>().sprite = NonWallSprite;
				wall.gameObject.GetComponent<WallScript>().active = false;
			}
		}
	}

	private List<GameObject> GetWalls(GameObject parent)
	{
		var children = parent.GetComponentsInChildren<Transform>();

		List<GameObject> returnList = new List<GameObject>();
		foreach (var child in children)
		{
			if (child.tag == "Wall")
				returnList.Add(child.gameObject);
		}

		return returnList;
	}

	private List<GameObject> GetNonWalls(GameObject parent)
	{
		var children = parent.GetComponentsInChildren<Transform>();

		List<GameObject> returnList = new List<GameObject>();
		foreach (var child in children)
		{
			if (child.tag == "Non-Wall")
				returnList.Add(child.gameObject);
		}

		return returnList;
	}

	private Vector3 GetScale(GameObject gameObjectRef, int gridWidthCount, float gameObjectWidth)
	{
		var widthOfPuzzleContainer = puzzleContainer.GetComponent<RectTransform>().rect.width;
		var cellWidth = widthOfPuzzleContainer / gridWidthCount;
		float scale = (cellWidth / gameObjectWidth);

		return new Vector3(scale, scale);

	}

	private Quaternion GetOctagonRotation()
	{
		return new Quaternion(0, 0, 0, 0);
	}

	private GameObject GetAppropriateGameObjectOctagon(Enumerations.OctagonType type)
	{
		GameObject octagonToReturn;

		switch (type)
		{
			case Enumerations.OctagonType.Endpoint:
				octagonToReturn = (GameObject)Resources.Load("Octagons/EndpointOctagon");
				break;

			case Enumerations.OctagonType.Normal:
			case Enumerations.OctagonType.CorrectPath:
				octagonToReturn = (GameObject)Resources.Load("Octagons/DefaultOctagonContainer");
				break;

			case Enumerations.OctagonType.Empty:
			default:
				octagonToReturn = (GameObject)Resources.Load("Octagons/EmptyOctagon");
				break;
		}

		return octagonToReturn;
	}

	private void ChangeAlpha(GameObject objectToChange, bool turnOnAlpha)
	{
		var x = objectToChange.transform.Find("Image").GetComponent<Image>().color;
		if (turnOnAlpha)
			x.a = 1;
		else
			x.a = 0;
		objectToChange.transform.Find("Image").GetComponent<Image>().color = x;
	}
}