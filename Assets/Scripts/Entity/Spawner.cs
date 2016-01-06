using UnityEngine;
using System.Collections;
using ExtendedEvents;
using UnityEngine.Events;

//Spawns given game object at its transform position. Need to regulate when it initializes

public class Spawner : MonoBehaviour
{
    public GameObject EntityToSpawn;

    //Reference to an object pool
    private ObjectPool m_ObjPoolRef;

    //Use start to make sure event manager is set up
    void Start()
    {
        //Register listener to remotely init pool
        UnityAction<InitSpawnersMessage> spawnMessage = Init;
        EventManager.RegisterListener<InitSpawnersMessage>(spawnMessage);
    }

    //Setup callbback to remotely int pool
    public void Init(InitSpawnersMessage initMessage)
    {
        if (m_ObjPoolRef) return;

        m_ObjPoolRef = GameManager.instance.GetObjectPool(EntityToSpawn);

        if (!m_ObjPoolRef) Debug.Log("Object pool reference failed");
    }

    //Return reference to spawned gameobject
    public GameObject Spawn(bool SetActive = true)
    {
        //No object pool, just using position of spawner
        if (m_ObjPoolRef == null) return null;

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
