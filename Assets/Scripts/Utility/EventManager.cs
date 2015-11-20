using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{

    private Dictionary<MessageKey, UnityEvent> eventDictionary;

    public static EventManager instance;

	void Awake ()
    {
        Debug.Log("Initializing EventManager");

        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Init();
	}

    void Init()
    {
        if(eventDictionary == null)
        {
            eventDictionary = new Dictionary<MessageKey, UnityEvent>();
        }
    }

    //Add a listerner under  the given Unity Event. If null, add
    public static void RegisterListener(MessageKey key, UnityAction callback)
    {
        if (EventManager.instance == null) return;
        UnityEvent thisEvent = null;

        if(instance.eventDictionary.TryGetValue(key, out thisEvent))
        {
            thisEvent.AddListener(callback);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(callback);
            instance.eventDictionary.Add(key, thisEvent);
        }
    }

    public static void RemoveListener(MessageKey key, UnityAction callback)
    {
        if (EventManager.instance == null) return;
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(key, out thisEvent))
        {
            thisEvent.RemoveListener(callback);
        }
    }

    //Post Message to listeners using TGV and invoke 
    public static void PostMessage(MessageKey message)
    {
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(message, out thisEvent))
        { 
            thisEvent.Invoke();
        }
        else
        {
            Debug.Log("No listeners for this event");
            Debug.Log(message);
        }
    }

    public enum MessageKey
    {
        Destroy,
        Pause,
        ShakeCamera,
        Test,
        EnemyDied,
        ChargeHit
    };
}

