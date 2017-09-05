using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleContainerSelectScript : MonoBehaviour {

	public void ClearPuzzles()
	{
		foreach (Transform child in this.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

	public void ResizeContainer(int puzzlesCount)
	{
		var gridLayoutGroup = GetComponent<GridLayoutGroup>();
		var yCellSize = gridLayoutGroup.cellSize.y;
		var ySpacing = gridLayoutGroup.spacing.y;

		var newHeight = (int)((puzzlesCount * yCellSize) + (puzzlesCount * ySpacing));

		var rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
	}

	public void ScrollToTop()
	{
		GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
	}
}
