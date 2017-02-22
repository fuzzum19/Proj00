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
        public Vector2 myMovedPos;
        // public Animator revive_thisAnim;
    }

    public ReviveScreen[] myReviveScreenClass;

    //========================================================================//

    [Space(10)]
    [Header(" --- ResultsScreen UI --- ")]
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
    }

    public ResultsScreen[] myResultsScreenClass;

    //========================================================================//

    [Space(20)]
    [Header(" --- Loading Screen Settings --- ")]
    public Image loaderOne;
    public Image loaderTwo;
    public Image loaderThree;
    public Image loaderFour;
    public ObjectPooler[] poolToDisable;

    //========================================================================//

	[Space(20)]
	[Header(" --- Environment Spawn Settings --- ")]
	public Transform maxHeightPlatforms;
	public Transform platformGeneratorPos;
	public Transform platformGenRange;

	[Space(20)]
	[Header(" --- Environment Pooling View --- ")]
    [Range(0f, 1.0f)]
    public float level_1_cactus_00_difficulty;
    [Range(0f, 1.0f)]
    public float level_1_cactus_01_difficulty;
	public ObjectPooler[] thePlatformPools;
    public ObjectPooler[] myRightCactusList;

    #region EndPool
    [Space(20)]
    [Header(" --- EndPool --- ")]
    public GameObject[] endPools;
    #endregion

	#region Parallax Pool
	[Space(20)]
	[Header(" --- Foreground & Background Leaps --- ")]
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
	public GameObject[] backgroundOneLeap;
    public GameObject[] backgroundTwoLeap;
    public GameObject coinPooler;
	#endregion

    #region GUI
	[Space(20)]
	[Header(" --- InGame Screen Elements --- ")]
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
    // LerpMyBlur()
    public Animator playerAnimator;
    public BlurOptimized myBlur;
    public bool isBlurEnable { get; set; }


    // Use this for initialization
	void Awake ()
	{
		if (groundPoolerRange == null)
		{
			groundPoolerRange = GameObject.Find ("GroundPoolerRange");
		}	
	}
}
