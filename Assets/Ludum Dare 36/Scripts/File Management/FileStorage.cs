using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.EventSystems;

public class FileStorage : MonoBehaviour
{
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

    [Header("UI")]

    [SerializeField]
    protected RectTransform _ParentFileUI;

    // Use this for initialization
    void Start()
    {
        _CurrentSize = _Size;
        _FileBehaviors = new List<FileBehavior>();

        // ONLY FOR TESTING, STORAGE MUST BE EMPTY AT THE BEGINNIG
        FileBehavior[] files = GetComponentsInChildren<FileBehavior>();
        foreach (FileBehavior fileBehavior in files)
        {
            _CurrentSize -= fileBehavior.File.Size;
            _Files.Add(fileBehavior.File);
            _FileBehaviors.Add(fileBehavior);
        }
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

    public File CloneFile(File file)
    {
        File newFile = new File(file.Name, file.Size);
        _Files.Add(newFile);

        FileBehavior newFileBehavior = Instantiate(Resources.Load<FileBehavior>("File"));
        newFileBehavior.File = newFile;
        newFileBehavior.transform.SetParent(_ParentFileUI);
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
}
