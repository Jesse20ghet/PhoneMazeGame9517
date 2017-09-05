using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrinkScript : MonoBehaviour {

	int currentState = 0;

	float startScale;
	float finishScale;
	public float timeBetweenScalingUp = .05F;
	public float timeBetweenScalingDown = .05F;
	float nextScaleIncrease;

	public float scaleIncrease = 1.05F;
	public float scaleDecrease = .99F;
	float finishScaleCoEfficient = 1.1F;

	float scaleTime;

	// Use this for initialization
	void Start()
	{
		nextScaleIncrease = Time.time;
		startScale = transform.localScale.x;

		finishScale = transform.localScale.x * finishScaleCoEfficient;
	}

	// Update is called once per frame
	void Update()
	{
		switch (currentState)
		{
			case 0:
				if (Time.time > nextScaleIncrease)
				{
					var newScale = new Vector3(transform.localScale.x * scaleIncrease, transform.localScale.y * scaleIncrease);
					transform.localScale = newScale;

					nextScaleIncrease = Time.time + timeBetweenScalingUp;

					if (transform.localScale.x >= finishScale)
					{
						currentState++;
					}
				}
				break;

			case 1:
				if (Time.time > nextScaleIncrease)
				{
					var newScale = new Vector3(transform.localScale.x * scaleDecrease, transform.localScale.y * scaleDecrease);

					if (newScale.x < startScale)
						newScale = new Vector3(startScale, startScale);

					transform.localScale = newScale;

					nextScaleIncrease = Time.time + timeBetweenScalingDown;

					if (transform.localScale.x <= startScale)
						currentState--;
				}
				break;
		}
	}
}
