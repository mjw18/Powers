﻿using UnityEngine;
using ExtendedEvents;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject player;
    private Text killCount;

    private Vector3 m_PlayerSpawn;
    public Vector3 enemySpawn;

    //This s strictly code. Is that okay? Make static class/singleton?
    public EntityHashTable entityTable { get; private set; }

    public float respawnTime = 1f;

    private GameObject m_Player;

    public ObjectPool m_EnemyPool;
    public ObjectPool m_LaserShotPool;
    public Dictionary<GameObject, ObjectPool> objectPoolLookup { get; private set; }
    
    public Regulator globalRegulator { get; private set; }

    private int m_KillCount = 0;

    // Use this for initialization
    void Awake ()
    {
        Debug.Log("Initializing GameManager");

	    if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        globalRegulator = GetComponent<Regulator>();

        objectPoolLookup = new Dictionary<GameObject, ObjectPool>();

        Init();

        InitLevel();
    }

    void OnDestroy()
    {
        Debug.Log("GameManager Destroyed!");
    }

    void Update()
    {
        //Update killCount
        killCount.text = "Kills: " + m_KillCount;

        if (Input.GetKeyDown("q"))
        {
            SpawnEnemies();
        }
        //RespawnPlayer at starting Pos
        if(!m_Player.activeSelf)
        {
            RespawnPlayer();
        }
    }

    //Do Game Initialization. Don't Do level Init
    void Init()
    {
        RegisterListeners();

        //This is awful get rid of it
        killCount = GameObject.Find("KillCountText").GetComponent<Text>();
        if(killCount == null)
        {
            Debug.Log("Kill Count Text object not found. Change this immediately!!!");
        }
        killCount.text = "Kills: " + m_KillCount;

        //Fix this with const string or with a get component in children?
        m_EnemyPool = GameObject.Find("BasicBlobPool").GetComponent<ObjectPool>();
        //Must init manually to ensure that entity hash inits after
        m_EnemyPool.InitPool();
        //Register pool in lookup table for spawner
        //objectPoolLookup.Add(m_EnemyPool.pooledObject, m_EnemyPool);

        m_LaserShotPool = GameObject.Find("LaserShotPool").GetComponent<ObjectPool>();
        m_LaserShotPool.InitPool();
        //objectPoolLookup.Add(m_LaserShotPool.pooledObject, m_LaserShotPool);

        InitPools();
        //Make this its own GameObject? Is that redundant?
        //Dont want to keep on GameManager since GM doesnt destroy on load (This might be 
        //what i want to happen
        entityTable = new EntityHashTable();
        entityTable.Init();
    }

    void InitPools()
    {
        GameObject[] objectPoolArray = GameObject.FindGameObjectsWithTag(Tags.objectPool);
        
        //Add each pool to poollookup with gameobject as key
        foreach(var go in objectPoolArray)
        {
            ObjectPool temp = go.GetComponent<ObjectPool>();
            //Initialize pool
            temp.InitPool();

            //Add pool to dictionary
            objectPoolLookup.Add(temp.pooledObject, temp);
        }
    }

    //Spawn Enemies and Player at predifined spawn points. Use Scriptable object for level config here?
    void InitLevel()
    {
        //Call Init on spawners
        EventManager.PostMessage<InitSpawnersMessage>(new InitSpawnersMessage());

        //Grab spawners manually. If message works, get rid of
        GameObject[] spawners = GameObject.FindGameObjectsWithTag(Tags.spawner);

        foreach (var spawner in spawners)
        {
            //Manual init. This will do until init sequence can be better controlled
            spawner.GetComponent<Spawner>().Init(new InitSpawnersMessage());

            //Spawn object and store reference
            GameObject obj = spawner.GetComponent<Spawner>().Spawn(false);

            //For enemy spawn, change this
            if (spawner.GetComponent<Spawner>().EntityToSpawn.CompareTag(Tags.enemy)) obj.GetComponent<Enemy>().Respawn();

            //Spawn player and store spawn pos
            else if (spawner.GetComponent<Spawner>().EntityToSpawn.CompareTag(Tags.player))
            {
                SpawnPlayer(spawner.transform);
                //Store spawn pos for respawn
                //Store reference to this spawner instead?
                m_PlayerSpawn = spawner.transform.position;
            }
        }
    }

    void RegisterListeners()
    {
        //Create Callbacks for listener actions
        UnityEngine.Events.UnityAction<EnemyDiedMessage> deathAction = OnEnemyDeath;

        //Register listeners
        EventManager.RegisterListener<EnemyDiedMessage>(deathAction);
    }

    //When an enemyDied message is recieved do this
    void OnEnemyDeath(EnemyDiedMessage deathMessage)
    {
        m_KillCount += 1;
    }

    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void RespawnPlayer()
    {
        //Store player spawner instead?
        m_Player.transform.position = m_PlayerSpawn;
        m_Player.SetActive(true);
        EventManager.PostMessage<PlayerRespawnedMessage>(new PlayerRespawnedMessage(Vector3.forward));
    }

    //Instantiate player prefab at player spawn or reset position
    void SpawnPlayer(Transform trans)
    {
        if(m_Player == null)
        {
            m_Player = Instantiate(player, trans.position, Quaternion.identity) as GameObject;
            entityTable.AddToTable(m_Player);
        }
        else
        {
            RespawnPlayer();
        }
    }

    //Pull next inactive enemy from pool
    void SpawnEnemies()
    {
        GameObject temp = m_EnemyPool.NextPooledObject(false);
        if (!temp) return;
        temp.GetComponent<Enemy>().Respawn();
        temp.transform.position = GetSpawnPos();
    }

    Vector3 GetSpawnPos()
    {
        float x = Random.Range(-1f, 1f);
        return new Vector3(x, 1, 0); 
    }

    //Get a reference to an object pool
    public ObjectPool GetObjectPool(GameObject obj)
    {
        ObjectPool temp = null;
        if(!objectPoolLookup.TryGetValue(obj, out temp))
        {
            Debug.Log("No registered pool for this GameObject");
            return null;
        }
        return temp;
    }
}
