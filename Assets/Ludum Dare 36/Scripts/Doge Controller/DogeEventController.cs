using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DogeEventController : MonoBehaviour {

	FloppyGameController _FloppyGameController;

    [System.Serializable]
    public struct DogeEvent
    {
        public string Event;
        public float Time;
    };

    [System.Serializable]
    public struct DogeEvents
    {
        public int LevelIndex;
        public DogeEvent[] Events;
    };

    public DogeEvents[] EventsData;

	// Use this for initialization
	void Start () {
		_FloppyGameController = GetComponent<FloppyGameController> ();
		_FloppyGameController.OnLoadLevel += Handle_OnLoadLevel;
	}

	void Handle_OnLoadLevel (int levelIndex)
	{
        // hardcoded events

        IEnumerable<DogeEvents> events = EventsData.Where(x => x.LevelIndex == levelIndex);
        if (events.Count() > 0)
        {
            DogeEvents dogeEvents = events.First();
            foreach(DogeEvent ev in dogeEvents.Events)
            {
                EventManager.Instance.TriggerEvent(new DogeStartSpeakEvent(ev.Event, ev.Time));
            }
        }
	}
}
