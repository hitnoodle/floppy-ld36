using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{
    public DaySplash DaySplash;
    public Animator EndingAnimator;

	// Use this for initialization
	void Start ()
    {
        DaySplash.ShowSplash(4000);
	}

    public void End()
    {
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        EndingAnimator.enabled = true;
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene("SplashScene");
    }
}
