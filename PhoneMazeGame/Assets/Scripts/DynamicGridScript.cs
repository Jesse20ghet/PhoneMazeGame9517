using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DynamicGridScript : MonoBehaviour {

	RectTransform rectTransRef;
	GridLayoutGroup grid;

	public int row;
	public int col;

	void Start()
	{
		rectTransRef = gameObject.GetComponent<RectTransform>();
		grid = gameObject.GetComponent<GridLayoutGroup>();

		ResizeGrid(row, col); // May not need to be here since this is ran on "Start" which doesn't seem to matter in this context
	}

	public void ResizeGrid()
	{
		ResizeGrid(row, col);
	}

	public void ResizeGrid(int width, int height)
	{
		row = width;
		col = height;
		grid.cellSize = new Vector2(rectTransRef.rect.width / width, rectTransRef.rect.width / width);
	}
}
