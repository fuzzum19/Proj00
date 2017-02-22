using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : RexJumpElement
{

    void OnTriggerEnter2D(Collider2D colPlayer)
    {
        // app.controller.myGameStates = RexJumpController.GameStates.PlayerFailed;


        if (colPlayer.gameObject.tag == "killPlayer")
		{
            app.model.originalCameraPosition = app.view.mainCamera.transform.position;
            InvokeRepeating("CameraShake", 0, .05f);
            Invoke("StopShaking", 0.3f);
            app.model.myUIStateSelector = 3;
		}
	}

    void CameraShake()
    {
        if(app.model.cameraShakeAmt > 0) 
        {
            float quakeAmt = Random.value*app.model.cameraShakeAmt*app.model.shakeMultiplier - app.model.cameraShakeAmt;
            // float quakeAmt = Random.value*app.model.cameraShakeAmt*app.model.shakeMultiplier;
            Vector3 myCamPos = app.view.mainCamera.transform.position;
            myCamPos = new Vector3(myCamPos.x + quakeAmt, myCamPos.y + quakeAmt, myCamPos.z);
            app.view.mainCamera.transform.position = myCamPos;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        app.view.mainCamera.transform.position = app.model.originalCameraPosition;
    }
}
