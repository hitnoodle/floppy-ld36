using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyCancelUI : MonoBehaviour
{
    public static bool FIRST = false;

    public float DisabledChance = 50f;
    public GameObject AngryObject;

    protected Button _Button;
    protected Image _Image;
    protected CopyUI _CopyUI;

	// Use this for initialization
	void Start ()
    {
        _Button = GetComponent<Button>();
        _Image = GetComponent<Image>();
        _CopyUI = GetComponentInParent<CopyUI>();

        if (!FIRST)
        {
            FIRST = true;
            _Button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            bool disabled = Random.Range(0, 100) < DisabledChance;
            if (disabled)
            {
                _Button.interactable = false;
                _Image.color = Color.grey;
            }
            else
            {
                _Button.onClick.AddListener(OnButtonClick);
            }
        }
	}

    void OnButtonClick()
    {
        _Button.interactable = false;
        _Image.color = Color.grey;

        StartCoroutine(CancelRoutine());
    }

    IEnumerator CancelRoutine()
    {
        AngryObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _CopyUI.Cancel();
    }
}
