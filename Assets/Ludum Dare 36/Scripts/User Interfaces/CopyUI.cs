using UnityEngine;
using System.Collections;
using UniRx;

public class CopyUI : MonoBehaviour
{
    public UIProgressBar ProgressBar;
    
    protected CanvasGroup _CanvasGroup;

    // Use this for initialization
    void Start ()
    {
        _CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void HidePanel()
    {
        _CanvasGroup.alpha = 0;
        _CanvasGroup.interactable = false;
        _CanvasGroup.blocksRaycasts = false;
    }

    public void ShowPanel()
    {
        _CanvasGroup.alpha = 1;
        _CanvasGroup.interactable = true;
        _CanvasGroup.blocksRaycasts = true;
    }
}
