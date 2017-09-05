using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuOnClickScript : MonoBehaviour {

	bool pointerHovering = false;

	Color mouseDownColor;
	Color originalColor;

	public GameObject mainMenuCanvasReference;
	public GameObject puzzleMenuCanvasReference;
	public GameObject puzzleCategoriesCanvasReference;

	void Start()
	{
		mouseDownColor = new Color(255, 255, 255);
		originalColor = GetComponent<Text>().color;
	}

	public void PlayOnUpClickMethod()
	{
		if (pointerHovering)
		{
			ChangeColor(originalColor);
			mainMenuCanvasReference.GetComponent<Canvas>().enabled = false;
			puzzleCategoriesCanvasReference.GetComponent<Canvas>().enabled = true;
			puzzleMenuCanvasReference.GetComponent<Canvas>().enabled = false;

			//Load all available categories

		}
	}

	public void OnEnterMethod()
	{
		pointerHovering = true;
	}

	public void OnExitClickMethod()
	{
		ChangeColor(originalColor);
		pointerHovering = false;
	}

	public void OnDownClickMethod()
	{
		ChangeColor(mouseDownColor);
	}

	private void ChangeColor(Color color)
	{
		GetComponent<Text>().color = color;
	}
}
