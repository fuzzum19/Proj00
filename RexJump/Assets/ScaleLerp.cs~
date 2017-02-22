using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLerp : MonoBehaviour {

    private Vector3 myScale;
    private Vector3 finalScale;
    private float duration;

    private float lerpTime = 1f;

    // Use this for initialization
	void Start () 
    {
        // t = Time.deltaTime / duration;
        myScale = new Vector3 (0f, 0f, 0f);
        transform.localScale = myScale;
        finalScale = new Vector3 (1f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        float t = Time.deltaTime / lerpTime;
        // t = Mathf.Sin(t * Mathf.PI * 0.5f); // ease out
        t = t*t*t * (t * (6f*t - 15f) + 10f);
        transform.localScale = Vector3.Lerp(transform.localScale, finalScale, t);
	}
}
