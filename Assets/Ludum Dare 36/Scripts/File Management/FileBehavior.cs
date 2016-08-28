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

	// Use this for initialization
	void Start ()
    {
	
	}

    public void StartDragging()
    {
        EventManager.Instance.TriggerEvent(new FileDraggingStartEvent(File.Name, "HDD"));
    }
}
