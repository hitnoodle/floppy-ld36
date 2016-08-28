﻿using UnityEngine;
using System.Collections;
using UniRx;

public class File : MonoBehaviour
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

    [SerializeField]
    protected bool _Transferred = false; 

	// Use this for initialization
	void Start ()
    {
	
	}
}