using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Countains all data related to app

public class RexJumpModel : RexJumpElement 
{   
 
    // Resolution manager
    [ReadOnlyAttribute]
    public float xWidth;
    [ReadOnlyAttribute]
    public float yHeight;
    [ReadOnlyAttribute]
    public float aspectRatio;
    [ReadOnlyAttribute]
    public int widthToLoad;
    [ReadOnlyAttribute]
    public int heightToLoad;

    public int myUIStateSelector { get; set; }
    public GroundPooler[] platformList { get; set; }
    public Text resolutionTest;

    [ReadOnlyAttribute]
    public float sceneStartIdleTime = 0f;
    [ReadOnlyAttribute]
    public float sceneStartNotIdleTime = 0f;
    [ReadOnlyAttribute]
    public float scenePlayTime = 0f;
    [ReadOnlyAttribute]
    public float sceneFailTime = 0f;
    [ReadOnlyAttribute]
    public float sceneReviveTime = 0f;
    [ReadOnlyAttribute]
    public float sceneResultsTime = 0f;
    [ReadOnlyAttribute]
    public float sceneLoadingTime = 0f;
    [ReadOnlyAttribute]
    public float sceneExitLoadingTime = 0f;

    [Space(20)]
    [Header(" --- Starting game values --- ")]
    public Vector3 parallaxVect3StartValues;

    [Space(20)]
    [Header(" --- Game Idle --- ")]
    [ReadOnlyAttribute]
    public float playerIdleTime;
    public bool playerIsIdle { get; set; }
    public bool fadeInFromIdle { get; set; }
    // Used by Event Handler
    public bool playerIsMoving { get; set; }

	[Header(" --- Speed Difficulty --- ")]
	public float speedIncreaseMilestone;
    [ReadOnlyAttribute]
	public float speedIncreaseMilestoneStore;
	public float speedMultiplier;

	[Space(20)]
	[Header(" --- Platform Difficulty --- ")]
    public int levelDifficulty; 

	[Space(20)]
	[Header(" --- Coin Settings --- ")]
	public float coinRotate;
	public int coinsToGive;
	public int coinsCount;
	public int coinMultiplier;
	public int maxCoins;
	public float coinRotationSpeed;

	[Space(20)]
	[Header(" --- Score Settings --- ")]
	public float pointsPerSecond;
	public float scoreCount;
	public float highScoreCount;
    // Used by Event Handler
	public bool scoreIncreasing { get; set; }

	[Space(20)]
	[Header(" --- Parallax Settings --- ")]
	public float foregroundOneLeapSpeed;
	public float foregroundTwoLeapSpeed;
	public float backgroundOneLeapSpeed;
    public float backgroundTwoLeapSpeed;
    public float titleLeapSpeed;
    [ReadOnlyAttribute]
	public Vector3 previousCameraPosition;

	[Space(20)]
	[Header(" --- Platform Settings --- ")]
	public float distanceBetweenPlatforms;
	public float platformWidth;
	public float platformHeight;
    [ReadOnlyAttribute]
	public int thePlatformPoolsSelector;
	public float platform00Threshold;
	public float platformE01Threshold; 

	[Space(20)]
	[Header(" --- Platform Ground Settings --- ")]
	public float brownGroundIndentY;
	public int brownGroundSelector { get; set; }
	public float minHeightRange { get; set; }
	public float maxHeightRange { get; set; }
	// -------------------------------------- //
	[Space(20)]
	[Header(" --- Random Height Platform --- ")]
	public float maxHeightChange;
	public float heightChange;

	// Store Camera Size
	public float camHeight { get; set; }
	public float camWidth { get; set; }
	// -------------------------------------- //
	// -------------------------------------- //
	// -------------------------------------- //
	// Store camera values
	[Space(20)]
	[Header(" --- Camera Settings --- ")]
	public float cameraSpeedFollow;
	public float cameraIndentX;
	public float cameraIndentY;
	public float minCameraClampY; 
	public float maxCameraClampY; 
    
	// Private values for getting starting point
	public Vector3 playerStartpoint { get; set; }
	public Vector3 platformStartpoint { get; set; }
	public Vector3 lastPlayerPos { get; set; }
	public Vector3 positionA { get; set; }
	public Vector3 positionB { get; set; }
	public Vector3 foreground01StartPoint { get; set; }
	public Vector3 foreground02StartPoint { get; set; }
	public float distanceToMove { get; set; }

    // ShakeCam
    public float cameraShakeAmt;
    public float shakeMultiplier;
    public Vector3 originalCameraPosition { get; set; }

    // My String Builder
    public StringBuilder my_strBuilder = new StringBuilder ( 32 );

    public string hiScoreString()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("TOP SCORE ").Append(Mathf.Round(highScoreCount)).Append("m");
        return my_strBuilder.ToString();
    }

    public string norScoreString()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append(Mathf.Round(scoreCount)).Append("m");
        return my_strBuilder.ToString();
    }

    // ====================================================================== //
    // Start screen UI

    public string GameModeClassicString()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("GAME MODE: CLASSIC");
        return my_strBuilder.ToString();
    }

    public string GameModeZenString()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("GAME MODE: ZEN");
        return my_strBuilder.ToString();
    }

    // ====================================================================== //
    // Revive screen UI

    public string WatchAVideo()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("WATCH A VIDEO");
        return my_strBuilder.ToString();
    }

    public string Spend100Coins()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("OR 100 COINS");
        return my_strBuilder.ToString();
    }

    // ====================================================================== //
    /*
    public string ClassicString()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("CLASSIC");
        return my_strBuilder.ToString();
    }

    public string ZenString()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append("ZEN");
        return my_strBuilder.ToString();
    }

    public string GameModeClassic()
    {
        my_strBuilder.Length = 0;
        my_strBuilder.Append(ClassicString());
        return my_strBuilder.ToString();
    }
    */

    // app.view.hiScoreText.text = "HIGH SCORE: " + Mathf.Round (app.model.highScoreCount);

}
