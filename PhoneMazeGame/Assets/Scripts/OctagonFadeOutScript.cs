using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OctagonFadeOutScript : MonoBehaviour {

	float timeBetweenFades = .02F;
	float nextFadeTime;

	float finalAlphaValue = .2F;
	float startingAlphaValue = 1;

	Image[] imagesToFade;


	// Use this for initialization
	void Start ()
	{
		nextFadeTime = Time.time + timeBetweenFades;
		imagesToFade = GetComponentsInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach(var image in imagesToFade)
		{
			image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - .01F);
		}

		if (imagesToFade[0].color.a < finalAlphaValue)
			Destroy(this);
	}
}
