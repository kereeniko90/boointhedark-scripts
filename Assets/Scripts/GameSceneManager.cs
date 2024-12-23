using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour {

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;

    
    public void ReloadGame() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void StartGameLoad() {
        
        StartCoroutine(LoadAsynchronouslyGame());
        
        Time.timeScale = 1f;
    }

    IEnumerator LoadAsynchronouslyGame() {

        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
    }

}
