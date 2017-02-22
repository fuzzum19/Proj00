using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : RexJumpElement
{
    public Texture2D fadeOutTexture;    // The texture that will overlay the screen
    public float fadeSpeed = 0f;        // The fading speed

    private int drawDepth = -1000;      // The texture's order on the draw hierarchy
    private float alpha = 1.0f;         // The texture's alpha value
    private int fadeDir = -1;           // The direction to fade

    void OnGUI ()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);               // Set alpha value
        GUI.depth = drawDepth;                                                              // Make the texture render on top
        GUI.DrawTexture ( new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture );   // Full screen texture
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene myScene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log("Scene Name: " + myScene.name);
        Debug.Log("Mode: " + mode);
    }
}

/*
public class LoadingScreenManager : RexJumpElement
{
    [Header("Loading Visuals")]
    public Image loadingIcon;
    public Image loadingDoneIcon;
    public Text loadingText;
    public Image progressBar;
    public Image fadeOverlay;

    [Header("Timing Settings")]
    public float waitOnLoadEnd = 0.25f;
    public float fadeDuration = 0.25f;

    [Header("Loading Settings")]
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    public ThreadPriority loadThreadPriority;

    [Header("Other")]
    // If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
    public AudioListener audioListener;

    AsyncOperation operation;
    Scene currentScene;

    public static int sceneToLoad = -1;
    // IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
    static int loadingSceneIndex = 1;

    public static void LoadScene(int levelNum) 
    {                
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = levelNum;
        SceneManager.LoadScene(loadingSceneIndex);
    }

    void Start() 
    {
        if (sceneToLoad < 0)
            return;

        fadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadAsync(sceneToLoad));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        ShowLoadingVisuals();

        yield return null; 

        FadeIn();
        StartOperation(levelNum);

        float lastProgress = 0f;

        // operation does not auto-activate scene, so it's stuck at 0.9
        while (DoneLoading() == false)
        {
            yield return null;

            if (Mathf.Approximately(operation.progress, lastProgress) == false)
            {
                progressBar.fillAmount = operation.progress;
                lastProgress = operation.progress;
            }
        }

        if (loadSceneMode == LoadSceneMode.Additive)
        {
            audioListener.enabled = false;
        }
            
        ShowCompletionVisuals();

        yield return new WaitForSeconds(waitOnLoadEnd);

        FadeOut();

        yield return new WaitForSeconds(fadeDuration);

        if (loadSceneMode == LoadSceneMode.Additive)
        {
//            SceneManager.UnloadScene(currentScene.name);
            SceneManager.UnloadSceneAsync(currentScene.name);
        }
        else
        {
            operation.allowSceneActivation = true;
        }
            
    }

    private void StartOperation(int levelNum) 
    {
        Application.backgroundLoadingPriority = loadThreadPriority;
        operation = SceneManager.LoadSceneAsync(levelNum, loadSceneMode);


        if (loadSceneMode == LoadSceneMode.Single)
            operation.allowSceneActivation = false;
    }

    private bool DoneLoading() 
    {
        return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f); 
    }

    void FadeIn() 
    {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
    }

    void FadeOut() 
    {
        fadeOverlay.CrossFadeAlpha(1, fadeDuration, true);
    }

    void ShowLoadingVisuals() 
    {
        loadingIcon.gameObject.SetActive(true);
        loadingDoneIcon.gameObject.SetActive(false);

        progressBar.fillAmount = 0f;
        loadingText.text = "LOADING...";
    }

    void ShowCompletionVisuals() {
        loadingIcon.gameObject.SetActive(false);
        loadingDoneIcon.gameObject.SetActive(true);

        progressBar.fillAmount = 1f;
        loadingText.text = "LOADING DONE";
    }
}
*/