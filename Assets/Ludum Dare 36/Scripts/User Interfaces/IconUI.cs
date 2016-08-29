using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class IconUI : MonoBehaviour
{
    [SerializeField]
    protected FileStorage _StoragePanelUI;

    [SerializeField]
    protected IntReactiveProperty _ClickedTimes = new IntReactiveProperty();

    protected CanvasGroup _CanvasGroup;

	// Use this for initialization
	void Start ()
    {
        _CanvasGroup = GetComponent<CanvasGroup>();

        _ClickedTimes.Where(x => x == 2).Subscribe(x => {
            _StoragePanelUI.ShowPanel();
            _ClickedTimes.Value = 0;
        });

        _StoragePanelUI.IsEjected.Subscribe(x =>
        {
            if (x)
                HidePanel();
            else
                ShowPanel();
        });
	}

    public void ClickIcon()
    {
        _ClickedTimes.Value++;
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
