using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public float minLoadingTime = 2f; // Minimum loading time in seconds
    public float smoothLoadingSpeed = 0.5f; // Speed of the loading bar's smooth progress

    // This function should be called directly by the button's onClick event
    public void LoadScene(string sceneName)
    {
        loadingScreen.SetActive(true); // Immediately activate loading screen
        StartCoroutine(DelayedLoad(sceneName));
    }

    // Delays the start of the scene loading to ensure the loading screen appears instantly
    IEnumerator DelayedLoad(string sceneName)
    {
        // Reset the loading bar's displayed progress and visual value at the start
        loadingBar.value = 0f;
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
                loadingBar.value = displayedProgress;
                yield return null;
            }

            // Check if it's time to allow scene activation
            if (operation.progress >= 0.9f)
            {
                float timeElapsed = Time.time - startTime;
                if (timeElapsed >= minLoadingTime && displayedProgress >= 1f)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }

        if(sceneName == "TowerDefense")
        {
            StartCoroutine(GameManager.Instance.TriggerEnemySpawnAfterDelay());
        }
        // Ensure the loading screen gets deactivated
        loadingScreen.SetActive(false);
    }
}
