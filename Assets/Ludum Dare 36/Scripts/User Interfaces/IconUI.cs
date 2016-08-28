using UnityEngine;
using System.Collections;
using UniRx;

public class IconUI : MonoBehaviour
{
    [SerializeField]
    protected FileStorage _StoragePanelUI;

    [SerializeField]
    protected IntReactiveProperty _ClickedTimes = new IntReactiveProperty();

	// Use this for initialization
	void Start ()
    {
        _ClickedTimes.Where(x => x == 2).Subscribe(x => {
            _StoragePanelUI.ShowPanel();
            _ClickedTimes.Value = 0;
        });
	}

    public void ClickIcon()
    {
        _ClickedTimes.Value++;
    }
}
