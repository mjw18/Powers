using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour{

    public List<GameObject> objectPool;
    public GameObject pooledObject;
    public int size;
    public int maxOverflow;

    void Awake()
    {
        InitPool();
    }

    public void InitPool()
    {
        if(objectPool.Count > 0)  objectPool.Clear();

        for (int i = 0; i < size; i++)
        {
            GameObject temp = Instantiate(pooledObject) as GameObject;
            objectPool.Add(temp);
            temp.SetActive(false);
        }
    }

    //Returns inactive gameobject, input whether to ativate object or not
    public GameObject NextPooledObject(bool setActive = true)
    {
        for (int i = 0; i < size; i++)
        {
            if(!objectPool[i].activeSelf)
            {
                //Don't set active here?
                objectPool[i].SetActive(setActive);
                return objectPool[i];
            }
        }

        //New GameObject to be added
        GameObject temp = null;

        //If we are allowed to add more objects
        if (objectPool.Count - size < maxOverflow)
        {
            temp = Instantiate(pooledObject) as GameObject;
            objectPool.Add(temp);
            temp.SetActive(true);
        }

        //return new object, returns null if no more room and no inactive objects
        return temp;
    }

    //Coroutine cleaner, could return wait for seconds to slow down remove calls
    public IEnumerator CoCleanPool()
    {
        while(objectPool.Count - size > 0)
        {
            int lastIndex = objectPool.Count - 1;
            GameObject t = objectPool[lastIndex];
            objectPool.RemoveAt(lastIndex);
            Destroy(t);
            yield return null;
        }
    }

    //This is probably not performant, consider coroutine
    public void CleanPool()
    {
        for(int i = 0; i < (objectPool.Count - size); i++)
        {
            int lastIndex = objectPool.Count - 1;
            GameObject t = objectPool[lastIndex];
            objectPool.RemoveAt(lastIndex);
            Destroy(t);
        }
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Debug.Log("This pool was destroyed");
    }
}
