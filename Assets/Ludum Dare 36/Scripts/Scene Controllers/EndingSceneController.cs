using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{
    public DaySplash DaySplash;
    public Animator EndingAnimator;

    protected IEnumerator _EndRoutine;

	// Use this for initialization
	void Start ()
    {
        DaySplash.ShowSplash(4000);
	}

    public void End()
    {
        if (_EndRoutine == null)
        {
            _EndRoutine = EndRoutine();
            StartCoroutine(_EndRoutine);
        }
    }

    IEnumerator EndRoutine()
    {
        EndingAnimator.enabled = true;
        yield return new WaitForSeconds(4f);
        SoundManager.PlaySoundEffectOneShot("startup");
        yield return new WaitForSeconds(16);
        SceneManager.LoadScene("SplashScene");
    }
}
