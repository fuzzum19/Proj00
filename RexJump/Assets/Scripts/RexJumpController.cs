using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Controls the app workflow
public class RexJumpController : RexJumpElement 
{
    public enum GameModes
    {
        Classic, 
        Zen, 
        Level    
    }

    [ReadOnlyAttribute]
    public GameModes myGameModes;

    public enum GameStates
    {
        PlayerIdle,
        PlayerNotIdle,
        PlayerStartedPlaying,
        PlayerIsPlaying,
        PlayerFailed,
        PlayerRevive,
        PlayerResults,
        PlayerLoading,
        PlayerLoadingExit
    }

    [ReadOnlyAttribute]
    public GameStates myGameStates;

    public enum UIStates
    {
        SceneStart,
        SceneStartPlay,
        ScenePlay,
        SceneFail,
        SceneRevive,
        SceneResults,
        SceneLoading,
        SceneLoadingExit
    }

    [ReadOnlyAttribute]
    public UIStates myUIStates;

    //========================================================================//

    void Awake ()
	{
        ResolutionManager();

        // Get Camera Values
		app.model.camHeight = Camera.main.orthographicSize * 2.0f;
		app.model.camWidth = app.model.camHeight * Screen.width / Screen.height;

        app.pController = app.model.GetComponent<PlayerController>();
        app.view.playerAnimator = app.view.player.GetComponent<Animator>(); 
	}

    //========================================================================//

	void Start ()
	{
//        app.model.resolutionTest.text = Screen.currentResolution + " " + SystemInfo.deviceType + " " + SystemInfo.deviceModel;
		app.model.resolutionTest.text = app.model.widthToLoad + " x " + app.model.heightToLoad + " | " + app.model.aspectRatio + " | " + SystemInfo.deviceModel + " | " + SystemInfo.deviceType;

		// ====== StartScreen_UI ===== //

		// Set last player position for the Camera
		app.model.lastPlayerPos = app.view.player.transform.position;

		// Set scoreIncreasing to Off
		app.model.scoreIncreasing = false;

		// Detect if player is moving
		app.model.playerIsMoving = false;

		// Set camera settings
		app.model.cameraSpeedFollow = 1.0f;
		app.model.minCameraClampY = -5f;
		app.model.maxCameraClampY = 5f;

		// Get platform width and height via BoxCollider 2D
		app.model.platformWidth = app.view.emptyPlatform.GetComponent<BoxCollider2D> ().size.x;
		app.model.platformHeight = app.view.emptyPlatform.GetComponent<BoxCollider2D> ().size.y;

		// Set min and max height for randomized height platforms
		app.model.minHeightRange = app.view.platformGeneratorPos.transform.position.y;
		app.model.maxHeightRange = app.view.maxHeightPlatforms.position.y;

		// Get Player Starting Point
		app.model.platformStartpoint = app.view.platformGeneratorPos.position;
		app.model.playerStartpoint = app.view.player.transform.position;

		// Set Revive UI's value to move
		app.view.reviveUIPercentToMove_X = Screen.width * 0.25f;
		app.view.reviveUIPercentToMove_Y = Screen.height / 30f;

		// Set Results UI's value to move
		app.view.resultsUIPercentToMove_X = Screen.width * 0.25f;
		app.view.resultsUIPercentToMove_Y = Screen.height / 10f;


		// MyDebugList();
        
		/*
		if (PlayerPrefs.HasKey ("HighScore"))
		{
			app.model.highScoreCount = PlayerPrefs.GetFloat ("HighScore");
		}

		if (PlayerPrefs.HasKey ("CoinScore"))
		{
			app.model.coinsCount = PlayerPrefs.GetInt ("CoinScore");
		}
		*/
	}

    //========================================================================//

	void Update()
    {
        // ====== StartScreen_UI ===== //
        // Set to Alpha zero
        StartScreenUIAlphaZero();
        InitializeUISelector();
        // Initialize counting
        StartState();

        // ====== GameScreen_UI ===== //
        CameraUpdate();
        Coins();
        ScoreUI();
        GameStateManager();

        // ====== FailScreen_UI ===== //
        FailScreenUIAlphaZero();

        // ====== ReviveScreen_UI ===== //
        ReviveScreenUIAlphaZero();

        // ====== ResultsScreen_UI ===== //
        ResultsScreenUIAlphaZero();

	}

    //========================================================================//

	void LateUpdate()
    {
        // Last camera move position
        if (app.model.playerIsMoving == true)
        {
            app.view.mainCamera.transform.position = Vector3.Lerp (app.model.positionA, app.model.positionB, Time.deltaTime * app.model.cameraSpeedFollow);
        }

//        Debug.Log(app.model.my_strBuilder.Length + " " + app.model.my_strBuilder.Capacity);
	}

    //========================================================================//

    public void ResolutionManager()
    {
        // Resolution Manager
        app.model.xWidth = Screen.width;
        app.model.yHeight = Screen.height;
        app.model.aspectRatio = app.model.yHeight / app.model.xWidth;

        if(app.model.aspectRatio >= 1.30f && app.model.aspectRatio <= 1.35f)
        {
            // 2048 * 1536
            app.model.aspectRatio = 1.33f;

            app.model.widthToLoad = 1536;
            app.model.heightToLoad = 2048;

            Screen.SetResolution(app.model.widthToLoad, app.model.heightToLoad, true);
        }
        else if(app.model.aspectRatio >= 1.45f && app.model.aspectRatio <= 1.55f)
        {
            // 960 * 640
            app.model.aspectRatio = 1.5f;

            app.model.widthToLoad = 640;
            app.model.heightToLoad = 960;

            Screen.SetResolution(app.model.widthToLoad, app.model.heightToLoad, true);
        }
        else if(app.model.aspectRatio >= 1.58f && app.model.aspectRatio <= 1.70f)
        {
            // 1280 * 768
            app.model.aspectRatio = 1.67f;
            app.model.widthToLoad = 768;
            app.model.heightToLoad = 1280;

            Screen.SetResolution(app.model.widthToLoad, app.model.heightToLoad, true);
        }
        else if(app.model.aspectRatio >= 1.75f && app.model.aspectRatio <= 1.80f)
        {
            // 1920 x 1080
            app.model.aspectRatio = 1.78f;

            app.model.widthToLoad = 1080;
            app.model.heightToLoad = 1920;

            Screen.SetResolution(app.model.widthToLoad, app.model.heightToLoad, true);
        }
        else
        {
            app.model.aspectRatio = 1.67f;

            app.model.widthToLoad = 768;
            app.model.heightToLoad = 1280;

            Screen.SetResolution(app.model.widthToLoad, app.model.heightToLoad, true);
        }
    }

    //========================================================================//

    // Scale UI method
    public void ScaleMyUI(RectTransform thisRect, float duration, float scaleSize)
    {
        float t = Mathf.PingPong(Time.deltaTime, duration) / duration;
        Vector3 finalSize = new Vector3 (scaleSize, scaleSize, scaleSize);

        thisRect.localScale = Vector3.Lerp(thisRect.localScale, finalSize, t);
    }

    // Fade UI method
    public void FadeMyUI(CanvasGroup theseCanvases, float duration, bool isFadeIn)
    {
        float t = Mathf.PingPong(Time.deltaTime, duration) / duration;

        if (isFadeIn == true)
        {
            theseCanvases.alpha = Mathf.Lerp(theseCanvases.alpha, 1.0f + Mathf.Epsilon, t);
        }
        else
        {
            theseCanvases.alpha = Mathf.Lerp(theseCanvases.alpha, 0.0f + Mathf.Epsilon, t);
        }
    }

    // Not fading UI method
    public void NormalStateUI(CanvasGroup theseCanvases, float theseCanvasAlpha, bool isInteractable)
    {
        theseCanvases.alpha = theseCanvasAlpha;
        theseCanvases.interactable = isInteractable;
    }

    // Animated Fade in UI method
    public void AnimInUI(RectTransform elementTrans, Vector2 backToElementStartPos, CanvasGroup elementAlpha)
    {
        float duration = 0.15f;
        float t = Mathf.PingPong(Time.deltaTime, duration) / duration;

        // Vector2 moveDown = new Vector2 (elementTrans.anchoredPosition.x, elementTrans.anchoredPosition.y - percentOfMovement);
        // Vector2 moveUp = new Vector2 (elementTrans.anchoredPosition.x, elementTrans.anchoredPosition.y + percentOfMovement);


        elementTrans.anchoredPosition = Vector2.Lerp (elementTrans.anchoredPosition, backToElementStartPos, t);
        elementAlpha.alpha = Mathf.Lerp (elementAlpha.alpha, 1.0f + Mathf.Epsilon, t);
    }

    // Fade Game object method
    public void FadeMyObject(SpriteRenderer theseObjects, float duration, bool isFadeIn)
    {
        float t = Mathf.PingPong(Time.deltaTime, duration) / duration;
        Color alpha = new Color (1f, 1f, 1f, 0f);
        Color solid = new Color (1f, 1f, 1f, 1f);

        if (isFadeIn)
        {
            theseObjects.color = Color.Lerp (theseObjects.color, solid, t);
        }
        else
        {
            theseObjects.color = Color.Lerp (theseObjects.color, alpha, t);
        }
    }

    // Change State method
    public void ChangeState()
    {
        if (myGameModes == GameModes.Classic)
        {
            myGameModes = GameModes.Zen;
        }
        else if (myGameModes == GameModes.Zen)
        {
            myGameModes = GameModes.Classic;
        }
    }

	// Lerp loading fill amount
    public void LerpFillValue(Image thisImage, float duration, bool animIn)
    {
        float t = Mathf.PingPong(Time.deltaTime, duration) / duration;

        if (animIn)
        {
            thisImage.fillAmount = Mathf.Lerp(thisImage.fillAmount, 1.0f + Mathf.Epsilon, t);
        }
        else
        {
            thisImage.fillAmount = Mathf.Lerp(thisImage.fillAmount, 0f + Mathf.Epsilon, t);
        }
    }

	// Lerp background blur method
    public void LerpMyBlur(bool blurEnable)
    {
        float duration = 1.0f;
        float t = Time.deltaTime / duration;

        if (blurEnable == true)
        {
            app.view.myBlur.enabled = true;
            app.view.myBlur.blurSize = Mathf.Lerp(app.view.myBlur.blurSize, 4.0f, 10 * t);
            app.view.myBlur.blurIterations = Mathf.Lerp(app.view.myBlur.blurIterations, 2f, 10 * t);
        }
        else
        {
            app.view.myBlur.blurSize = 0f;
            app.view.myBlur.blurIterations = 1;
            app.view.myBlur.enabled = false;
        }
    }

    // Random weight method
    public int RandomWeightedLevel1 ()
	{
		int result = 0;
		int total = 0;
		int randomValue = Random.Range (0, app.view.weightTotal);

		for (result = 0; result < app.view.level1CactusSet.Length; result++)
		{
			total += app.view.level1CactusSet[result].level1CactusProbability;

			if (total > randomValue ) break;
		}

		return result;
	}

	public int RandomWeightedLevel2 ()
	{
		int result = 0;
		int total = 0;
		int randomValue = Random.Range (0, app.view.weightTotal);

		for (result = 0; result < app.view.level2CactusSet.Length; result++)
		{
			total += app.view.level2CactusSet[result].level2CactusProbability;

			if (total > randomValue) break;
		}

		return result;
	}

	// Level tuner
	public void LevelChangeDifficulty ()
	{
		if (app.model.scoreCount >= 41)
		{
			app.model.levelDifficulty = 2;
		}
	}

    //========================================================================//

    public void GameStateManager()
    {
        switch (myGameStates)
        {
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameState.PlayerNotIdle

            case GameStates.PlayerNotIdle:

                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime += Time.deltaTime;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
				app.model.sceneLoadingTime = 0f;
				app.model.sceneExitLoadingTime = 0f;

				app.view.loaderOne.fillAmount = 0f;
				app.view.loaderTwo.fillAmount = 0f;
				app.view.loaderThree.fillAmount = 0f;
				app.view.loaderFour.fillAmount = 0f;

                // Fade in Game title
                for (int i = 0; i < app.view.myGameTitleClass.Length; i++)
                {
                    FadeMyObject(app.view.myGameTitleClass[i].titleSprite, 0.15f, true);
                }

                FadeMyUI(app.view.myStartScreenClass[3].start_thisCanvasGroup, 0.15f, true);

                if (app.model.sceneStartNotIdleTime >= 0.25f)
                {
                    FadeMyUI(app.view.myStartScreenClass[4].start_thisCanvasGroup, 0.15f, true);
                    FadeMyUI(app.view.myStartScreenClass[5].start_thisCanvasGroup, 0.15f, true);
                    FadeMyUI(app.view.myStartScreenClass[6].start_thisCanvasGroup, 0.15f, true);
                }

                if (app.model.sceneStartNotIdleTime >= 0.50f)
                {
                    FadeMyUI(app.view.myStartScreenClass[7].start_thisCanvasGroup, 0.15f, true);
                }

                if (app.model.sceneStartNotIdleTime >= 0.75f)
                {
                    for (int i = 0; i < app.view.myStartScreenClass.Length; i++)
                    {
                        // Canvas Group
                        if (app.view.myStartScreenClass[i].start_thisCanvasGroup == null)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisCanvasGroup == app.view.myStartScreenClass[0].start_thisCanvasGroup)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisCanvasGroup == app.view.myStartScreenClass[1].start_thisCanvasGroup)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisCanvasGroup == app.view.myStartScreenClass[2].start_thisCanvasGroup)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.interactable = true;
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.blocksRaycasts = true;
                        }

                        // Button
                        if (app.view.myStartScreenClass[i].start_thisButton == null)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisButton == app.view.myStartScreenClass[0].start_thisButton)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisButton == app.view.myStartScreenClass[1].start_thisButton)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisButton == app.view.myStartScreenClass[2].start_thisButton)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_thisButton.interactable = true;
                        }

                        // Text
                        if (app.view.myStartScreenClass[i].start_text == null)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_text == app.view.myStartScreenClass[0].start_text)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_text == app.view.myStartScreenClass[1].start_text)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_text == app.view.myStartScreenClass[2].start_text)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_text.raycastTarget = true;
                        }
                    }
                }

                switch (myGameModes)
                {
                    case GameModes.Classic:
                        float t1 = Mathf.PingPong(Time.deltaTime, 0.25f) / 0.25f;

                        FadeMyUI(app.view.myGameModesText[0].modeCG, 0.25f, true);
                        app.view.myGameModesText[0].modeRectTransform.anchoredPosition = Vector2.Lerp(app.view.myGameModesText[0].modeRectTransform.anchoredPosition, app.view.gameModeStartingPos, t1);
                        FadeMyUI(app.view.myGameModesText[1].modeCG, 0.25f, false);
                        app.view.myGameModesText[1].modeRectTransform.anchoredPosition = Vector2.Lerp(app.view.myGameModesText[1].modeRectTransform.anchoredPosition, app.view.gameModeMoveRight, t1);

                        break;

                // ++++++++++++++++++++++++++++
                    case GameModes.Zen:
                        float t2 = Mathf.PingPong(Time.deltaTime, 0.25f) / 0.25f;

                        FadeMyUI(app.view.myGameModesText[0].modeCG, 0.25f, false);
                        app.view.myGameModesText[0].modeRectTransform.anchoredPosition = Vector2.Lerp(app.view.myGameModesText[0].modeRectTransform.anchoredPosition, app.view.gameModeMoveLeft, t2);
                        FadeMyUI(app.view.myGameModesText[1].modeCG, 0.25f, true);
                        app.view.myGameModesText[1].modeRectTransform.anchoredPosition = Vector2.Lerp(app.view.myGameModesText[1].modeRectTransform.anchoredPosition, app.view.gameModeStartingPos, t2);

                        break;
                }

                break;

            #endregion
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameStates.PlayerIdle

            case GameStates.PlayerIdle:

                app.model.sceneStartIdleTime += Time.deltaTime;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
				app.model.sceneLoadingTime = 0f;

                for (int i = 0; i < app.view.myStartScreenClass.Length; i++)
                {
                    FadeMyUI(app.view.myStartScreenClass[i].start_thisCanvasGroup, 0.15f, false);
                }

                if (app.model.sceneStartIdleTime >= 0.15f)
                {
                    for (int i = 0; i < app.view.myStartScreenClass.Length; i++)
                    {
                        // Canvas Group
                        if (app.view.myStartScreenClass[i].start_thisCanvasGroup == null)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.interactable = false;
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.blocksRaycasts = false;
                        }

                        // Button
                        if (app.view.myStartScreenClass[i].start_thisButton == null)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_thisButton.interactable = false;
                        }

                        // Text
                        if (app.view.myStartScreenClass[i].start_text == null)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_text.raycastTarget = false;
                        }
                    }
                }

                if (app.model.sceneStartIdleTime >= 5.15f)
                {
                    for (int x = 0; x < app.view.myGameTitleClass.Length; x++)
                    {
                        FadeMyObject(app.view.myGameTitleClass[x].titleSprite, 0.15f, false);
                    }
                }

                break;

            #endregion
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameStates.PlayerStartedPlaying

            case GameStates.PlayerStartedPlaying:

                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime += Time.deltaTime;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
				app.model.sceneLoadingTime = 0f;

                FadeMyUI(app.view.myStartScreenClass[0].start_thisCanvasGroup, 0.15f, true);
                FadeMyUI(app.view.myStartScreenClass[1].start_thisCanvasGroup, 0.15f, true);
                FadeMyUI(app.view.myStartScreenClass[2].start_thisCanvasGroup, 0.15f, true);

                // Fade out my Tap to Play
                FadeMyUI(app.view.myStartScreenClass[3].start_thisCanvasGroup, 0.10f, false);

                FadeMyUI(app.view.myStartScreenClass[4].start_thisCanvasGroup, 0.10f, false);
                FadeMyUI(app.view.myStartScreenClass[5].start_thisCanvasGroup, 0.10f, false);
                FadeMyUI(app.view.myStartScreenClass[6].start_thisCanvasGroup, 0.10f, false);
                FadeMyUI(app.view.myStartScreenClass[7].start_thisCanvasGroup, 0.10f, false);

                if (app.model.scenePlayTime >= 0.25f)
                {
                    for (int i = 0; i < app.view.myStartScreenClass.Length; i++)
                    {
                        if (app.view.myStartScreenClass[i].start_thisCanvasGroup == null)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisCanvasGroup == app.view.myStartScreenClass[0].start_thisCanvasGroup)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisCanvasGroup == app.view.myStartScreenClass[1].start_thisCanvasGroup)
                        {
                            continue;
                        }
                        else if (app.view.myStartScreenClass[i].start_thisCanvasGroup == app.view.myStartScreenClass[2].start_thisCanvasGroup)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.alpha = 0f;
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.interactable = false;
                            app.view.myStartScreenClass[i].start_thisCanvasGroup.blocksRaycasts = false;
                        }


                        if (app.view.myStartScreenClass[i].start_thisButton == null)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_thisButton.interactable = false;
                        }

                        if (app.view.myStartScreenClass[i].start_text == null)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myStartScreenClass[i].start_text.raycastTarget = false;
                        }   
                    }

                    app.pController.enabled = true;
                    app.model.scoreIncreasing = true;
                    app.model.playerIsMoving = true;
                    app.model.myUIStateSelector = 2;
                }

                break;

            #endregion
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameStates.PlayerIsPlaying

            case GameStates.PlayerIsPlaying:

                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime += Time.deltaTime;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
				app.model.sceneLoadingTime = 0f;

                NormalStateUI(app.view.myStartScreenClass[0].start_thisCanvasGroup, 1.0f, false);
                NormalStateUI(app.view.myStartScreenClass[1].start_thisCanvasGroup, 1.0f, false);
                NormalStateUI(app.view.myStartScreenClass[2].start_thisCanvasGroup, 1.0f, false);

                app.view.myStartScreenClass[3].start_thisButton.interactable = false;
                app.view.myStartScreenClass[3].start_thisCanvasGroup.interactable = false;
                app.view.myStartScreenClass[3].start_thisCanvasGroup.blocksRaycasts = false;
                app.view.myStartScreenClass[3].start_text.raycastTarget = false;

                LevelChangeDifficulty();
                PlatformGeneration();

                // Environment Init
                GameTitleLeap();
                ForegroundOneLeap();
                ForegroundTwoLeap();
                BackgroundOneLeap();
                BackgroundTwoLeap();

                break;

            #endregion
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameStates.PlayerFailed

            case GameStates.PlayerFailed:

				// Start Failed Screen Time
                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime += Time.deltaTime;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
				app.model.sceneLoadingTime = 0f;

        		// Stop player and score
                app.model.scoreIncreasing = false;
                app.model.playerIsMoving = false;

                app.pController.playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                app.pController.enabled = false;

                // Set to Idle animation
                app.view.playerAnimator.SetFloat("AnimSpeed", 0);

                // Fade and Scale Ouch
                FadeMyUI(app.view.myFailScreenClass[0].fail_thisCanvasGroup, 0.10f, true);
                ScaleMyUI(app.view.myFailScreenClass[0].fail_thisRectTransform, 0.15f, 1.0f);

                // Fade in Tap to continue
                if (app.model.sceneFailTime >= 0.25f)
                {
                    FadeMyUI(app.view.myFailScreenClass[1].fail_thisCanvasGroup, 0.15f, true);
                }

                // Interactable On
                if (app.model.sceneFailTime >= 0.50f)
                {
                    app.view.myFailScreenClass[1].fail_thisButton.interactable = true;
                    app.view.myFailScreenClass[1].fail_thisCanvasGroup.interactable = true;
                    app.view.myFailScreenClass[1].fail_thisCanvasGroup.blocksRaycasts = true;
                    app.view.myFailScreenClass[1].fail_thisText.raycastTarget = true;

                    app.pController.mSpeed = app.pController.mSpeedStore;
                    app.pController.speedMilestoneCount = app.pController.speedMilestoneCountStore;
                    app.model.speedIncreaseMilestone = app.model.speedIncreaseMilestoneStore;
                }

                break;

            #endregion
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameStates.PlayerRevive

            case GameStates.PlayerRevive:

                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime += Time.deltaTime;
                app.model.sceneResultsTime = 0f;
				app.model.sceneLoadingTime = 0f;

                app.view.myFailScreenClass[1].fail_thisButton.interactable = false;
                app.view.myFailScreenClass[1].fail_thisCanvasGroup.interactable = false;
                app.view.myFailScreenClass[1].fail_thisCanvasGroup.blocksRaycasts = false;
                app.view.myFailScreenClass[1].fail_thisText.raycastTarget = false;

                LerpMyBlur(true);

                FadeMyUI(app.view.myFailScreenClass[0].fail_thisCanvasGroup, 0.25f, false);
                FadeMyUI(app.view.myFailScreenClass[1].fail_thisCanvasGroup, 0.25f, false);

                FadeMyUI(app.view.myStartScreenClass[0].start_thisCanvasGroup, 0.25f, false);

                if (app.model.sceneReviveTime >= 0.25f)
                {
                    AnimInUI(app.view.myReviveScreenClass[0].revive_thisRectTransform, app.view.myReviveScreenClass[0].myReviveLastPos, app.view.myReviveScreenClass[0].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 0.40f)
                {
                    AnimInUI(app.view.myReviveScreenClass[1].revive_thisRectTransform, app.view.myReviveScreenClass[1].myReviveLastPos, app.view.myReviveScreenClass[1].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 0.55f)
                {
                    AnimInUI(app.view.myReviveScreenClass[2].revive_thisRectTransform, app.view.myReviveScreenClass[2].myReviveLastPos, app.view.myReviveScreenClass[2].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 0.70f)
                {
                    AnimInUI(app.view.myReviveScreenClass[3].revive_thisRectTransform, app.view.myReviveScreenClass[3].myReviveLastPos, app.view.myReviveScreenClass[3].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 0.85f)
                {
                    AnimInUI(app.view.myReviveScreenClass[4].revive_thisRectTransform, app.view.myReviveScreenClass[4].myReviveLastPos, app.view.myReviveScreenClass[4].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 1.00f)
                {
                    AnimInUI(app.view.myReviveScreenClass[5].revive_thisRectTransform, app.view.myReviveScreenClass[5].myReviveLastPos, app.view.myReviveScreenClass[5].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 1.15f)
                {
                    AnimInUI(app.view.myReviveScreenClass[6].revive_thisRectTransform, app.view.myReviveScreenClass[6].myReviveLastPos, app.view.myReviveScreenClass[6].revive_thisCanvasGroup);
                }

                if (app.model.sceneReviveTime >= 1.30f)
                {
                    RegressBarReviveScreen(); 
                }

				if (app.view.myReviveScreenClass[6].revive_thisCanvasGroup.alpha >= 0.99f)
				{
					app.view.myReviveScreenClass[4].revive_thisButton.interactable = true;
	                app.view.myReviveScreenClass[4].revive_thisCanvasGroup.interactable = true;
	                app.view.myReviveScreenClass[4].revive_thisCanvasGroup.blocksRaycasts = true;

	                app.view.myReviveScreenClass[5].revive_thisButton.interactable = true;
	                app.view.myReviveScreenClass[5].revive_thisCanvasGroup.interactable = true;
	                app.view.myReviveScreenClass[5].revive_thisCanvasGroup.blocksRaycasts = true;

	                app.view.myReviveScreenClass[6].revive_thisButton.interactable = true;
	                app.view.myReviveScreenClass[6].revive_thisCanvasGroup.interactable = true;
	                app.view.myReviveScreenClass[6].revive_thisCanvasGroup.blocksRaycasts = true;
				}


                break;

            #endregion
		// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            #region GameStates.PlayerResults

            case GameStates.PlayerResults:

                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime += Time.deltaTime;
				app.model.sceneLoadingTime = 0f;

                for (int i = 0; i < app.view.myReviveScreenClass.Length; i++)
                {
                    FadeMyUI(app.view.myReviveScreenClass[i].revive_thisCanvasGroup, 0.25f, false);
                }

                FadeMyUI(app.view.myResultsScreenClass[0].results_thisCanvasGroup, 0.25f, true);

                // Show current mode in Results Screen
                if (myGameModes == GameModes.Classic)
                {
					app.view.myResultsScreenClass[4].results_thisText.text = app.model.GameModeClassicString();
                }
                else if (myGameModes == GameModes.Zen)
                {
                	app.view.myResultsScreenClass[4].results_thisText.text = app.model.GameModeZenString();
                }

                if (app.model.sceneResultsTime >= 0.25f)
                {
                    AnimInUI(app.view.myResultsScreenClass[4].results_thisRectTransform, app.view.myResultsScreenClass[4].myResultsLasPos, app.view.myResultsScreenClass[4].results_thisCanvasGroup);
                }
                if (app.model.sceneResultsTime >= 0.40f)
                {
                    AnimInUI(app.view.myResultsScreenClass[1].results_thisRectTransform, app.view.myResultsScreenClass[1].myResultsLasPos, app.view.myResultsScreenClass[1].results_thisCanvasGroup);
                    app.view.myResultsScreenClass[3].results_thisText.text = app.model.norScoreString();
                }
                if (app.model.sceneResultsTime >= 0.55f)
                {
                    AnimInUI(app.view.myResultsScreenClass[6].results_thisRectTransform, app.view.myResultsScreenClass[6].myResultsLasPos, app.view.myResultsScreenClass[6].results_thisCanvasGroup);
                }
                if (app.model.sceneResultsTime >= 0.70f)
                {
                    AnimInUI(app.view.myResultsScreenClass[7].results_thisRectTransform, app.view.myResultsScreenClass[7].myResultsLasPos, app.view.myResultsScreenClass[7].results_thisCanvasGroup);
                }
                if (app.model.sceneResultsTime >= 0.85f)
                {
                    AnimInUI(app.view.myResultsScreenClass[8].results_thisRectTransform, app.view.myResultsScreenClass[8].myResultsLasPos, app.view.myResultsScreenClass[8].results_thisCanvasGroup);
                }
                if (app.model.sceneResultsTime >= 1.00f)
                {
                    AnimInUI(app.view.myResultsScreenClass[9].results_thisRectTransform, app.view.myResultsScreenClass[9].myResultsLasPos, app.view.myResultsScreenClass[9].results_thisCanvasGroup);
                }
                if (app.model.sceneResultsTime >= 1.15f)
                {
                    AnimInUI(app.view.myResultsScreenClass[10].results_thisRectTransform, app.view.myResultsScreenClass[10].myResultsLasPos, app.view.myResultsScreenClass[10].results_thisCanvasGroup);
                }
                if (app.model.sceneResultsTime >= 1.30f)
                {
                    AnimInUI(app.view.myResultsScreenClass[11].results_thisRectTransform, app.view.myResultsScreenClass[11].myResultsLasPos, app.view.myResultsScreenClass[11].results_thisCanvasGroup);
                }

                if (app.view.myResultsScreenClass[11].results_thisCanvasGroup.alpha >= 0.99f)
                {
                    app.view.myResultsScreenClass[6].results_thisCanvasGroup.interactable = true;
                    app.view.myResultsScreenClass[6].results_thisCanvasGroup.blocksRaycasts = true;
                    app.view.myResultsScreenClass[7].results_thisCanvasGroup.interactable = true;
                    app.view.myResultsScreenClass[7].results_thisCanvasGroup.blocksRaycasts = true;
                    app.view.myResultsScreenClass[8].results_thisCanvasGroup.interactable = true;
                    app.view.myResultsScreenClass[8].results_thisCanvasGroup.blocksRaycasts = true;
                    app.view.myResultsScreenClass[9].results_thisCanvasGroup.interactable = true;
                    app.view.myResultsScreenClass[9].results_thisCanvasGroup.blocksRaycasts = true;
                    app.view.myResultsScreenClass[10].results_thisCanvasGroup.interactable = true;
                    app.view.myResultsScreenClass[10].results_thisCanvasGroup.blocksRaycasts = true;
                    app.view.myResultsScreenClass[11].results_thisCanvasGroup.interactable = true;
                    app.view.myResultsScreenClass[11].results_thisCanvasGroup.blocksRaycasts = true;
                }

                break;

            #endregion
        // ++++++++++++++++++++++++++++
            #region GameStates.PlayerLoading

            case GameStates.PlayerLoading:

                app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
                app.model.sceneLoadingTime += Time.deltaTime;

                if (app.model.sceneLoadingTime >= 0.15f)
                {
                    LerpFillValue(app.view.loaderOne, 0.15f, true);
                }
                if (app.model.sceneLoadingTime >= 0.30f)
                {
                    LerpFillValue(app.view.loaderTwo, 0.15f, true);
                }
                if (app.model.sceneLoadingTime >= 0.45f)
                {
                    LerpFillValue(app.view.loaderThree, 0.15f, true);
                }
                if (app.model.sceneLoadingTime >= 0.60f)
                {
                    LerpFillValue(app.view.loaderFour, 0.15f, true);
                }

				if (app.model.sceneLoadingTime >= 1.75f)
               	{
               		app.model.myUIStateSelector = 11;
               	}

            break;

            #endregion
		// ++++++++++++++++++++++++++++
			#region GameStates.PlayerLoadingExit

            case GameStates.PlayerLoadingExit:

				app.model.sceneStartIdleTime = 0f;
                app.model.sceneStartNotIdleTime = 0f;
                app.model.scenePlayTime = 0f;
                app.model.sceneFailTime = 0f;
                app.model.sceneReviveTime = 0f;
                app.model.sceneResultsTime = 0f;
                app.model.sceneLoadingTime = 0f;
                app.model.sceneExitLoadingTime += Time.deltaTime;

				if (app.model.sceneExitLoadingTime >= 0.25f)
                {
                	// Disable intertactable in Results UI
                    for (int a = 0; a < app.view.myResultsScreenClass.Length; a++)
                    {
                        if (app.view.myResultsScreenClass[a].results_thisCanvasGroup == null)
                        {
                            continue;
                        }
                        else
                        {
                            app.view.myResultsScreenClass[a].results_thisCanvasGroup.alpha = 0f;
                            app.view.myResultsScreenClass[a].results_thisCanvasGroup.blocksRaycasts = false;
                            app.view.myResultsScreenClass[a].results_thisCanvasGroup.interactable = false;
                        }
                    }

                    // Disable interactable in Revive UI
                    for (int b = 0; b < app.view.myReviveScreenClass.Length; b++)
                    {
                    	if (app.view.myReviveScreenClass[b].revive_thisCanvasGroup == null)
                    	{
                    		continue;
                    	}
                    	else
                    	{
                    		app.view.myReviveScreenClass[b].revive_thisCanvasGroup.blocksRaycasts = false;
                    		app.view.myReviveScreenClass[b].revive_thisCanvasGroup.interactable = false;
                    	}
                    }

                    // Disable interactable in Failed UI
                    for (int c = 0; c < app.view.myFailScreenClass.Length; c++)
                    {
                    	if (app.view.myFailScreenClass[c].fail_thisCanvasGroup == null)
                    	{
                    		continue;
                    	}
                    	else
                    	{
                    		app.view.myFailScreenClass[c].fail_thisCanvasGroup.blocksRaycasts = false;
                    		app.view.myFailScreenClass[c].fail_thisCanvasGroup.interactable = false;
                    	}
                    }

                    app.model.playerIdleTime = 0f;
                }
				
				if (app.model.sceneExitLoadingTime >= 0.50f)
				{
					// Bring back App title
                    for (int x = 0; x < app.view.myGameTitleClass.Length; x++)
                    {
                        app.view.myGameTitleClass[x].titleObj.transform.position = app.view.myGameTitleClass[x].startingPos;
                        app.view.myGameTitleClass[x].titleObj.SetActive (true);
                    }

                    // Enable Starting Platform
                    app.view.startingPlatform.SetActive (true);

                    // Reset Generator
                    app.view.endlessScroller.transform.position = Vector3.zero;

                    // Reset Camera
                    app.view.mainCamera.transform.position = new Vector3 (0f, 0f, -15f);

	            	// Fade out my Coin UI
	                FadeMyUI(app.view.myStartScreenClass[1].start_thisCanvasGroup, 0.15f, false);
	                FadeMyUI(app.view.myStartScreenClass[2].start_thisCanvasGroup, 0.15f, false);

	                // Remove Blur
					LerpMyBlur(false);

					#region Reset Failed UI
					// Reset Failed UI
					for (int i = 0; i < app.view.myFailScreenClass.Length; i++)
		            {
		                app.view.myFailScreenClass[i].fail_thisCanvasGroup.alpha = 0.0f;
		                app.view.myFailScreenClass[i].fail_thisCanvasGroup.interactable = false;
		                app.view.myFailScreenClass[i].fail_thisCanvasGroup.blocksRaycasts = false;
		            }
		            #endregion

					#region Reset Revive UI
					// Reset Revive UI to original position
					for (int a = 0; a < app.view.myReviveScreenClass.Length; a++)
	            	{
						app.view.myReviveScreenClass[a].revive_thisRectTransform.anchoredPosition = app.view.myReviveScreenClass[a].myReviveMovedPos;
	            	}

	            	// Reset revive bar
	            	app.view.regressBar.fillAmount = 1.0f;
					app.view.barTime = app.view.barTimeAmount;
	                #endregion

	                #region Reset Results UI
	                // Reset Results UI to orig pos
	                for (int j = 0; j < app.view.myResultsScreenClass.Length; j++)
	                {
	                	app.view.myResultsScreenClass[j].results_thisRectTransform.anchoredPosition = app.view.myResultsScreenClass[j].myResultsMovePos;
	                }

	                #endregion

	                #region Reset Environment
	            	// Return active pools to inactive
	            	for (int b = 0; b < app.view.poolToDisable.Length; b++)
	            	{
	            		for (int c = 0; c < app.view.poolToDisable[b].objectList.Count; c++)
	            		{
							if (app.view.poolToDisable[b].objectList[c].activeInHierarchy == true)
							{
								app.view.poolToDisable[b].objectList[c].SetActive(false);
							}
	            		}
	            	}

	            	// Reset Platform generator pos
					app.view.platformGeneratorPos.transform.position = new Vector3 (0f, app.view.platformGeneratorPos.transform.position.y, app.view.platformGeneratorPos.transform.position.z);

					// Reset Foreground Ones
					for (int d = 0; d < app.view.myFGOne.Length; d++)
					{
						app.view.myFGOne[d].foregroundOneLeap.transform.position = app.view.myFGOne[d].foregroundOneStartingPos;
					}

					// Reset Foreground Twos
					for (int e = 0; e < app.view.myFGTwo.Length; e++)
					{
						app.view.myFGTwo[e].foregroundTwoLeap.transform.position = app.view.myFGTwo[e].foregroundTwoStartingPos;
					}

					// Reset Background ones
					for (int f = 0; f < app.view.myBGOne.Length; f++)
					{
						app.view.myBGOne[f].backgroundOneLeap.transform.position = app.view.myBGOne[f].backgroundOneStartingPos;
					}

					// Reset background twos
					for (int g = 0; g < app.view.myBGTwo.Length; g++)
					{
						app.view.myBGTwo[g].backgroundTwoLeap.transform.position = app.view.myBGTwo[g].backgroundTwoStartingPos;
					}
					#endregion

					// Reset Score
					app.model.scoreCount = 0f;

					for (int h = 0; h < app.view.myStartScreenClass.Length; h++)
	            	{
	            		app.view.myStartScreenClass[h].start_thisCanvasGroup.alpha = 0f;
						app.view.myStartScreenClass[h].start_thisCanvasGroup.interactable = false;
	                    app.view.myStartScreenClass[h].start_thisCanvasGroup.blocksRaycasts = false;
	            	}

					if (app.model.sceneExitLoadingTime >= 0.75f)
	                {
	                    LerpFillValue(app.view.loaderFour, 0.25f, false);
	                }
	                if (app.model.sceneExitLoadingTime >= 1.0f)
	                {
	                    LerpFillValue(app.view.loaderThree, 0.25f, false);
	                }
	                if (app.model.sceneExitLoadingTime >= 1.25f)
	                {
	                    LerpFillValue(app.view.loaderTwo, 0.25f, false);
	                }
	                if (app.model.sceneExitLoadingTime >= 1.5f)
	                {
	                    LerpFillValue(app.view.loaderOne, 0.25f, false);
	                }

	                // Make sure loaders are not visible after loading
					if (app.view.loaderOne.fillAmount <= 0.01f && app.view.loaderTwo.fillAmount <= 0.01f && app.view.loaderThree.fillAmount <= 0.01f && app.view.loaderFour.fillAmount <= 0.01f)
	                {
						Debug.Log("done loading");

						app.model.myUIStateSelector = 0;
	                }
				}

            break;

            #endregion
        }
    }

    //========================================================================//
    private void InitializeUISelector ()
	{
		if (app.model.myUIStateSelector == 0)
		{
			myUIStates = UIStates.SceneStart;
		}
		else if (app.model.myUIStateSelector == 1)
		{
			myUIStates = UIStates.SceneStartPlay;
		}
		else if (app.model.myUIStateSelector == 2)
		{
			myUIStates = UIStates.ScenePlay;
		}
		else if (app.model.myUIStateSelector == 3)
		{
			myUIStates = UIStates.SceneFail;
		}
		else if (app.model.myUIStateSelector == 4)
		{
			myUIStates = UIStates.SceneRevive;
		}
		else if (app.model.myUIStateSelector == 5)
		{
			myUIStates = UIStates.SceneResults;
		}
		else if (app.model.myUIStateSelector == 10)
		{
			myUIStates = UIStates.SceneLoading;
		}
		else if (app.model.myUIStateSelector == 11)
		{
			myUIStates = UIStates.SceneLoadingExit;
		}
    }

    //========================================================================//

	private void StartState ()
	{
		// Start counting
		if (myUIStates == UIStates.SceneStart)
		{
			app.model.playerIdleTime += Time.deltaTime;

			// while idletime is greater than 10 seconds
			if (app.model.playerIdleTime > 10.0f)
			{
				// Player is idle so fade out UI
				myGameStates = GameStates.PlayerIdle;
			}
			else // less than 10 seconds
			{
				// Player is not idle maintain UI
				myGameStates = GameStates.PlayerNotIdle;
			}

			if (Input.GetMouseButtonDown (0))
			{
				// Reset Time
				app.model.playerIdleTime = 0f;
				myGameStates = GameStates.PlayerNotIdle;
			}
		}
		else if (myUIStates == UIStates.SceneStartPlay)
		{
			myGameStates = GameStates.PlayerStartedPlaying;
			// yield return new WaitForSeconds (0.25f);
		}
		else if (myUIStates == UIStates.ScenePlay)
		{
			myGameStates = GameStates.PlayerIsPlaying;
		}
		else if (myUIStates == UIStates.SceneFail)
		{
			myGameStates = GameStates.PlayerFailed;
		}
		else if (myUIStates == UIStates.SceneRevive)
		{
			myGameStates = GameStates.PlayerRevive;
		}
		else if (myUIStates == UIStates.SceneResults)
		{
			myGameStates = GameStates.PlayerResults;
		}
		else if (myUIStates == UIStates.SceneLoading)
		{
			myGameStates = GameStates.PlayerLoading;
		}
		else if (myUIStates == UIStates.SceneLoadingExit)
		{
			myGameStates = GameStates.PlayerLoadingExit;
		}
    }

	//========================================================================//

    #region StartScreen_UI

    private void StartScreenUIAlphaZero ()
	{
		if (app.view.tapToPlayStartLoop)
		{
			// Iterate each Start screen UI
			for (int i = 0; i < app.view.myStartScreenClass.Length; i++)
			{
				// Canvas Group - Interactable
				if (app.view.myStartScreenClass [i].start_thisCanvasGroup == null)
				{
					continue;
				}
				else if (app.view.myStartScreenClass [i].start_thisCanvasGroup == app.view.myStartScreenClass [0].start_thisCanvasGroup)
				{
					app.view.myStartScreenClass [0].start_thisCanvasGroup.alpha = 0f;
					app.view.myStartScreenClass [0].start_thisCanvasGroup.interactable = false;
					continue;
				}
				else if (app.view.myStartScreenClass [i].start_thisCanvasGroup == app.view.myStartScreenClass [1].start_thisCanvasGroup)
				{
					app.view.myStartScreenClass [1].start_thisCanvasGroup.alpha = 0f;
					app.view.myStartScreenClass [1].start_thisCanvasGroup.interactable = false;
					continue;
				}
				else if (app.view.myStartScreenClass [i].start_thisCanvasGroup == app.view.myStartScreenClass [2].start_thisCanvasGroup)
				{
					app.view.myStartScreenClass [2].start_thisCanvasGroup.alpha = 0f;
					app.view.myStartScreenClass [2].start_thisCanvasGroup.interactable = false;
					continue;
				}
				else
				{
					app.view.myStartScreenClass [i].start_thisCanvasGroup.alpha = 0.0f;
					app.view.myStartScreenClass [i].start_thisCanvasGroup.interactable = true;
				}

				// Button
				if (app.view.myStartScreenClass [i].start_thisButton == null)
				{
					continue;
				}
				else if (app.view.myStartScreenClass [i].start_thisButton == app.view.myStartScreenClass [0].start_thisButton)
				{
					continue;
				}
				else if (app.view.myStartScreenClass [i].start_thisButton == app.view.myStartScreenClass [1].start_thisButton)
				{
					continue;
				}
				else if (app.view.myStartScreenClass [i].start_thisButton == app.view.myStartScreenClass [2].start_thisButton)
				{
					continue;
				}
				else
				{
					app.view.myStartScreenClass [i].start_thisButton.enabled = true;
				}
			}

			// Iterate each Game modes text
			for (int x = 0; x < app.view.myGameModesText.Length; x++)
			{
				// Canvas Group - Text Mode
				if (app.view.myGameModesText [x].modeCG == null)
				{
					continue;
				}
				else
				{
					app.view.myGameModesText [x].modeCG.alpha = 0f;
					app.view.myGameModesText [x].modeCG.interactable = false;
					app.view.myGameModesText [x].modeCG.blocksRaycasts = false;
				}
			}

			// Move away from center
			app.view.myGameModesText [1].modeRectTransform.anchoredPosition = new Vector2 (app.view.myGameModesText [1].modeRectTransform.anchoredPosition.x + Screen.width / 2.0f, app.view.myGameModesText [1].modeRectTransform.anchoredPosition.y);

			// Get vector values
			app.view.gameModeStartingPos = app.view.myGameModesText [0].modeRectTransform.anchoredPosition;
			app.view.gameModeMoveLeft = new Vector2 (0 - Screen.width / 2.0f, app.view.myGameModesText [0].modeRectTransform.anchoredPosition.y);
			app.view.gameModeMoveRight = new Vector2 (0 + Screen.width / 2.0f, app.view.myGameModesText [0].modeRectTransform.anchoredPosition.y);   

			// Change Strings of Modes
			app.view.myGameModesText [0].modeText.text = app.model.GameModeClassicString ();
			app.view.myGameModesText [1].modeText.text = app.model.GameModeZenString ();

			// Get title starting values
			for (int z = 0; z < app.view.myGameTitleClass.Length; z++)
			{
				app.view.myGameTitleClass [z].startingPos = new Vector3 (app.view.myGameTitleClass [z].titleObj.transform.position.x, app.view.myGameTitleClass [z].titleObj.transform.position.y, app.view.myGameTitleClass [z].titleObj.transform.position.z);
			}

			// Get Foreground One starting values
			for (int y = 0; y < app.view.myFGOne.Length; y++)
			{
				app.view.myFGOne [y].foregroundOneStartingPos = app.view.myFGOne [y].foregroundOneLeap.transform.position;
			}

			// Get Foreground Two starting values
			for (int w = 0; w < app.view.myFGTwo.Length; w++)
			{
				app.view.myFGTwo [w].foregroundTwoStartingPos = app.view.myFGTwo [w].foregroundTwoLeap.transform.position;
			}

			// Get Background One starting values
			for (int u = 0; u < app.view.myBGOne.Length; u++)
			{
				app.view.myBGOne [u].backgroundOneStartingPos = app.view.myBGOne [u].backgroundOneLeap.transform.position;
			}

			// Get Background Two starting values
			for (int v = 0; v < app.view.myBGTwo.Length; v++)
			{
				app.view.myBGTwo [v].backgroundTwoStartingPos = app.view.myBGTwo [v].backgroundTwoLeap.transform.position;
			}

			// Set weight spawn of Platforms
			app.view.weightTotal = 0;

            app.view.tapToPlayStartLoop = false;
        }
    }

    #endregion

    //========================================================================//

    #region FailSCreen_Methods

    private void FailScreenUIAlphaZero()
    {
        if (app.view.failScreenStartLoop)
        {
            for (int i = 0; i < app.view.myFailScreenClass.Length; i++)
            {
                app.view.myFailScreenClass[i].fail_thisCanvasGroup.alpha = 0.0f;
                app.view.myFailScreenClass[i].fail_thisCanvasGroup.interactable = false;
                app.view.myFailScreenClass[i].fail_thisCanvasGroup.blocksRaycasts = false;
            }

			app.view.myFailScreenClass [0].fail_thisRectTransform.localScale = new Vector3 (0.85f, 0.85f, 0.85f);

            app.view.failScreenStartLoop = false;
        }
    }

    #endregion

    //========================================================================//

    #region ReviveScreen_Methods

    private void ReviveScreenUIAlphaZero ()
	{
		if (app.view.reviveScreenStartLoop) 
		{
			for (int i = 0; i < app.view.myReviveScreenClass.Length; i++) 
			{
				app.view.myReviveScreenClass [i].revive_thisCanvasGroup.alpha = 0.0f;
				app.view.myReviveScreenClass [i].revive_thisCanvasGroup.interactable = false;
				app.view.myReviveScreenClass [i].revive_thisCanvasGroup.blocksRaycasts = false;
				app.view.myReviveScreenClass [i].myReviveLastPos = app.view.myReviveScreenClass [i].revive_thisRectTransform.anchoredPosition;
			}

			app.view.myReviveScreenClass [0].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [0].revive_thisRectTransform.anchoredPosition.x, app.view.myReviveScreenClass [0].revive_thisRectTransform.anchoredPosition.y - app.view.reviveUIPercentToMove_Y);
			app.view.myReviveScreenClass [1].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [1].revive_thisRectTransform.anchoredPosition.x, app.view.myReviveScreenClass [1].revive_thisRectTransform.anchoredPosition.y - app.view.reviveUIPercentToMove_Y);
			app.view.myReviveScreenClass [2].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [2].revive_thisRectTransform.anchoredPosition.x, app.view.myReviveScreenClass [2].revive_thisRectTransform.anchoredPosition.y - app.view.reviveUIPercentToMove_Y);
			app.view.myReviveScreenClass [3].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [3].revive_thisRectTransform.anchoredPosition.x, app.view.myReviveScreenClass [3].revive_thisRectTransform.anchoredPosition.y - app.view.reviveUIPercentToMove_Y);

			app.view.myReviveScreenClass [4].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [4].revive_thisRectTransform.anchoredPosition.x + app.view.reviveUIPercentToMove_X, app.view.myReviveScreenClass [4].revive_thisRectTransform.anchoredPosition.y);
			app.view.myReviveScreenClass [5].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [5].revive_thisRectTransform.anchoredPosition.x - app.view.reviveUIPercentToMove_X, app.view.myReviveScreenClass [5].revive_thisRectTransform.anchoredPosition.y);
			app.view.myReviveScreenClass [6].revive_thisRectTransform.anchoredPosition = new Vector2 (app.view.myReviveScreenClass [6].revive_thisRectTransform.anchoredPosition.x + app.view.reviveUIPercentToMove_X, app.view.myReviveScreenClass [6].revive_thisRectTransform.anchoredPosition.y);

			// Get 2nd position
			for (int x = 0; x < app.view.myReviveScreenClass.Length; x++) 
			{
				app.view.myReviveScreenClass[x].myReviveMovedPos = app.view.myReviveScreenClass[x].revive_thisRectTransform.anchoredPosition;
			}

            app.view.myReviveScreenClass[4].revive_thisButton.interactable = false;
            app.view.myReviveScreenClass[5].revive_thisButton.interactable = false;
            app.view.myReviveScreenClass[6].revive_thisButton.interactable = false;

            app.view.barTime = app.view.barTimeAmount;

            app.view.reviveScreenStartLoop = false;
        }
    }

    private void RegressBarReviveScreen()
    {
        if (app.view.barTime > 0)
        {
            app.view.barTime -= Time.deltaTime;
            app.view.regressBar.fillAmount = app.view.barTime / app.view.barTimeAmount;
        }
    }

    #endregion

    //========================================================================//

    #region ResultsScreen_Method

    private void ResultsScreenUIAlphaZero ()
	{
		if (app.view.resultScreenStartLoop)
		{
			for (int i = 0; i < app.view.myResultsScreenClass.Length; i++)
			{
				// Canvas Group
				if (app.view.myResultsScreenClass [i].results_thisCanvasGroup == null)
				{
					continue;
				}
				else
				{
					app.view.myResultsScreenClass [i].results_thisCanvasGroup.alpha = 0f;
					app.view.myResultsScreenClass [i].results_thisCanvasGroup.interactable = false;
					app.view.myResultsScreenClass [i].results_thisCanvasGroup.blocksRaycasts = false;
				}

				// Rect Transform
				app.view.myResultsScreenClass [i].myResultsLasPos = app.view.myResultsScreenClass [i].results_thisRectTransform.anchoredPosition;
			}

			app.view.myResultsScreenClass [1].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [1].results_thisRectTransform.anchoredPosition.x + app.view.resultsUIPercentToMove_X, app.view.myResultsScreenClass [1].results_thisRectTransform.anchoredPosition.y);
			app.view.myResultsScreenClass [4].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [4].results_thisRectTransform.anchoredPosition.x - app.view.resultsUIPercentToMove_X, app.view.myResultsScreenClass [4].results_thisRectTransform.anchoredPosition.y);
			app.view.myResultsScreenClass [6].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [6].results_thisRectTransform.anchoredPosition.x - app.view.resultsUIPercentToMove_X, app.view.myResultsScreenClass [6].results_thisRectTransform.anchoredPosition.y);
			app.view.myResultsScreenClass [7].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [7].results_thisRectTransform.anchoredPosition.x + app.view.resultsUIPercentToMove_X, app.view.myResultsScreenClass [7].results_thisRectTransform.anchoredPosition.y);
			app.view.myResultsScreenClass [8].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [8].results_thisRectTransform.anchoredPosition.x - app.view.resultsUIPercentToMove_X, app.view.myResultsScreenClass [8].results_thisRectTransform.anchoredPosition.y);

			app.view.myResultsScreenClass [9].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [9].results_thisRectTransform.anchoredPosition.x, app.view.myResultsScreenClass [9].results_thisRectTransform.anchoredPosition.y - app.view.resultsUIPercentToMove_Y);
			app.view.myResultsScreenClass [10].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [10].results_thisRectTransform.anchoredPosition.x, app.view.myResultsScreenClass [10].results_thisRectTransform.anchoredPosition.y - app.view.resultsUIPercentToMove_Y);
			app.view.myResultsScreenClass [11].results_thisRectTransform.anchoredPosition = new Vector2 (app.view.myResultsScreenClass [11].results_thisRectTransform.anchoredPosition.x, app.view.myResultsScreenClass [11].results_thisRectTransform.anchoredPosition.y - app.view.resultsUIPercentToMove_Y);

			for (int a = 0; a < app.view.myResultsScreenClass.Length; a++)
			{
				app.view.myResultsScreenClass[a].myResultsMovePos = app.view.myResultsScreenClass[a].results_thisRectTransform.anchoredPosition;
			}

            app.view.resultScreenStartLoop = false;
        }
    }

    #endregion

    //========================================================================//

    #region EnvironmentPooling

	private void ForegroundOneLeap ()
	{
		for (int i = 0; i < app.view.myFGOne.Length; i++)
		{
			if (app.model.playerIsMoving == true)
			{
				app.view.myFGOne[i].foregroundOneLeap.transform.Translate (Vector3.left * app.pController.mSpeed * app.model.foregroundOneLeapSpeed * Time.deltaTime);

				if (app.view.myFGOne[i].foregroundOneLeap.transform.position.x <= app.view.groundPoolerRange.transform.position.x)
				{
					app.view.myFGOne[i].foregroundOneLeap.transform.position = new Vector3 (app.view.platformGenRange.transform.position.x + Random.Range(-2.0f, 2.0f), app.view.myFGOne[i].foregroundOneLeap.transform.position.y, app.view.myFGOne[i].foregroundOneLeap.transform.position.z);
				}
			}
		}

	}

    private void ForegroundTwoLeap ()
	{
		for (int i = 0; i < app.view.myFGTwo.Length; i++)
		{
			if (app.model.playerIsMoving == true)
			{
				app.view.myFGTwo[i].foregroundTwoLeap.transform.Translate (Vector3.left * app.pController.mSpeed * app.model.foregroundTwoLeapSpeed * Time.deltaTime);

				if (app.view.myFGTwo[i].foregroundTwoLeap.transform.position.x <= app.view.groundPoolerRange.transform.position.x)
				{
					app.view.myFGTwo[i].foregroundTwoLeap.transform.position = new Vector3 (app.view.platformGenRange.transform.position.x + Random.Range(-3.0f, 3.0f), app.view.myFGTwo[i].foregroundTwoLeap.transform.position.y, app.view.myFGTwo[i].foregroundTwoLeap.transform.position.z);
				}
			}
		}
	}

    private void GameTitleLeap ()
    {
        for (int i = 0; i < app.view.myGameTitleClass.Length; i++)
        {
            if (app.model.playerIsMoving == true)
            {
                app.view.myGameTitleClass[i].titleObj.transform.Translate (Vector3.left * app.pController.mSpeed * app.model.titleLeapSpeed * Time.deltaTime);
            }
        }
    }

	private void BackgroundOneLeap ()
	{
		for (int i = 0; i < app.view.myBGOne.Length; i++)
		{
			if (app.model.playerIsMoving == true)
			{
				app.view.myBGOne[i].backgroundOneLeap.transform.Translate (Vector3.left * app.pController.mSpeed * app.model.backgroundOneLeapSpeed * Time.deltaTime);

				if (app.view.myBGOne[i].backgroundOneLeap.transform.position.x <= app.view.groundPoolerRange.transform.position.x)
				{
					app.view.myBGOne[i].backgroundOneLeap.transform.position = new Vector3 (app.view.platformGenRange.transform.position.x + Random.Range(-3.0f, 3.0f), app.view.myBGOne[i].backgroundOneLeap.transform.position.y, app.view.myBGOne[i].backgroundOneLeap.transform.position.z);
				}
			}
		}
	}

    private void BackgroundTwoLeap()
    {
		for (int i = 0; i < app.view.myBGTwo.Length; i++)
        {
            if (app.model.playerIsMoving == true)
            {
                app.view.myBGTwo[i].backgroundTwoLeap.transform.Translate(Vector3.left * app.pController.mSpeed * app.model.backgroundTwoLeapSpeed * Time.deltaTime);

				if (app.view.myBGTwo[i].backgroundTwoLeap.transform.position.x <= app.view.mountainPoolerRange.transform.position.x)
                {
					app.view.myBGTwo[i].backgroundTwoLeap.transform.position = new Vector3 (app.view.mountainGeneratorPos.transform.position.x, app.view.myBGTwo[i].backgroundTwoLeap.transform.position.y, app.view.myBGTwo[i].backgroundTwoLeap.transform.position.z);
                }
            }
        }
    }

    #endregion

    //========================================================================//


	public void CameraUpdate ()
	{
		app.model.distanceToMove = app.view.player.transform.position.x - app.model.lastPlayerPos.x;
		app.model.positionA = new Vector3 (app.view.mainCamera.transform.position.x + app.model.distanceToMove, app.view.mainCamera.transform.position.y, app.view.mainCamera.transform.position.z);
		// app.model.positionB = new Vector3 (app.view.mainCamera.transform.position.x, Mathf.Clamp (app.model.lastPlayerPos.y + app.model.cameraIndentY, app.model.minCameraClampY, app.model.maxCameraClampY), -10);
		app.model.positionB = new Vector3 (app.model.lastPlayerPos.x + app.model.cameraIndentX, Mathf.Clamp (app.model.lastPlayerPos.y + app.model.cameraIndentY, app.model.minCameraClampY, app.model.maxCameraClampY), app.view.mainCamera.transform.position.z);
		app.model.lastPlayerPos = app.view.player.transform.position;
	}

	public void Coins ()
	{
		app.view.coinText.text = (app.model.coinsCount.ToString ());

		if (app.model.coinsCount >= app.model.maxCoins)
		{
			app.model.coinsCount = app.model.maxCoins;
		}
        
		// app.view.hiScoreText.text = "HIGH SCORE: " + Mathf.Round (app.model.highScoreCount);
	}

	public void AddCoins (int coinsToAdd)
	{
		app.model.coinsCount += coinsToAdd;
	}

	public void ScoreUI ()
	{
		if (app.model.scoreIncreasing)
		{
			app.model.scoreCount += app.model.pointsPerSecond * Time.deltaTime;
		}

		if (app.model.scoreCount > app.model.highScoreCount)
		{
			app.model.highScoreCount = app.model.scoreCount;
		}

		PlayerPrefs.SetFloat("HighScore", app.model.highScoreCount);
		PlayerPrefs.SetInt("CoinScore", app.model.coinsCount);

		 app.view.scoreText.text = app.model.norScoreString();
         app.view.hiScoreText.text = app.model.hiScoreString();

	}

	public void PlatformGeneration ()
	{
		if (app.view.platformGeneratorPos.transform.position.x < app.view.platformGenRange.position.x) // If generator pos nears generator range
		{
//            app.model.thePlatformPoolsSelector = Random.Range(0, app.view.myEmptyPlatform.Length); // Platform Pool Selector
			app.model.heightChange = app.view.platformGeneratorPos.transform.position.y + Random.Range (app.model.maxHeightChange, -app.model.maxHeightChange); // Change height randomly between value of maxHeightChange and -maxHeightChange

			// app.view.newCactiOne = app.view.theCactiPoolsRight[app.model.cactiSelector].GetPooledObject();
			app.view.platformGeneratorPos.transform.position = new Vector3 (app.view.platformGeneratorPos.transform.position.x + app.model.platformWidth + app.model.distanceBetweenPlatforms, app.model.heightChange, 0);

			switch (app.model.levelDifficulty)
			{
				case 0: // Level 0
                    // app.view.newCactusRight = app.view.myRightCactusList[0].cactus.GetPooledObject();
                    // app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
					app.view.newPlatform = app.view.level1CactusSet [0].level1Cactus.GetPooledObject ();

					break;

				case 1: // Level 1

					app.view.weightTotal = 
					app.view.level1CactusSet[0].level1CactusProbability + 
					app.view.level1CactusSet[1].level1CactusProbability +
					app.view.level1CactusSet[2].level1CactusProbability +
					app.view.level1CactusSet[3].level1CactusProbability +
					app.view.level1CactusSet[4].level1CactusProbability +
					app.view.level1CactusSet[5].level1CactusProbability;

					app.view.finalSpawn = RandomWeightedLevel1 ();

					Debug.Log (app.view.finalSpawn);

					if (app.view.finalSpawn == 0)
					{
						app.view.newPlatform = app.view.level1CactusSet [0].level1Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 1)
					{
						app.view.newPlatform = app.view.level1CactusSet [1].level1Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 2)
					{
						app.view.newPlatform = app.view.level1CactusSet [2].level1Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 3)
					{
						app.view.newPlatform = app.view.level1CactusSet [3].level1Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 4)
					{
						app.view.newPlatform = app.view.level1CactusSet [4].level1Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 5)
					{
						app.view.newPlatform = app.view.level1CactusSet [5].level1Cactus.GetPooledObject ();
					}

					break;

				case 2: // Level 2

					app.view.weightTotal = 
					app.view.level2CactusSet[0].level2CactusProbability + 
					app.view.level2CactusSet[1].level2CactusProbability +
					app.view.level2CactusSet[2].level2CactusProbability + 
					app.view.level2CactusSet[3].level2CactusProbability +
					app.view.level2CactusSet[4].level2CactusProbability + 
					app.view.level2CactusSet[5].level2CactusProbability +
					app.view.level2CactusSet[6].level2CactusProbability + 
					app.view.level2CactusSet[7].level2CactusProbability +
					app.view.level2CactusSet[8].level2CactusProbability + 
					app.view.level2CactusSet[9].level2CactusProbability +
					app.view.level2CactusSet[10].level2CactusProbability + 
					app.view.level2CactusSet[11].level2CactusProbability;

					app.view.finalSpawn = RandomWeightedLevel2 ();

					if (app.view.finalSpawn == 0)
					{
						app.view.newPlatform = app.view.level2CactusSet [0].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 1)
					{
						app.view.newPlatform = app.view.level2CactusSet [1].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 2)
					{
						app.view.newPlatform = app.view.level2CactusSet [2].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 3)
					{
						app.view.newPlatform = app.view.level2CactusSet [3].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 4)
					{
						app.view.newPlatform = app.view.level2CactusSet [4].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 5)
					{
						app.view.newPlatform = app.view.level2CactusSet [5].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 6)
					{
						app.view.newPlatform = app.view.level2CactusSet [6].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 7)
					{
						app.view.newPlatform = app.view.level2CactusSet [7].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 8)
					{
						app.view.newPlatform = app.view.level2CactusSet [8].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 9)
					{
						app.view.newPlatform = app.view.level2CactusSet [9].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 10)
					{
						app.view.newPlatform = app.view.level2CactusSet [10].level2Cactus.GetPooledObject ();
					}
					else if (app.view.finalSpawn == 11)
					{
						app.view.newPlatform = app.view.level2CactusSet [11].level2Cactus.GetPooledObject ();
					}
                break;

            }

            /*
            if (Random.Range(0f, 1f) <= app.view.myRightCactusList[0].randomSelectThreshold)
            {
                app.view.newCactusRight = app.view.myRightCactusList[1].cactus.GetPooledObject();
                app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
            }
            else if (Random.Range(0f, 1f) <= app.view.myRightCactusList[1].randomSelectThreshold)
            {
                app.view.newCactusRight = app.view.myRightCactusList[1].cactus.GetPooledObject();
                app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
            }
            else if (Random.Range(0f, 1f) <= app.view.myRightCactusList[2].randomSelectThreshold)
            {
                app.view.newCactusRight = app.view.myRightCactusList[2].cactus.GetPooledObject();
                app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
            }
            else if (Random.Range(0f, 1f) <= app.view.myRightCactusList[3].randomSelectThreshold)
            {
                app.view.newCactusRight = app.view.myRightCactusList[3].cactus.GetPooledObject();
                app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
            }
            else if (Random.Range(0f, 1f) <= app.view.myRightCactusList[4].randomSelectThreshold)
            {
                
                app.view.newCactusRight = app.view.myRightCactusList[4].cactus.GetPooledObject();
                app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
            }
            else // Normal cactus
            {
                app.view.newCactusRight = app.view.myRightCactusList[0].cactus.GetPooledObject();
                app.view.cactusRightSpriteRenderer = app.view.newCactusRight.GetComponent<SpriteRenderer>();
            }
            */

            /// Clamp height change value to maxHeight and minHeight
            if (app.model.heightChange > app.model.maxHeightRange)
            {
                app.model.heightChange = app.model.maxHeightRange;
            }
            else if (app.model.heightChange < app.model.minHeightRange)
            {
                app.model.heightChange = app.model.minHeightRange;
            }

            // If generator pos nears generator range, set new generator pos


            #region Old Pool
                // Platform One

//                GameObject newPlatform = app.view.myEmptyPlatform[0].GetPooledObject();
//                newPlatform.transform.position = app.view.platformGeneratorPos.transform.position;
//                newPlatform.transform.rotation = app.view.platformGeneratorPos.transform.rotation;
//				newPlatform.SetActive(true);


                app.view.newPlatform.transform.position = new Vector3(app.view.platformGeneratorPos.transform.position.x, app.view.platformGeneratorPos.transform.position.y, app.view.platformGeneratorPos.transform.position.z);
                app.view.newPlatform.transform.rotation = Quaternion.Euler (app.view.newPlatform.transform.rotation.x, app.view.newPlatform.transform.rotation.y, app.view.newPlatform.transform.rotation.z);
                app.view.newPlatform.SetActive (true);

            #endregion

            // Set cactus spawn threshold
            /*
            if (Random.Range (0f, 100f) < app.model.randomCactiThreshold)
            {
                GameObject newCactiOne = app.view.theCactiPoolsOne [app.model.cactiSelector].GetPooledObject ();
                SpriteRenderer renCact1 = newCactiOne.gameObject.GetComponent<SpriteRenderer> ();
                float randomCactiX = Random.Range (-app.model.platformWidth / 2f + 0.5f, app.model.platformWidth / 2f - 0.5f);
                newCactiOne.transform.position = new Vector3 (app.view.platformGeneratorPos.transform.position.x + randomCactiX, app.view.platformGeneratorPos.transform.position.y + 1.0f, app.view.platformGeneratorPos.transform.position.z);
                newCactiOne.transform.rotation = app.view.platformGeneratorPos.transform.rotation;
                newCactiOne.SetActive (true);
            }
            */
            /*
            if (app.model.scoreCount <= app.model.startLevelSet)
            {
                app.model.startLevelSetBool = true;
                app.model.easyLevelSetBool = false;

                if (app.model.startLevelSetBool == true)
                {
                    Debug.Log("StartLevel");
                    GameObject newPlatform = app.view.thePlatformPools[0].GetPooledObject();
                    newPlatform.transform.position = app.view.platformGeneratorPos.transform.position;
                    newPlatform.transform.rotation = app.view.platformGeneratorPos.transform.rotation;
                    newPlatform.SetActive(true);
                }
            }

            else if (app.model.scoreCount <= app.model.easyLevelSet)
            {
                app.model.startLevelSetBool = false;
                app.model.easyLevelSetBool = true;

                if (app.model.easyLevelSetBool == true)
                {
                    Debug.Log("EasyLevel");
                    GameObject easyLevelPlatform = app.view.thePlatformPools[1].GetPooledObject();
                    easyLevelPlatform.transform.position = app.view.platformGeneratorPos.transform.position;
                    easyLevelPlatform.transform.rotation = app.view.platformGeneratorPos.transform.rotation;
                    easyLevelPlatform.SetActive(true);
                }

            }

            else
            {
                Debug.Log("Continue..");
                GameObject easyLevelPlatform = app.view.thePlatformPools[1].GetPooledObject();
                easyLevelPlatform.transform.position = app.view.platformGeneratorPos.transform.position;
                easyLevelPlatform.transform.rotation = app.view.platformGeneratorPos.transform.rotation;
                easyLevelPlatform.SetActive(true);

                // Add coins in list
                /*
                coinsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("coinsOff"));

                for (int i = 0; i < coinsList.Count; i++)
                {
                    Debug.Log("Count: " + coinsList.Count);
                    coinsList[i].gameObject.GetComponent<Renderer>().enabled = true;
                    coinsList[i].gameObject.tag = "coins";
                }
             
            }
            */

            // BigCoinSpawn();

            /*
			#region COIN SPAWN FORMATION 0
         
                if (Random.Range(0f, 1f) <= app.model.randomCoinThreshold)
    			{
                    Vector3 startPosition;


                    if (app.model.coinLeftSpawnChance < Random.Range(0f, 1f))
                    {
                        startPosition = new Vector3 (app.view.platformGeneratorPos.transform.position.x - 3.0f, app.view.platformGeneratorPos.transform.position.y + 1.0f, app.view.platformGeneratorPos.transform.position.z);
                    }

                    else if (app.model.coinLeftSpawnChance > Random.Range(0f, 1f))
                    {
                        startPosition = new Vector3 (app.view.platformGeneratorPos.transform.position.x + 3.0f, app.view.platformGeneratorPos.transform.position.y + 1.0f, app.view.platformGeneratorPos.transform.position.z);
                    }

                    else
                    {
                        startPosition = new Vector3 (app.view.platformGeneratorPos.transform.position.x, app.view.platformGeneratorPos.transform.position.y + 1.0f, app.view.platformGeneratorPos.transform.position.z);
                    }


                    // startPosition = new Vector3 (app.view.newCactusRight.transform.position.x, app.view.newCactusRight.transform.position.y + 2.0f, app.view.newCactusRight.transform.position.z);
                    
                    #region Coin1
    				GameObject coin1 = app.view.theCoinPools [app.model.coinPoolsSelector].GetPooledObject ();
                    Renderer renCoin1 = coin1.gameObject.GetComponent<Renderer>();
    				coin1.transform.position = startPosition;
                    coin1.transform.Rotate (0, 50 * Time.deltaTime ,0);

                    if (renCoin1.bounds.Intersects (app.view.cactusRightSpriteRenderer.bounds) || renCoin1.bounds.Intersects (app.view.renCact2.bounds))
                    {
                        coin1.transform.position = new Vector3 (startPosition.x, startPosition.y, startPosition.z);
                        coin1.SetActive (false);
                    }

                    else 
                    {
                        coin1.SetActive (true);
                    }
                    #endregion

                    #region Coin2
    				// 1st Left Coin
    				GameObject coin2 = app.view.theCoinPools [app.model.coinPoolsSelector].GetPooledObject ();
    				Renderer renCoin2 = coin2.gameObject.GetComponentInChildren<Renderer> ();
    				coin2.transform.position = startPosition;
    				if (renCoin2.bounds.Intersects (renCoin1.bounds))
    				{
                        coin2.transform.position = new Vector3 (startPosition.x - app.model.distanceBetweenCoins, startPosition.y, startPosition.z);

                        if (renCoin2.bounds.Intersects (app.view.cactusRightSpriteRenderer.bounds) || renCoin2.bounds.Intersects (app.view.renCact2.bounds))
    					{
                            coin2.transform.position = new Vector3 (coin2.transform.position.x, coin2.transform.position.y + 1.0f, coin2.transform.position.z);
    					}
    				}
                    else if (renCoin2.bounds.Intersects (app.view.cactusRightSpriteRenderer.bounds) || renCoin2.bounds.Intersects (app.view.renCact2.bounds))
    				{
                        coin2.transform.position = new Vector3 (startPosition.x - app.model.distanceBetweenCoins - 0.5f, startPosition.y, startPosition.z);
    				}
    				coin2.SetActive (true);
                    #endregion

                    #region Coin3
    				// 1st Right Coin
    				GameObject coin3 = app.view.theCoinPools [app.model.coinPoolsSelector].GetPooledObject ();
                    Renderer renCoin3 = coin3.gameObject.GetComponentInChildren<Renderer> ();
    				coin3.transform.position = startPosition;
    				if (renCoin3.bounds.Intersects (renCoin1.bounds))
    				{
                        coin3.transform.position = new Vector3 (startPosition.x + app.model.distanceBetweenCoins, startPosition.y, startPosition.z);

                        if (renCoin3.bounds.Intersects (app.view.cactusRightSpriteRenderer.bounds) || renCoin3.bounds.Intersects (app.view.renCact2.bounds))
    					{
                            coin3.transform.position = new Vector3 (coin3.transform.position.x, coin3.transform.position.y + 1.0f, coin3.transform.position.z);
    					}
    				}
                    else if (renCoin3.bounds.Intersects (app.view.cactusRightSpriteRenderer.bounds) || renCoin3.bounds.Intersects (app.view.renCact2.bounds))
    				{
                        coin3.transform.position = new Vector3 (startPosition.x + app.model.distanceBetweenCoins + 0.5f, startPosition.y, startPosition.z);
    				}
    				coin3.SetActive (true);
                    #endregion
                }

            #endregion 
            */
		}
	}


	void MyDebugList ()
	{
		Debug.Log ("Speed Increase Milestone " + app.model.speedIncreaseMilestone);
		Debug.Log ("Speed Multiplier " + app.model.speedMultiplier);
		Debug.Log ("Movement Speed " + app.pController.mSpeed);
		Debug.Log ("Camera Width: " + app.model.camWidth);
		Debug.Log ("Camera Height: " + app.model.camHeight);
	}
}
