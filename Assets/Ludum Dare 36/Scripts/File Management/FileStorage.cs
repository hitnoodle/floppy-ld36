﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using UniRx;
using UnityEngine.EventSystems;

public class FileStorage : MonoBehaviour
{
	const string FILE_PREFAB = "Prefabs/File";

	public delegate void _OnTransferFile(FileStorage fileStorage, List<File> files);
	public _OnTransferFile OnTransferFile;

    [Header("Attributes")]

    [SerializeField]
    protected string _Name;
    public string Name
    {
        get { return _Name; }
    }

    [SerializeField]
    protected float _Size = 1474.56f;
    public float Size
    {
        get { return _Size; }
    }

    public string SizeInText
    {
        get { return FileUtilities.GetSizeText(_Size); }
    }

	public LevelModel.StorageType StorageType;

    [SerializeField]
    protected float _CurrentSize = 1474.56f;
    public float CurrentSize
    {
        get { return _CurrentSize;  }
        set { _CurrentSize = value; }
    }

    [SerializeField]
    protected float _TransferSpeed = 25; // KB per second
    public float TransferSpeed
    {
        get { return _TransferSpeed; }
    }

    [Header("Files")]

    [SerializeField]
    protected List<File> _Files;
    public List<File> Files
    {
        get { return _Files; }
    }

    protected List<FileBehavior> _FileBehaviors;

    [Header("Eject")]

    [SerializeField]
    protected BoolReactiveProperty _IsEjected = new BoolReactiveProperty();
    public BoolReactiveProperty IsEjected
    {
        get { return _IsEjected; }
    }

    [SerializeField]
    protected float _EjectRefreshDuration = 5f;

	GridLayoutGroup _GridLayout;

    [Header("UI")]

    [SerializeField]
    protected RectTransform _ParentFileUI;

	[SerializeField]
	protected Button _EjectButton;

    [SerializeField]
    protected CopyUI _CopyUIPrefab;

    [SerializeField]
    protected Transform _CopyUIParentTransform;

	private CanvasGroup _CanvasGroup;
	private IEnumerator _EjectRoutine;

    [HideInInspector]
	public List<File> TransferringFile = new List<File> ();

    // Use this for initialization
    void Start()
    {
        _CurrentSize = _Size;
        _FileBehaviors = new List<FileBehavior>();

		_GridLayout = GetComponentInChildren<GridLayoutGroup> ();
		_CanvasGroup = GetComponent<CanvasGroup> ();

		if (_EjectButton != null) {
			_EjectButton.onClick.AddListener (() => {
				Eject();
			});
		}
    }

    public void Eject()
    {
        if (!_IsEjected.Value)
        {
            SoundManager.PlaySoundEffect("put");

			_EjectRoutine = EjectRoutine();
            StartCoroutine(_EjectRoutine);
        }
    }

    protected IEnumerator EjectRoutine()
    {
        _IsEjected.Value = true;
		HidePanel ();
		TransferFiles ();

        yield return new WaitForSeconds(_EjectRefreshDuration);

        _IsEjected.Value = false;
		ShowPanel ();
    }

	public void StopEjectRoutine() {
		if (_EjectRoutine != null) {
			StopCoroutine (_EjectRoutine);
            _EjectRoutine = null;
		}
		_IsEjected.Value = false;
	}

	public void GenerateFile(File file)
    {
		GameObject filePrefab = Instantiate (Resources.Load (FILE_PREFAB) as GameObject);

		// place
		if (_GridLayout == null) 
			_GridLayout = GetComponentInChildren<GridLayoutGroup> ();

        filePrefab.transform.SetParent(_GridLayout.transform);
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
		
    public File CloneFile(File file)
    {
        File newFile = new File(file.Name, file.Size);
        _Files.Add(newFile);

        FileBehavior newFileBehavior = Instantiate(Resources.Load<FileBehavior>(FILE_PREFAB));
        newFileBehavior.File = newFile;
        newFileBehavior.transform.SetParent(_ParentFileUI);
        newFileBehavior.transform.localScale = new Vector3(1, 1, 1);
        _FileBehaviors.Add(newFileBehavior);

        // Implisit
        Destroy(newFileBehavior.GetComponent<EventTrigger>());

        return newFile;
    }

    public File GetFile(string fileName)
    {
        IEnumerable<File> fileFound = _Files.Where(x => x.Name.Equals(fileName));
        if (fileFound.Count() >= 1)
            return fileFound.First();

        return null;
    }

    public bool DeleteFile(string fileName)
    {
        IEnumerable<File> fileFound = _Files.Where(x => x.Name.Equals(fileName));
        if (fileFound.Count() >= 1)
        {
            File file = fileFound.First();

            int index = _Files.IndexOf(file);
            _Files.RemoveAt(index);

            FileBehavior fileBehavior = _FileBehaviors.ElementAt(index);
            _FileBehaviors.RemoveAt(index);
            Destroy(fileBehavior.gameObject);
        }

        return false;
    }

    public string[] GetIdleFiles()
    {
        List<string> idleFiles = new List<string>();

        IEnumerable<File> fileFound = _Files.Where(x => x.IsTransferring == false);
        if (fileFound.Count() >= 1)
        {
            foreach (File file in fileFound)
            {
                idleFiles.Add(file.Name);
            }
        }

        return idleFiles.ToArray();
    }

	public void HidePanel() {
		_CanvasGroup.alpha = 0;
		_CanvasGroup.interactable = false;
		_CanvasGroup.blocksRaycasts = false;
	}

    public void HidePanelWithSound()
    {
        HidePanel();
        SoundManager.PlaySoundEffect("put");
    }

	public void ShowPanel() {
		_CanvasGroup.alpha = 1;
		_CanvasGroup.interactable = true;
		_CanvasGroup.blocksRaycasts = true;

        transform.localPosition = new Vector3(Random.Range(-640, 170), Random.Range(0, 340), 0);
    }

	void TransferFiles() { 
		if (OnTransferFile != null) {
			OnTransferFile (this, Files);
		}
		DeleteFiles ();
		CurrentSize = Size;
	}

	public void EnableEject() {
		if (_EjectButton != null)
			_EjectButton.interactable = true;
	}

	public void DisableEject() {
		if (_EjectButton != null)
			_EjectButton.interactable = false;
	}

    public void ShowAlertFull()
    {
        EventManager.Instance.TriggerEvent(new AlertFullEvent(_Name));
    }

    public void ShowCopy(FloatReactiveProperty progress, BoolReactiveProperty finished)
    {
        if (_CopyUIPrefab != null)
        {
            CopyUI copyUI = Instantiate(_CopyUIPrefab);
            copyUI.ID = Name;

            progress.Subscribe(x =>
            {
                float clampVal = Mathf.Clamp(x, 0, 100);
                copyUI.ProgressBar.Percentage.Value = clampVal;
            });

            finished.Where(x => x == true).Subscribe(x => Destroy(copyUI.gameObject));

            copyUI.transform.SetParent(_CopyUIParentTransform);
            copyUI.transform.localPosition = new Vector3(Random.Range(-250, 250), Random.Range(-150, 150), 0);
            copyUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
