using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class FileStorage : MonoBehaviour
{
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

    // Use this for initialization
    void Start()
    {
        FileBehavior[] files = GetComponentsInChildren<FileBehavior>();
        foreach (FileBehavior fileBehavior in files)
            _Files.Add(fileBehavior.File);
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
}
