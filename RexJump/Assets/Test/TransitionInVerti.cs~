using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionInVerti : MonoBehaviour 
{

    private bool SetMove = false;
    public Vector3 sourcePos;
    // public GameObject[] toEnterUI;
    public RectTransform[] toEnterUI;
    public float screenHeight;

    // Use this for initialization
	void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2.0f;
        SetMove = true;
        sourcePos = toEnterUI[0].transform.position;
	}
	
	// Update is called once per frame
	void Update()
    {
        if (SetMove == true)
        {
            for (int i = 0; i < toEnterUI.Length; i++)
            {

                toEnterUI[i].transform.position = new Vector3 (toEnterUI[i].transform.position.x, toEnterUI[i].transform.position.y * -2, toEnterUI[i].transform.position.z);
                Debug.Log(toEnterUI[i].transform.position);
            }

            SetMove = false;
        }
	}
}
