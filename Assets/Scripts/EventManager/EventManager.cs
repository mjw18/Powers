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
        public class MessageTypeKey
        {
            public MessageKey key { get; private set; }
            public Type messageType { get; private set; }

            public MessageTypeKey(MessageKey key, Type type)
            {
                this.key = key;
                this.messageType = type;
            }
        }

        private Dictionary<Type, UnityEventWrapper<MessageInfo>> typeLookup = new Dictionary<Type, UnityEventWrapper<MessageInfo>>();
        private Dictionary<MessageKey, UnityEventWrapper<MessageInfo>> messageLookup = new Dictionary<MessageKey, UnityEventWrapper<MessageInfo>>();
        private Dictionary<MessageTypeKey, UnityEventWrapper<MessageInfo>> messageTypeLookup = new Dictionary<MessageTypeKey, UnityEventWrapper<MessageInfo>>();

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
        }

        //Add a listerner under  the given Unity Event. If null, add event and listener
        public static void RegisterListener<T>(UnityAction<T> callback) where T : MessageInfo
        {
            if (EventManager.instance == null) return;

            UnityAction<MessageInfo> intern = (e) => callback.Invoke((T)e);
            UnityEventWrapper<MessageInfo> tempEvent = null;

            if (!instance.typeLookup.TryGetValue(typeof(T), out tempEvent))
            {
                instance.typeLookup[typeof(T)] = new UnityEventWrapper<MessageInfo>();
                instance.typeLookup[typeof(T)].AddListener(intern);
            }
            else
            {
                instance.typeLookup[typeof(T)].AddListener(intern);
            }
        }

        //Add a listerner under  the given Unity Event by Message key. If null, add event and listener
        public static void RegisterKeyListener<T>(UnityAction<T> callback, MessageKey key) where T : MessageInfo
        {
            if (EventManager.instance == null) return;

            UnityAction<MessageInfo> intern = (e) => callback.Invoke((T)e);
            UnityEventWrapper<MessageInfo> tempEvent = null;

            if (!instance.messageLookup.TryGetValue(key, out tempEvent))
            {
                instance.messageLookup[key] = new UnityEventWrapper<MessageInfo>();
                instance.messageLookup[key].AddListener(intern);
            }
            else
            {
                instance.messageLookup[key].AddListener(intern);
            }
        }

        //Alternative register by MessageInfo and Message Key
        public static void RegisterListener<T>(UnityAction<T> callback, MessageKey key) where T : MessageInfo
        {
            if (EventManager.instance == null) return;

            //CreateMessageTypeKey for alt lookup
            MessageTypeKey typeKey = new MessageTypeKey(key, typeof(T));

            UnityAction<MessageInfo> intern = (e) => callback.Invoke((T)e);
            UnityEventWrapper<MessageInfo> tempEvent = null;

            //Alt lookup
            if (!instance.typeLookup.TryGetValue(typeof(T), out tempEvent))
            {
                instance.messageTypeLookup[typeKey] = new UnityEventWrapper<MessageInfo>();
                instance.messageTypeLookup[typeKey].AddListener(intern);
            }
            else
            {
                instance.messageTypeLookup[typeKey].AddListener(intern);
            }

            //Register in MessageType lookup
            RegisterListener<T>(callback);
            //Register in MessageKey lookup
            RegisterKeyListener<T>(callback, key);
        }

        //Post message to all messages of given type
        public static void PostMessage<T>(T message) where T : MessageInfo
        {
            if (!instance.typeLookup.ContainsKey(typeof(T)))
            {
                Debug.Log("typeLookup does not contain " + typeof(T));
                return;
            }

            instance.typeLookup[typeof(T)].Invoke(message);
        }

        //Post message to listeners by key
        public static void PostKeyMessage<T>(MessageKey key, T message) where T : MessageInfo
        {
            if (!instance.messageLookup.ContainsKey(key))
            {
                Debug.Log("messageLookup does not contain " + key);
                return;
            }

            instance.messageLookup[key].Invoke(message);
        }

        //PostBykey AND type (seems redundant, just use the other?)
        public static void PostMessage<T>(T message, MessageKey key) where T : MessageInfo
        {
            MessageTypeKey typeKey = new MessageTypeKey(key, typeof(T));

            if (!instance.messageTypeLookup.ContainsKey(typeKey))
            { 
                Debug.Log("messageTypeLookup does not contain " + typeKey);
                return;
            }

            instance.messageTypeLookup[typeKey].Invoke(message);
        }

        //Remove functions for typeLookup
        public static void RemoveListener<T>(UnityAction<T> callback) where T : MessageInfo
        {
            if (!instance.typeLookup.ContainsKey(typeof(T)))
            {
                Debug.Log("typeLookup does not contain " + typeof(T));
                return;
            }

            instance.typeLookup.Remove(typeof(T));
        }

        public static void RemoveAllListeners<T>(T message) where T : MessageInfo
        {
            if (!instance.typeLookup.ContainsKey(typeof(T)))
            {
                Debug.Log("typeLookup does not contain " + typeof(T));
                return;
            }

            instance.typeLookup[typeof(T)].RemoveAllListeners();
        }

        //Remove in key and message type lookups
        public static void RemoveListener<T>(UnityAction<T> callback, MessageKey key) where T : MessageInfo
        {
            MessageTypeKey typeKey = new MessageTypeKey(key, typeof(T));

            if (!instance.messageTypeLookup.ContainsKey(typeKey))
            {
                Debug.Log("typeLookup does not contain " + typeKey);
                return;
            }

            //Remove typeKey and key lookup entires for specific callback
            instance.messageLookup.Remove(key);
            instance.messageTypeLookup.Remove(typeKey);
        }

        public static void RemoveAllListeners<T>(MessageKey key, T message) where T : MessageInfo
        {
            MessageTypeKey typeKey = new MessageTypeKey(key, typeof(T));

            if (!instance.messageTypeLookup.ContainsKey(typeKey))
            {
                Debug.Log("typeLookup does not contain " + typeKey);
                return;
            }

            //Remove typeKey and key lookup entires for specific callback
            instance.messageLookup[key].RemoveAllListeners();
            instance.messageTypeLookup[typeKey].RemoveAllListeners();
        }

        //Remove in key message lookups
        public static void RemoveKeyListener(MessageKey key)
        {
            if (!instance.messageLookup.ContainsKey(key))
            {
                Debug.Log("messageLookup does not contain " + key);
                return;
            }

            //Remove typeKey and key lookup entires for specific callback
            instance.messageLookup.Remove(key);
        }

        public static void RemoveAllKeyListeners(MessageKey key)
        {
            if (!instance.messageLookup.ContainsKey(key))
            {
                Debug.Log("messageLookup does not contain " + key);
                return;
            }

            //Remove typeKey and key lookup entires for specific callback
            instance.messageLookup[key].RemoveAllListeners();
            instance.messageLookup.Remove(key);
        }
    }

    public class UnityEventWrapper<T> : UnityEvent<T> { }

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
        PlayerDied,
        Null
    };

}