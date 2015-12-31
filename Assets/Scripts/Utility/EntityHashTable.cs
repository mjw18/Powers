using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Make singleton? Holds a list of all entities existing upon Init
public class EntityHashTable {

    //Dictionary which holds id tags for entities (go's)
    //Sinec public, entities could be added manually at instantiation but this seems weak, should use more pooling
    //Make entity class so this doesn't work for ALL gameObjects
    public Dictionary<int, GameObject> entityMap;
    private List<GameObject> goList;
    //Consider using messages?
    public void Init()
    {
        Debug.Log("Initializing EntityTable");

        if (entityMap == null) entityMap = new Dictionary<int, GameObject>();
        else entityMap.Clear();

        if (goList == null) goList = new List<GameObject>();
        else goList.Clear();

        foreach ( GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if(goList.Contains(go))
            {
                continue;
            }
            goList.Add(go);
            entityMap[go.GetInstanceID()] = go;
        }
    }

    void ClearMap()
    {
        entityMap.Clear();
    }

    public GameObject GetEntityFromID(int ID)
    {
        GameObject obj = null;
        if(entityMap.TryGetValue(ID, out obj))
        {
            return obj;
        }

        Debug.Log("Invalid ID");
        return null;
    }

    public void RemoveFromTable(int ID)
    {
        if (entityMap.ContainsKey(ID)) entityMap.Remove(ID);

        else { Debug.Log("Entity isn't registered"); }
    }

    public void AddToTable(GameObject go)
    {
        int ID = go.GetInstanceID();

        if (entityMap.ContainsKey(ID)) return;

        else { entityMap.Add(ID, go); }
    }

    public void UpdateMap()
    {
        entityMap.Clear();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (goList.Contains(go))
            {
                continue;
            }

            goList.Add(go);
            entityMap[go.GetInstanceID()] = go;
        }
    }
}
