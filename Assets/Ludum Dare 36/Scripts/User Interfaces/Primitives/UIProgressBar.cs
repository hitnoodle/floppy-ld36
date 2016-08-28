using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField]
    protected FloatReactiveProperty _Percentage = new FloatReactiveProperty();
    public FloatReactiveProperty Percentage
    {
        get { return _Percentage; }
    }

    [SerializeField]
    protected bool _IsHideWhenFull = true;

    [SerializeField]
    protected Image _BarOuter;

    protected RectTransform _Transform;
    protected Image _Bar;

    void Awake()
    {
        _Transform = GetComponent<RectTransform>();
        _Bar = GetComponent<Image>();
    }

    void Start()
    {
        _Percentage.Subscribe(x =>
        {
            float percentage = Mathf.Clamp(x, 0, 100);
            _Transform.localScale = new Vector3(percentage / 100f, 1, 1);

            if (_IsHideWhenFull)
            {
                _Bar.enabled = percentage != 100f;
                if (_BarOuter != null) _BarOuter.enabled = percentage != 100f;
            }
        });
    }
}
