using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HighlightPathScript : MonoBehaviour
{
	float timeBetweenAction = .4F;
	float nextActionTime;

	private int beginningIter = 0;
	private int endingIter;

	private bool highlightingFinished = false;
	private bool octagonsUneven = false;
	
	public List<GameObject> correctPath = new List<GameObject>();
	public List<GameObject> otherOctagons = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{
		nextActionTime = Time.time + timeBetweenAction;
		endingIter = correctPath.Count - 1;

		foreach(var otherOctagon in otherOctagons)
		{
			otherOctagon.AddComponent<OctagonFadeOutScript>();
		}

		octagonsUneven = correctPath.Count % 2 == 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.time > nextActionTime)
		{
			correctPath[beginningIter].transform.Find("Background").GetComponent<Image>().color = Color.white;
			correctPath[beginningIter].AddComponent<OctagonWinScript>();

			if (endingIter > beginningIter) // We've traversed the entire path
			{
				correctPath[endingIter].transform.Find("Background").GetComponent<Image>().color = Color.white;
				correctPath[endingIter].AddComponent<OctagonWinScript>();
			}
			else if(!octagonsUneven) // If we have an even amount of octagons, we good
			{
				highlightingFinished = true;
			}
			else if( octagonsUneven) // If not, we need to highlight the last octagon
			{
				correctPath[beginningIter].transform.Find("Background").GetComponent<Image>().color = Color.white;
				correctPath[beginningIter].AddComponent<OctagonWinScript>();
				highlightingFinished = true;
			}
			
			nextActionTime += timeBetweenAction;
			beginningIter++;
			endingIter--;
		}

		if (highlightingFinished)
		{
			GameObject.Find("WinDialog").GetComponent<Animator>().SetBool("Show", true);
			GameObject.Find("WinDialog").transform.Find("CanvasGroup").gameObject.SetActive(true);
			Destroy(this);
		}
	}
}
