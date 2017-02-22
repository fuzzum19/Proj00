using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPooler : RexJumpElement
{	
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < app.view.groundPoolerRange.transform.position.x)
        {
            gameObject.SetActive(false);
            /*
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }
            */
		}
	}
}
