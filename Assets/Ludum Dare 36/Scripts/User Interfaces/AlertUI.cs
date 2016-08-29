using UnityEngine;
using System.Collections;
using System;

public class AlertUI : MonoBehaviour
{
    public string ID;

    public bool IsAutoHideAfterShow = false;
    public float AutoHideDelay = 2f;

    protected CanvasGroup _CanvasGroup;
    protected IEnumerator _AutoHideRoutine;

    // Use this for initialization
    void Start ()
    {
        _CanvasGroup = GetComponent<CanvasGroup>();
        EventManager.Instance.AddListener<AlertFullEvent>(OnAlertFullEvent);
    }

    private void OnAlertFullEvent(AlertFullEvent e)
    {
        if (e.StorageID.Equals(ID))
        {
            ShowPanel();
            SoundManager.PlaySoundEffect("denied");
        }
    }

    public void HidePanel()
    {
        _CanvasGroup.alpha = 0;
        _CanvasGroup.interactable = false;
        _CanvasGroup.blocksRaycasts = false;

        if (_AutoHideRoutine != null)
        {
            StopCoroutine(_AutoHideRoutine);
            _AutoHideRoutine = null;
        }

        SoundManager.PlaySoundEffect("pick");
    }

    public void ShowPanel()
    {
        _CanvasGroup.alpha = 1;
        _CanvasGroup.interactable = true;
        _CanvasGroup.blocksRaycasts = true;

        if (IsAutoHideAfterShow && _AutoHideRoutine == null)
        {
            _AutoHideRoutine = AutoHideRoutine();
            StartCoroutine(_AutoHideRoutine);
        }
    }

    private IEnumerator AutoHideRoutine()
    {
        yield return new WaitForSeconds(AutoHideDelay);
        HidePanel();
    }
}
