using UnityEngine;
using System.Collections;

public class FileBehavior : MonoBehaviour
{
    [SerializeField]
    protected File _File;
    public File File
    {
        get { return _File;  }
        set { _File = value; }
    }

	FileUI _FileUI;

	// Use this for initialization
	void Start ()
    {
		_FileUI = GetComponent<FileUI> ();
	}

	public void SetFile(File file) {
		this._File = file;

		// reset UI
		if (_FileUI == null)
			_FileUI = GetComponent<FileUI> ();
		
		_FileUI.Reset();

	}

    public void StartDragging()
    {
        EventManager.Instance.TriggerEvent(new FileDraggingStartEvent(File.Name, "HDD"));
    }
}
