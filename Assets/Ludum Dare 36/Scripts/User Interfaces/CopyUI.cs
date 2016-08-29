using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class CopyUI : MonoBehaviour
{
    public string ID;
    public UIProgressBar ProgressBar;
    
    protected CanvasGroup _CanvasGroup;

    // Use this for initialization
    void Start ()
    {
        _CanvasGroup = GetComponent<CanvasGroup>();
        EventManager.Instance.AddListener<CancelAllTransferEvent>(OnCancelAllTransferEvent);
    }

    void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveListener<CancelAllTransferEvent>(OnCancelAllTransferEvent);
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

    public void Cancel()
    {
        EventManager.Instance.TriggerEvent(new CancelTransferEvent(ID));
        Destroy(gameObject);
    }

    private void OnCancelAllTransferEvent(CancelAllTransferEvent e)
    {
        if (e.StorageID.Equals(ID))
        {
            Cancel();
        }
    }

}
