using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(FileBehavior))]
public class FileUI : MonoBehaviour
{
    protected FileBehavior _FileBehavior;

    [SerializeField]
    protected Image _FileImage;

    [SerializeField]
    protected Text _FileText;

    [SerializeField]
    protected UIProgressBar _FileProgressBar;

    void Awake()
    {
        _FileBehavior = GetComponent<FileBehavior>();
    }

    // Use this for initialization
    void Start()
    {
        _FileText.text = _FileBehavior.File.Name;
        _FileBehavior.File.Progress.Subscribe(x => _FileProgressBar.Percentage.Value = x);
    }
}
