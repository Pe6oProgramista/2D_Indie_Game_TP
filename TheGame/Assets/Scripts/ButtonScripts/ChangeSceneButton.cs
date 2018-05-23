using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour, ISelectHandler
{
    public Animator animator;
    public int sceneIndex;

    public void OnSelect(BaseEventData eventData)
    {
        animator.SetTrigger("FadeOut");
        Invoke("ChangeScene", 1);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
