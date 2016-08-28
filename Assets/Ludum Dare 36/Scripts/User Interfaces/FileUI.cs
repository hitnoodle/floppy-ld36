using UnityEngine;
using System.Collections;
using System;
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

    protected IDisposable _ProgressBarDisposable;

    void Awake()
    {
        _FileBehavior = GetComponent<FileBehavior>();
    }

    // Use this for initialization
    void Start()
    {
        _FileText.text = _FileBehavior.File.Name;
        _ProgressBarDisposable = _FileBehavior.File.Progress.Subscribe(x => _FileProgressBar.Percentage.Value = x);
    }

	public void Reset()
    {
        if (_ProgressBarDisposable != null)
        {
            // dispose previous subscription
            _ProgressBarDisposable.Dispose();

            //resubscribe
            _ProgressBarDisposable = _FileBehavior.File.Progress.Subscribe(x => _FileProgressBar.Percentage.Value = x);
        }

		_FileText.text = _FileBehavior.File.Name;

		// set image
		if (_FileBehavior.File.ImageURL != null && _FileBehavior.File.ImageURL != "") {
			Texture2D loadedTexture2D = Resources.Load (_FileBehavior.File.ImageURL) as Texture2D;
			Sprite loadedSprite = Sprite.Create (loadedTexture2D, new Rect (0,0, loadedTexture2D.width, loadedTexture2D.height), Vector2.zero);
			_FileImage.sprite = loadedSprite;
		}
	}
}
