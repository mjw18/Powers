using UnityEngine;
using System.Collections;
using ExtendedEvents;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public GameObject EntityToSpawn;

    //Reference to an object pool
    private ObjectPool m_ObjPoolRef;

    //Use start to make sure event manager is set up
    void Awake()
    {
        //Register listener to remotely init pool
        UnityAction<InitSpawnersMessage> spawnMessage = Init;
        EventManager.RegisterListener<InitSpawnersMessage>(spawnMessage);
    }

    //Setup callbback to remotely int pool
    public void Init(InitSpawnersMessage initMessage)
    {
        Debug.Log("Initializing " + m_ObjPoolRef);
        if (m_ObjPoolRef) return;

        m_ObjPoolRef = GameManager.instance.GetObjectPool(EntityToSpawn);

        if (!m_ObjPoolRef) Debug.Log("Object pool reference failed");
    }

    //Return reference to spawned gameobject
    public GameObject Spawn(bool SetActive = true)
    {
        GameObject obj = m_ObjPoolRef.NextPooledObject(false);

        //If no inactive objects in pool, return
        if (obj == null)
        {
            Debug.Log("No spawnable entities in " + m_ObjPoolRef);
            return null;
        }

        obj.transform.position = transform.position;
        obj.SetActive(false);
        return obj;
    }


}
