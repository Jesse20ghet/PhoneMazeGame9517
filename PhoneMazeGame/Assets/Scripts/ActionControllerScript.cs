using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Utilities;
using UnityEngine.UI;

public class ActionControllerScript : MonoBehaviour {

	List<GameObject> activatedObjects = new List<GameObject>();
	PuzzleControllerScript puzzleController;
	public GameObject movesText;
	public GameObject puzzleContainer;

	int actionCount = 0;

	private bool actionsEnabled = true;

	// Use this for initialization
	void Start ()
	{
		puzzleController = GameObject.Find("PuzzleController").GetComponent<PuzzleControllerScript>();
	}

	public void EnableActions()
	{
		actionsEnabled = true;
	}

	public void DisableActions()
	{
		actionsEnabled = false;
	}

	public void PerformAction(Enumerations.OctagonAction action, List<GameObject> objectsToManipulate)
	{
		if (!actionsEnabled)
			return;

		// Disable grid layout group every time because we need it at the beginning of the scene to lay things out but it causes
		// problems later on when swapping octagons
		puzzleContainer.GetComponent<GridLayoutGroup>().enabled = false;

		var actionResponse = false;

		switch (action)
		{
			case Enumerations.OctagonAction.Turn:
				actionResponse = Rotate(objectsToManipulate);
				break;

			case Enumerations.OctagonAction.Swap:
				actionResponse = Swap(objectsToManipulate);
				break;
		}

		// If we successfully performed an action, increment action count
		if (actionResponse)
		{
			activatedObjects.AddRange(objectsToManipulate);
			objectsToManipulate[0].GetComponent<OctagonControllerScript>().PlaySound();
			actionCount++;
		}

		movesText.GetComponent<Text>().text = actionCount.ToString();
	}

	private bool Rotate(List<GameObject> rotateObjects)
	{
		// If any object is rotating, don't rotate any of them.
		foreach(var rotateObject in rotateObjects)
		{
			if (rotateObject.GetComponent<RotatePieceScript>() != null)
				return false;
		}

		foreach(var rotateObject in rotateObjects)
		{
			rotateObject.AddComponent<RotatePieceScript>();
		}

		return true;
	}

	private bool Swap(List<GameObject> swapObjects)
	{
		// If any of the pieces to swap are currently swapping, we can't swap them again.
		foreach(var swapObject in swapObjects)
		{
			if (swapObject.GetComponent<OctagonSwapScript>() != null)
				return false;
		}

		int firstXCoord = -1;
		int firstYCoord = -1;

		for(int i = 0; i < swapObjects.Count; i++)
		{
			var nextOctagonInt = i + 1;
			if (nextOctagonInt == swapObjects.Count)
				nextOctagonInt = 0;

			var distanceBetween = Vector3.Distance(swapObjects[i].transform.position, swapObjects[nextOctagonInt].transform.position);
			var positionToMoveTowards = swapObjects[nextOctagonInt].transform.position;

			// Update coordinates in grid
			var octagonControllerScript = swapObjects[i].GetComponent<OctagonControllerScript>();
			var nextOctagonControllerScript = swapObjects[nextOctagonInt].GetComponent<OctagonControllerScript>();
			if (i == 0)
			{
				firstXCoord = octagonControllerScript.XCoordinate;
				firstYCoord = octagonControllerScript.YCoordinate;
			}

			octagonControllerScript.XCoordinate = (i == swapObjects.Count - 1) ? firstXCoord : nextOctagonControllerScript.XCoordinate;
			octagonControllerScript.YCoordinate = (i == swapObjects.Count - 1) ? firstYCoord : nextOctagonControllerScript.YCoordinate;

			swapObjects[i].AddComponent<OctagonSwapScript>();
			swapObjects[i].GetComponent<OctagonSwapScript>().distanceBetween = distanceBetween;
			swapObjects[i].GetComponent<OctagonSwapScript>().positionToMoveTowards = positionToMoveTowards;
			swapObjects[i].transform.SetAsLastSibling();
		}

		return true;
	}

	public void Completed(GameObject gameObject)
	{
		if (activatedObjects.Contains(gameObject))
			activatedObjects.Remove(gameObject);

		// If no more objects are currently activated, check for win
		if (activatedObjects.Count == 0)
		{
			puzzleController.CheckForWin();
		}
	}

	public void Reset()
	{
		actionCount = 0;
		movesText.GetComponent<Text>().text = actionCount.ToString();
	}

	public int GetActionCount()
	{
		return actionCount;
	}

	public void ResetActionCount()
	{
		actionCount = 0;
	}

	public void IncreaseActionCountByValue(int x = 0)
	{
		actionCount += x;
		movesText.GetComponent<Text>().text = actionCount.ToString();
	}
}
