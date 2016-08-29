using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DogeController : MonoBehaviour {

	public Animator DogeAnimator;

	public string CurrentSpeakID;
	public bool DoSpeak;

	public List<DogeModel> DogeLists = new List<DogeModel>();



	// Use this for initialization
	void Start () {
		EventManager.Instance.AddListener<DogeStartSpeakEvent> (OnDogeStartSpeakEvent);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (DoSpeak) {
			DoSpeak = false;
			Show (CurrentSpeakID, 0);
		}

	}

	public void Show(string speakID, float waitTime) {
		StartCoroutine (ShowRoutine(speakID, waitTime));
	}

	IEnumerator ShowRoutine(string speakID, float waitTime) {

		yield return new WaitForSeconds (waitTime);

        if (!speakID.Equals("TRANSPARENT"))
        {
            if (speakID.Contains("STOP"))
                SoundManager.PlaySoundEffect("poeh");
            else
                SoundManager.PlaySoundEffect("doge");
        }

        DogeModel dogeModel = DogeLists.Where (x => x.ID == speakID).FirstOrDefault ();

		if (dogeModel != null) {
			if (dogeModel.SpeakObject != null)
				dogeModel.SpeakObject.SetActive (dogeModel.IsShowObject);

			transform.localPosition = dogeModel.Position;

			PlayDoge(dogeModel.Animation);

			CurrentSpeakID = speakID;
		}
	}


	void OnDogeStartSpeakEvent(DogeStartSpeakEvent eve) {
		Show (eve.ID, eve.WaitTime);
	}

	void PlayDoge(DogeModel.DOGE_ANIMATIONS animation) {
		switch (animation) {
		case DogeModel.DOGE_ANIMATIONS.TRANSPARENT:
			DogeAnimator.Play ("Transparent");
			break;
		case DogeModel.DOGE_ANIMATIONS.BARK:
			DogeAnimator.Play ("DogeIdle");
			break;
		case DogeModel.DOGE_ANIMATIONS.IDLE:
			DogeAnimator.Play ("DogeBark");
			break;
		case DogeModel.DOGE_ANIMATIONS.STOP:
			DogeAnimator.Play ("DogeStop");
			break;
		}
	}
}
