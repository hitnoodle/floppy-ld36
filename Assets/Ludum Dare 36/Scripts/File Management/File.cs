using UnityEngine;
using System.Collections;
using UniRx;

[System.Serializable]
public class File
{
    public enum FileType
    {
        Default,
    };

    [SerializeField]
    protected string _Name;
    public string Name
    {
        get { return _Name; }
    }

    [SerializeField]
    protected float _Size; // In kilobytes (KB)
    public float Size
    {
        get { return _Size; }
    }

    public string SizeInText
    {
        get { return FileUtilities.GetSizeText(_Size); }
    }

    [SerializeField]
    protected FileType _Type = FileType.Default;

    [SerializeField]
    protected FloatReactiveProperty _Progress = new FloatReactiveProperty();
    public FloatReactiveProperty Progress
    {
        get { return _Progress; }
    }

    public File(string name, float size)
    {
        _Name = name;
        _Size = size;

        _Progress = new FloatReactiveProperty();
        _Progress.Value = 0;
    }
}
