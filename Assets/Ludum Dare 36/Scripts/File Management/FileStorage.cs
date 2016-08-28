using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;

public class FileStorage : MonoBehaviour
{

	const string FILE_PREFAB = "Prefabs/File";


    [SerializeField]
    protected float _Size = 1474.56f;
    public string SizeInText
    {
        get { return FileUtilities.GetSizeText(_Size); }
    }

    [SerializeField]
    protected float _TransferSpeed = 25; // KB per second
    public float TransferSpeed
    {
        get { return _TransferSpeed; }
    }

    [SerializeField]
    protected List<File> _Files;
    public List<File> Files
    {
        get { return _Files; }
    }

    [SerializeField]
    protected BoolReactiveProperty _IsEjected = new BoolReactiveProperty();
    public BoolReactiveProperty IsEjected
    {
        get { return _IsEjected; }
    }

    [SerializeField]
    protected float _EjectRefreshDuration = 5f;

	GridLayoutGroup _GridLayout;
	List<FileBehavior> _FileBehaviors = new List<FileBehavior>();

    // Use this for initialization
    void Start()
    {
        FileBehavior[] files = GetComponentsInChildren<FileBehavior>();
        foreach (FileBehavior fileBehavior in files)
            _Files.Add(fileBehavior.File);

		_GridLayout = GetComponentInChildren<GridLayoutGroup> ();
    }

    public void Eject()
    {
        if (!_IsEjected.Value)
        {
            StartCoroutine(EjectRoutine());
        }
    }

    protected IEnumerator EjectRoutine()
    {
        _IsEjected.Value = true;

        yield return new WaitForSeconds(_EjectRefreshDuration);

        _IsEjected.Value = false;
    }

	public void GenerateFile(File file) {
		GameObject filePrefab = Instantiate (Resources.Load (FILE_PREFAB) as GameObject);

		// place
		if (_GridLayout == null) 
			_GridLayout = GetComponentInChildren<GridLayoutGroup> ();

		filePrefab.transform.parent = _GridLayout.transform;
		filePrefab.transform.localScale = new Vector3 (1, 1, 1);

		// set the data
		FileBehavior fileBehavior = filePrefab.GetComponent<FileBehavior> ();
		fileBehavior.SetFile (file);

		_Files.Add (fileBehavior.File);
		_FileBehaviors.Add (fileBehavior);
	}

	public void DeleteFiles() {
		foreach (FileBehavior fileBehavior in _FileBehaviors) {
			Destroy (fileBehavior.gameObject);
		}

		_Files.Clear ();
		_FileBehaviors.Clear ();
	}

}
