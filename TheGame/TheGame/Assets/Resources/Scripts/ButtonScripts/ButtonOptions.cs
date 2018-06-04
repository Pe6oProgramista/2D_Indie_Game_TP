using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;

public class ButtonOptions : MonoBehaviour {
    private int sceneIndex;
    private Animator animator;

    public void OnSelect(int sceneIndex)
    {
        ApplicationModel.sceneIndexes.Add(SceneManager.GetActiveScene().buildIndex);
        this.sceneIndex = sceneIndex;
        animator = GameObject.Find("Fade/Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        Invoke("ChangeScene", 1);
    }

    public void Back()
    {
        int backSceneIndex = ApplicationModel.sceneIndexes.ToArray()[ApplicationModel.sceneIndexes.Count - 1];
        ApplicationModel.sceneIndexes.Remove(backSceneIndex);
        sceneIndex = backSceneIndex;
        animator = GameObject.Find("Fade/Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        InvokeRealTime("ChangeScene", 1);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Resume(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        int backSceneIndex = ApplicationModel.sceneIndexes.ToArray()[ApplicationModel.sceneIndexes.Count - 1];
        ApplicationModel.sceneIndexes.Remove(backSceneIndex);
        backSceneIndex = ApplicationModel.sceneIndexes.ToArray()[ApplicationModel.sceneIndexes.Count - 1];
        ApplicationModel.sceneIndexes.Remove(backSceneIndex);
        sceneIndex = backSceneIndex;
        animator = GameObject.Find("Fade/Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        InvokeRealTime("ChangeScene", 1);
    }

    public void InvokeRealTime(string functionName, float delay)
    {
        StartCoroutine(InvokeRealTimeHelper(functionName, delay));
    }
    private IEnumerator InvokeRealTimeHelper(string functionName, float delay)
    {
        float timeElapsed = 0f;
        while (timeElapsed < delay)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        SendMessage(functionName);
    }


}
