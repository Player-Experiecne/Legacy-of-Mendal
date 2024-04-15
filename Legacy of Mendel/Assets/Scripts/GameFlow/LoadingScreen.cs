using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject loadingIndicator;
    public CutsceneManager cutsceneManager;
    private float minLoadingTime = 2f; // Minimum loading time in seconds
    private float smoothLoadingSpeed = 0.5f; // Speed of the loading bar's smooth progress
    private Vector3 startPosition = new Vector3(-700, -400, 0);
    private Vector3 endPosition = new Vector3(700, -400, 0);

    private bool cutsceneCompleted = false;

    public void LoadScene(string sceneName)
    {
        loadingScreen.SetActive(true); // Immediately activate loading screen
        StartCoroutine(DelayedLoad(sceneName));
    }

    // Delays the start of the scene loading to ensure the loading screen appears instantly
    IEnumerator DelayedLoad(string sceneName)
    {
        // Reset the loading bar's displayed progress and visual value at the start
        loadingIndicator.transform.localPosition = startPosition;
        float displayedProgress = 0f;

        // Optionally wait a very short time to ensure the loading screen is displayed
        yield return null; // Wait for one frame

        // Start the actual loading process
        StartCoroutine(LoadAsynchronously(sceneName, displayedProgress));
    }

    IEnumerator LoadAsynchronously(string sceneName, float displayedProgress)
    {
        float startTime = Time.time;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Calculate the actual load progress
            float totalProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Smoothly update the displayed progress
            while (displayedProgress < totalProgress)
            {
                displayedProgress += smoothLoadingSpeed * Time.deltaTime;
                loadingIndicator.transform.localPosition = startPosition + new Vector3(displayedProgress * 1400, 0, 0);
                yield return null;
            }

            // Check if it's time to allow scene activation
            if (operation.progress >= 0.9f)
            {
                float timeElapsed = Time.time - startTime;
                if (timeElapsed >= minLoadingTime && displayedProgress >= 1f)
                {
                    // Determine if a cutscene needs to be played before the scene loads
                    int cutsceneIndex = ShouldPlayCutscene(sceneName);
                    if (cutsceneIndex != -1)
                    {
                        loadingScreen.SetActive(false);
                        cutsceneManager.TriggerCutsceneByIndex(cutsceneIndex);
                        while (!cutsceneCompleted)
                        {
                            yield return null;
                        }
                        cutsceneCompleted = false;
                    }

                    // Default load scene
                    operation.allowSceneActivation = true;
                    yield return null;
                }
            }
            yield return null;
        }
        cutsceneManager.cutsceneObject.SetActive(false);

        // Logics after entering each scene
        if (sceneName == "TowerDefense")
        {
            StartCoroutine(GameManager.Instance.TriggerEnemySpawnAfterDelay());
        }
        if(sceneName == "TowerDefense" || sceneName == "Breeding" || sceneName == "Tutorial")
        {
            GameManager.Instance.isTitleScreen = false;
        }
        if(sceneName == "TitleScreen")
        {
            GameManager.Instance.isTitleScreen = true;
            StopAllCoroutines();
        }
        // Ensure the loading screen gets deactivated
        loadingScreen.SetActive(false);
    }

    public void OnCutsceneComplete()
    {
        cutsceneCompleted = true;
    }

    int ShouldPlayCutscene(string sceneName)
    {
        // Determine the cutscene index based on the scene name and other conditions
        if (sceneName == "Tutorial")
            return 1;
        if (sceneName == "TowerDefense" && GameManager.Instance.currentLevelIndex == 0)
            return 2;
        if (sceneName == "TowerDefense" && GameManager.Instance.currentLevelIndex == 4)
            return 3;

        return -1; // Default, indicating no cutscene should play
    }
}
