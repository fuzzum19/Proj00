using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () 
    {
        // transform.localRotation = new Quaternion (transform.localRotation.x, transform.localRotation.y, -45f, 0);
        // transform.rotation = new Quaternion (transform.rotation.x, 0 * 3 * Time.deltaTime, -45f, 0);
        transform.Rotate (0, 45f * Time.deltaTime, 0);

	}
}
