using UnityEngine;
using System.Collections;

public class FillCamera : RexJumpElement
{
	/*
	public float maxSize;
	public float minSize;
	public float paraMeter;
	public float finalValue;
	*/

	private float quadHeight;
	private float quadWidth;

	public float ScreenWidth;
	public float ScreenHeight;

	void Start ()
	{
		Rescale();
	}

	void Update ()
	{

	}

	void Rescale ()
	{
		// finalValue = (paraMeter-minSize)/(maxSize-minSize);

		quadHeight = Camera.main.orthographicSize * 2.5f;
		quadWidth = quadHeight * Screen.width / Screen.height;

		transform.localScale = new Vector3 (quadWidth, transform.localScale.y, 1);

		ScreenWidth = Screen.width;
		ScreenHeight = Screen.height;
	}

}
