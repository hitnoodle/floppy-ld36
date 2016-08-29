using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EjectUI : MonoBehaviour
{
    public static bool FIRST = false;

    public string USBStorageID;
    public float EjectRestartDuration = 30f;
    public GameObject AngryObject;

    protected Button _Button;
    protected Image _Image;
    protected FileStorage _Storage;

    // Use this for initialization
    void Start()
    {
        _Button = GetComponent<Button>();
        _Image = GetComponent<Image>();
        _Storage = GetComponentInParent<FileStorage>();

        if (!FIRST)
        {
            FIRST = true;
            _Button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        _Button.interactable = false;
        StartCoroutine(EjectRoutine());
    }

    IEnumerator EjectRoutine()
    {
        AngryObject.SetActive(true);
        EventManager.Instance.TriggerEvent(new CancelAllTransferEvent(USBStorageID));

        yield return new WaitForSeconds(1f);

        AngryObject.SetActive(false);
        _Storage.HidePanel();

        yield return new WaitForSeconds(1f);

        _Image.enabled = false;
        _Storage.ShowPanel();

        yield return new WaitForSeconds(EjectRestartDuration);

        _Image.enabled = true;
        _Button.interactable = true;
    }
}
