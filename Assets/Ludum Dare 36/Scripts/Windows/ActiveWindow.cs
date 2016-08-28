using UnityEngine;
using System.Collections;

public class ActiveWindow : MonoBehaviour
{
    protected Transform _Transform;

	// Use this for initialization
	void Start ()
    {
        _Transform = GetComponent<Transform>();
	}

    public void SetToChild(Transform childTransform)
    {
        childTransform.SetParent(_Transform, true);
    }
}
