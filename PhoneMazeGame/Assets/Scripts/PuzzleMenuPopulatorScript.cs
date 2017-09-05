using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Assets.Code.ConcreteClasses;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using Assets.Code;
using Assets.Code.Save;
using Assets.Code.Utilities;

public class PuzzleMenuPopulatorScript : MonoBehaviour {

	int currentPuzzleCounter = 1;
	int puzzleRowCount = 0;

	GameObject puzzleUIElement;
	GameObject puzzleContainer;
	float currentXcoordinate = 0;
	float currentYcoordinate = 0;

	float puzzleUIElementWidth = 0;
	float puzzleUIElementHeight = 0;

	float buffer = 10F;
	string currentCategory;

	public Text title;
	public Sprite FullStarSprite;
	public Text completedTitle;

	PuzzleContainerSelectScript puzzleContainerScript;

	void Start()
	{
		puzzleContainer = GameObject.Find("PuzzlesContainer");
		puzzleContainerScript = puzzleContainer.GetComponent<PuzzleContainerSelectScript>();
	}

	public void PopulatePuzzleMenu()
	{
		puzzleUIElement = (GameObject)Resources.Load("UI/PuzzleUIElement");
		puzzleUIElementWidth = puzzleUIElement.GetComponent<RectTransform>().sizeDelta.x;
		puzzleUIElementHeight = puzzleUIElement.GetComponent<RectTransform>().sizeDelta.y;

		var rectTransform = puzzleContainer.GetComponent<RectTransform>();

		// Load the puzzles under the current category
		currentCategory = PlayerPrefs.GetString("PuzzleCategory");
		var puzzlesRaw = Resources.LoadAll("Puzzles/" + currentCategory);

		// Reset puzzle Coutner to 1 so the gameobjects know what text to value to display
		currentPuzzleCounter = 1;
		title.text = currentCategory;

		puzzleContainerScript.ClearPuzzles();

		// Take all the puzzles loaded from resources and convert them into text assets
		var puzzles = new TextPuzzleParser().ParsePuzzles("Puzzles/" + currentCategory);

		// Create a UI element for each puzzle under the categories
		foreach(var puzzle in puzzles)
		{
			CreatePuzzleUIElement(puzzle);
		}

		// Resize container so that the scrolling behaves properly
		puzzleContainerScript.ResizeContainer(puzzles.Count);

		// Make sure scroll rect is scrolled to the top
		puzzleContainerScript.ScrollToTop();

		var category = Constants.PuzzleCategories.First(x => x.CategoryName == currentCategory);

		completedTitle.text =  category.LevelsCompleted + " / " + category.LevelsAvailable;
	}

	private int GetHeightOfPuzzleContainer(int puzzlesCount, float yCellSize, float ySpacing)
	{
		return (int)((puzzlesCount * yCellSize) + (puzzlesCount * ySpacing));
	}

	private void CreatePuzzleUIElement(Puzzle puzzle)
	{
		var uiElementRef = (GameObject)GameObject.Instantiate(puzzleUIElement, new Vector3(0, 0), new Quaternion());
		uiElementRef.transform.SetParent(puzzleContainer.transform);
		uiElementRef.transform.localPosition = new Vector3(currentXcoordinate, currentYcoordinate);
		uiElementRef.transform.localScale = Vector3.one;

		uiElementRef.transform.Find("Title-PuzzleCount").gameObject.GetComponent<Text>().text = currentPuzzleCounter.ToString();

		if (PuzzlePersistence.IsPuzzleFinished(currentCategory, puzzle.PuzzleName))
		{
			var starRef = uiElementRef.transform.Find("Star").gameObject;
			starRef.GetComponent<Image>().sprite = FullStarSprite;
		}

		// Setup best moves and best time
		var bestMovesRef = uiElementRef.transform.Find("Dynamic-Moves");
		var bestTimeRef = uiElementRef.transform.Find("Dynamic-Time");

		var puzzleSaveClass = PuzzlePersistence.GetBestPuzzle(currentCategory, puzzle.PuzzleName);

		if (puzzleSaveClass != null)
		{
			bestMovesRef.GetComponent<Text>().text = puzzleSaveClass.TurnCount.ToString();
			bestTimeRef.GetComponent<Text>().text = TimeConverter.ConvertSecondsToTimeString(puzzleSaveClass.TimeSpent);
		}

		var levelSelectScript = uiElementRef.GetComponent<PuzzleMenuLevelSelectScript>();
		levelSelectScript.PuzzleCategory = currentCategory;
		levelSelectScript.PuzzleToLoad = puzzle.PuzzleName;

		puzzleRowCount++;
		currentPuzzleCounter++;

		currentXcoordinate += puzzleUIElementWidth + buffer;
	}
}
