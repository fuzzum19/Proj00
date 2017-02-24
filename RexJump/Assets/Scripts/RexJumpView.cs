using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class RexJumpView : RexJumpElement 
{
    public GameObject player;
	public Camera mainCamera;
    public GameObject startingPlatform;
	public GameObject ourPlatform;
	public GameObject groundPoolerRange;
    public GameObject mountainPoolerRange;
    public GameObject coinPoolerRange;

    //========================================================================//

    [Space(20)]
    [Header(" --- StartScreen UI--- ")]
	[Space(10)]
    public Text scoreText;
    public Text coinText;
    public bool tapToPlayStartLoop = true;

	
	[System.Serializable]
    public class StartScreen
    {
        public CanvasGroup start_thisCanvasGroup;
        public Button start_thisButton;
        public Text start_text;
    }

    public StartScreen[] myStartScreenClass;

	
    [System.Serializable]
    public class GameTitle
    {
        public GameObject titleObj;
        // public CanvasGroup titleCG;
        public SpriteRenderer titleSprite;
        public Vector3 startingPos;
    }

    public GameTitle[] myGameTitleClass;

	
    [System.Serializable]
    public class GameModesText
    {
        public RectTransform modeRectTransform;
        public CanvasGroup modeCG;
        public Text modeText;
    }

    [Space(10)]
    public GameModesText[] myGameModesText;

    [Space(10)]
    [ReadOnlyAttribute]
    public Vector2 gameModeStartingPos;
    [ReadOnlyAttribute]
    public Vector2 gameModeMoveLeft;
    [ReadOnlyAttribute]
    public Vector2 gameModeMoveRight;

    //========================================================================//

    [Space(10)]
    [Header(" --- FailScreen UI --- ")]
	[Space(10)]
    public bool failScreenStartLoop = true;


    [System.Serializable]
    public class FailScreen
    {
        public RectTransform fail_thisRectTransform;
        public CanvasGroup fail_thisCanvasGroup;
        public Button fail_thisButton;
        public Text fail_thisText;
    }

    public FailScreen[] myFailScreenClass;

    //========================================================================//

    [Space(10)]
    [Header(" --- ReviveScreen UI --- ")]
	[Space(10)]
    public bool reviveScreenStartLoop = true;
    [ReadOnlyAttribute]
    public float reviveUIPercentToMove_X;
    [ReadOnlyAttribute]
    public float reviveUIPercentToMove_Y;

    public Image regressBar;
    public float barTimeAmount;
    [ReadOnlyAttribute]
    public float barTime;


    [System.Serializable]
    public class ReviveScreen
    {
        public RectTransform revive_thisRectTransform;
        public CanvasGroup revive_thisCanvasGroup;
        public Button revive_thisButton;
        public Text revive_thisText;
        [ReadOnlyAttribute]
        public Vector2 myReviveLastPos;
		[ReadOnlyAttribute]
        public Vector2 myReviveMovedPos;
        // public Animator revive_thisAnim;
    }

    public ReviveScreen[] myReviveScreenClass;

    //========================================================================//

    [Space(20)]
    [Header(" --- ResultsScreen UI --- ")]
	[Space(10)]
    public bool resultScreenStartLoop = true;
    [ReadOnlyAttribute]
    public float resultsUIPercentToMove_X;
    [ReadOnlyAttribute]
    public float resultsUIPercentToMove_Y;

    [System.Serializable]
    public class ResultsScreen
    {
        public RectTransform results_thisRectTransform;
        public CanvasGroup results_thisCanvasGroup;
        public Button results_thisButton;
        public Text results_thisText;
        [ReadOnlyAttribute]
        public Vector2 myResultsLasPos;
        [ReadOnlyAttribute]
        public Vector2 myResultsMovePos;
    }

    public ResultsScreen[] myResultsScreenClass;

    //========================================================================//

    [Space(20)]
    [Header(" --- Loading Screen Settings --- ")]
	[Space(10)]
    public Image loaderOne;
    public Image loaderTwo;
    public Image loaderThree;
    public Image loaderFour;
    public ObjectPooler[] poolToDisable;
//    [ReadOnlyAttribute]
//    public bool gameIsLoading;

    //========================================================================//

	[Space(20)]
	[Header(" --- Environment Spawn Settings --- ")]
	[Space(10)]
	public Transform maxHeightPlatforms;
	public Transform platformGeneratorPos;
	public Transform platformGenRange;

	[Space(20)]
	[Header(" --- Environment Pooling View --- ")]
	[Space(10)]
    [Range(0f, 1.0f)]
    public float level_1_cactus_00_difficulty;
    [Range(0f, 1.0f)]
    public float level_1_cactus_01_difficulty;
	public ObjectPooler[] thePlatformPools;
    public ObjectPooler[] myRightCactusList;

    #region EndPool
    [Space(20)]
    [Header(" --- EndPool --- ")]
	[Space(10)]
    public GameObject[] endPools;
    #endregion

	#region Parallax Pool
	[Space(20)]
	[Header(" --- Foreground & Background Leaps --- ")]
	[Space(10)]

	public GameObject endlessScroller;

	[System.Serializable]
	public class ForegroundOneSettings
	{
		public GameObject foregroundOneLeap;
		[ReadOnlyAttribute]
		public Vector3 foregroundOneStartingPos;
	}
	public ForegroundOneSettings[] myFGOne;


	[System.Serializable]
	public class ForegroundTwoSettings
	{
		public GameObject foregroundTwoLeap;
		[ReadOnlyAttribute]
		public Vector3 foregroundTwoStartingPos;
	}
	public ForegroundTwoSettings[] myFGTwo;

	public Transform mountainGeneratorPos;


	[System.Serializable]
	public class BackgroundOneSettings
	{
		public GameObject backgroundOneLeap;
		[ReadOnlyAttribute]
		public Vector3 backgroundOneStartingPos;
	}
	public BackgroundOneSettings[] myBGOne;


	[System.Serializable]
	public class BackgroundTwoSettings
	{
		public GameObject backgroundTwoLeap;
		[ReadOnlyAttribute]
		public Vector3 backgroundTwoStartingPos;
	}
	public BackgroundTwoSettings[] myBGTwo;

	[Space(10)]
    public GameObject coinPooler;
	#endregion

    #region GUI
	[Space(20)]
	[Header(" --- InGame Screen Elements --- ")]
	[Space(10)]
	public Text hiScoreText;

    
    #endregion

    // PlatformGeneration()
    public GameObject newPlatform { get; set; }

    // Sets of Cactus

    // CoinGeneration()
    public GameObject bigCoin01 { get; set; }
    public Renderer renBigCoin1 { get; set; }

    [Space(20)]
    [Header(" --- PostImage Effects View --- ")]
	[Space(10)]
    // LerpMyBlur()
    public Animator playerAnimator;
    public BlurOptimized myBlur;

    // Use this for initialization
	void Awake ()
	{
		if (groundPoolerRange == null)
		{
			groundPoolerRange = GameObject.Find ("GroundPoolerRange");
		}	
	}
}
