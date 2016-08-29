using UnityEngine;
using System.Collections;
using System;

public class AlertUI : MonoBehaviour
{
    public string ID;

    protected CanvasGroup _CanvasGroup;

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
        }
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
