using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WifiUI : MonoBehaviour
{
    public static bool FIRST = false;

    public string CloudStorageID;
    public float WifiRestartDuration = 30f;
    public GameObject AngryObject;

    protected Button _Button;
    protected Image _Image;

    // Use this for initialization
    void Start ()
    {
        _Button = GetComponent<Button>();
        _Image = GetComponent<Image>();

        _Button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        _Button.interactable = false;

        SoundManager.PlaySoundEffect("denied");
        StartCoroutine(PauseRoutine());
    }

    IEnumerator PauseRoutine()
    {
        AngryObject.SetActive(true);
        EventManager.Instance.TriggerEvent(new PauseTransferEvent(CloudStorageID));

        yield return new WaitForSeconds(1f);

        AngryObject.SetActive(false);

        yield return new WaitForSeconds(WifiRestartDuration);

        _Button.interactable = true;
        _Image.color = Color.white;
    }
}
