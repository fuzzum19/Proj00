using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bass class for all elements in this application
public class RexJumpElement : MonoBehaviour 
{
	// Gives access to the application and all instances.
	// public static RexJumpApplication app;
    /*
    void Awake ()
    {
        app = GameObject.FindObjectOfType<RexJumpApplication>(); 
    }
    */
    static RexJumpApplication _app;

    public static RexJumpApplication app
    {
        get
        {
            if (_app == null)
            {
                _app = GameObject.FindObjectOfType<RexJumpApplication>();
            }
            return _app;
        }
    }

}

// Application Entry Point
public class RexJumpApplication : MonoBehaviour 
{
    public RexJumpModel model;
    public RexJumpView view;
    public RexJumpController controller;   
    public PlayerController pController;
    public CameraParallax myMainCamera;
}

