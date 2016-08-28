using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(File))]
public class FileUI : MonoBehaviour
{
    protected File _File;

    [SerializeField]
    protected Image _FileImage;

    [SerializeField]
    protected Text _FileText;

    [SerializeField]
    protected UIProgressBar _FileProgressBar;

    void Awake()
    {
        _File = GetComponent<File>();
    }

    // Use this for initialization
    void Start()
    {
        _FileText.text = _File.Name;
        _File.Progress.Subscribe(x => _FileProgressBar.Percentage.Value = x);
    }
}
