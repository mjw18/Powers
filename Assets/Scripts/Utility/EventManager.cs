using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace ExtendedEvents
{
    class EventMap<T>
    {
        public Dictionary<MessageKey, GenericUnityEvent<T>> eventMap;

        public EventMap()
        {
            eventMap = new Dictionary<MessageKey, GenericUnityEvent<T>>();
        }
    }

    public class EventManager : MonoBehaviour
    { 
        private EventMap<DBNull> eventDictionary;
        private EventMap<int> intEventDictionary;
        private EventMap<Collision2D> collisionEventDictionary;
        private EventMap<RaycastHit2D> rayHitEventDictionary;

        public static EventManager instance;

        void Awake()
        {
            Debug.Log("Initializing EventManager");

            if (instance == null)
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
            if (eventDictionary == null)
            {
                eventDictionary = new EventMap<System.DBNull>();
            }
            if (intEventDictionary == null)
            {
                intEventDictionary = new EventMap<int>();
            }
            if (collisionEventDictionary == null)
            {
                collisionEventDictionary = new EventMap<Collision2D>();
            }
            if (rayHitEventDictionary == null)
            {
                rayHitEventDictionary = new EventMap<RaycastHit2D>();
            }
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener(MessageKey key, UnityAction callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<System.DBNull> thisEvent = null;

            if (instance.eventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.naEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new GenericUnityEvent<System.DBNull>();
                thisEvent.naEvent.AddListener(callback);
                instance.eventDictionary.eventMap.Add(key, thisEvent);
            }
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener(MessageKey key, UnityAction<int> callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<int> thisEvent = null;

            if (instance.intEventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new GenericUnityEvent<int>();
                thisEvent.oaEvent.AddListener(callback);
                instance.intEventDictionary.eventMap.Add(key, thisEvent);
            }
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener(MessageKey key, UnityAction<Collision2D> callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<Collision2D> thisEvent = null;

            if (instance.collisionEventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new GenericUnityEvent<Collision2D>();
                //Change to just using this in everything
                thisEvent.oaEvent = new UnityEventWrapper<Collision2D>();
                thisEvent.oaEvent.AddListener(callback);
                instance.collisionEventDictionary.eventMap.Add(key, thisEvent);
            }
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener(MessageKey key, UnityAction<RaycastHit2D> callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<RaycastHit2D> thisEvent = null;

            if (instance.rayHitEventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new GenericUnityEvent<RaycastHit2D>();
                thisEvent.oaEvent.AddListener(callback);
                instance.rayHitEventDictionary.eventMap.Add(key, thisEvent);
            }
        }

        public static void RemoveListener(MessageKey key, UnityAction callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<System.DBNull> thisEvent = null;

            if (instance.eventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.naEvent.RemoveListener(callback);
            }
        }

        public static void RemoveListener(MessageKey key, UnityAction<int> callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<int> thisEvent = null;

            if (instance.intEventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.RemoveListener(callback);
            }
        }

        public static void RemoveListener(MessageKey key, UnityAction<Collision2D> callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<Collision2D> thisEvent = null;

            if (instance.collisionEventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.RemoveListener(callback);
            }
        }

        public static void RemoveListener(MessageKey key, UnityAction<RaycastHit2D> callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<RaycastHit2D> thisEvent = null;

            if (instance.rayHitEventDictionary.eventMap.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.RemoveListener(callback);
            }
        }

        //Post Message to listeners using TGV and invoke 
        public static void PostMessage(MessageKey message)
        {
            GenericUnityEvent<System.DBNull> thisEvent = null;

            if (instance.eventDictionary.eventMap.TryGetValue(message, out thisEvent))
            {
                thisEvent.naEvent.Invoke();
            }
            else
            {
                Debug.Log("No listeners in na for this event: " + message);
            }
        }

        //Post Message to listeners using TGV and invoke 
        public static void PostMessage(MessageKey message, int arg)
        {
            GenericUnityEvent<int> thisEvent = null;

            if (instance.intEventDictionary.eventMap.TryGetValue(message, out thisEvent))
            {
                thisEvent.oaEvent.Invoke(arg);
            }
            else
            {
                Debug.Log("No listeners for this event: " + message);
            }
        }

        //Post Message to listeners using TGV and invoke 
        public static void PostMessage(MessageKey message, Collision2D col)
        {
            GenericUnityEvent<Collision2D> thisEvent = null;

            if (instance.collisionEventDictionary.eventMap.TryGetValue(message, out thisEvent))
            {
                thisEvent.oaEvent.Invoke(col);
            }
            else
            {
                Debug.Log("No listeners for this event: " + message);
            }
        }

        //Post Message to listeners using TGV and invoke 
        public static void PostMessage(MessageKey message, RaycastHit2D hit)
        {
            GenericUnityEvent<RaycastHit2D> thisEvent = null;

            if (instance.rayHitEventDictionary.eventMap.TryGetValue(message, out thisEvent))
            {
                thisEvent.oaEvent.Invoke(hit);
            }
            else
            {
                Debug.Log("No listeners for this event: " + message);
            }
        }
    }

    public enum MessageKey
    {
        Pause,
        ShakeCamera,
        Test,
        EnemyDamaged,
        EnemyDied,
        ChargeHit,
        LaserHit,
        PlayerRespawned,
        PlayerDied
    };

    public class UnityEventWrapper<T> : UnityEvent<T> { }

    //This is superfluous until reflection either works or I give up
    public class GenericUnityEvent<T>
    {
        private UnityEventBase m_Event;
        private T thingToGetTypeFrom;

        //One of these should be null
        public UnityEvent naEvent;
        public UnityEventWrapper<T> oaEvent;

        public GenericUnityEvent()
        {
            //Debug.Log(thingToGetTypeFrom is System.DBNull);
            if (thingToGetTypeFrom == null)
            {
                naEvent = new UnityEvent();
            }
            else
            {
                oaEvent = new UnityEventWrapper<T>();
            }
        }
    }
}