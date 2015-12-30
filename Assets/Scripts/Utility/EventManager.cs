using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace ExtendedEvents
{

    public class EventManager : MonoBehaviour
    { 
        private Dictionary<Type, Dictionary<MessageKey, GenericUnityEvent<Type>>> completeDictionary;

        private Dictionary<MessageKey, GenericUnityEvent<System.DBNull>> eventDictionary;
        //Consdier Using reflection to make a single argument event Log
        //Could also use a custom pool
        private Dictionary<MessageKey, GenericUnityEvent<int>> intEventDictionary;

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
            if (completeDictionary == null)
            {
                completeDictionary = new Dictionary<Type, Dictionary<MessageKey, GenericUnityEvent<Type> > >();
            }
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<MessageKey, GenericUnityEvent<System.DBNull>>();
            }
            if (intEventDictionary == null)
            {
                intEventDictionary = new Dictionary<MessageKey, GenericUnityEvent<int>>();
            }
        }

        Dictionary<MessageKey, GenericUnityEvent<T>> MakeDict<T>()
        {
            return new Dictionary<MessageKey, GenericUnityEvent<T>>();
        }

        //Register a single argument delegate
        public static void RegisterListener<T>(MessageKey key, UnityAction<T> callback)
        {
            //Get paramters of the callback method
            System.Reflection.ParameterInfo[] p = callback.Method.GetParameters();
            Type[] paramTypes = new Type[p.GetUpperBound(0)];
            //Get types of the parameters
            for (int i = 0; i <= p.GetUpperBound(0); ++i)
            {
                paramTypes[i] = p[i].ParameterType;
            }

            //Is this dictionary in the complete map yet?
            Dictionary<MessageKey, GenericUnityEvent<Type> > tempDict = null;
            
            if (!instance.completeDictionary.TryGetValue(paramTypes[0], out tempDict))
            {
                tempDict = new Dictionary<MessageKey, GenericUnityEvent<t>>();
                instance.completeDictionary.Add(typeof(T), tempDict);
            }
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener(MessageKey key, UnityAction callback)
        {
            callback.Method.GetGenericArguments();
            Type type = callback.Method.ReflectedType;
            Debug.Log("this has a type : " + type);

            if (EventManager.instance == null) return;
            GenericUnityEvent<System.DBNull> thisEvent = null;

            if (instance.eventDictionary.TryGetValue(key, out thisEvent))
            {
                thisEvent.naEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new GenericUnityEvent<System.DBNull>();
                thisEvent.naEvent.AddListener(callback);
                instance.eventDictionary.Add(key, thisEvent);
            }
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener(MessageKey key, UnityAction<int> callback)
        {
            System.Reflection.ParameterInfo[] type = callback.Method.GetParameters();
            for( int i = 0; i <= type.GetUpperBound(0); ++i)
            {
                Debug.Log(type[i].ParameterType + "  " + i);
            }

            if (EventManager.instance == null) return;
            GenericUnityEvent<int> thisEvent = null;

            if (instance.intEventDictionary.TryGetValue(key, out thisEvent))
            {
                thisEvent.oaEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new GenericUnityEvent<int>();
                thisEvent.oaEvent.AddListener(callback);
                instance.intEventDictionary.Add(key, thisEvent);
            }
        }

        public static void RemoveListener(MessageKey key, UnityAction callback)
        {
            if (EventManager.instance == null) return;
            GenericUnityEvent<System.DBNull> thisEvent = null;

            if (instance.eventDictionary.TryGetValue(key, out thisEvent))
            {
                thisEvent.naEvent.RemoveListener(callback);
            }
        }

        //Post Message to listeners using TGV and invoke 
        public static void PostMessage(MessageKey message, int arg)
        {
            GenericUnityEvent<int> thisEvent = null;

            if (instance.intEventDictionary.TryGetValue(message, out thisEvent))
            {
                thisEvent.oaEvent.Invoke(arg);
            }
            else
            {
                Debug.Log("No listeners for this event: " + message);
            }
        }

        //Post Message to listeners using TGV and invoke 
        public static void PostMessage(MessageKey message)
        {
            GenericUnityEvent<System.DBNull> thisEvent = null;

            if (instance.eventDictionary.TryGetValue(message, out thisEvent))
            {
                thisEvent.naEvent.Invoke();
            }
            else
            {
                Debug.Log("No listeners in na for this event: " + message);
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

    public class GenericUnityEvent<T>
    {
        private UnityEventBase m_Event;
        private T thingToGetTypeFrom;

        //One of these should be null
        public UnityEvent naEvent;
        public UnityEventWrapper<T> oaEvent;

        public GenericUnityEvent(Type t)
        {
            //Debug.Log(thingToGetTypeFrom is System.DBNull);
            if (thingToGetTypeFrom == null)
            {
                naEvent = new UnityEvent();
            }
            else
            {
                oaEvent = Activator.CreateInstance<UnityEventWrapper<T>>();

                MethodInfo method = typeof(T).GetMethod("UnityEventWrapper", BindingFlags.CreateInstance);
                method = method.MakeGenericMethod(typeof(T));
                if (!method.IsConstructor) Debug.Log("Nope, not how that works");
                object[] arguments;
                oaEvent = method.Invoke(oaEvent, arguments);
            }
        }
    }
}