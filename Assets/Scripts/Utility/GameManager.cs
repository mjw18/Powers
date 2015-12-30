using UnityEngine;
using ExtendedEvents;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject player;
    private Text killCount;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;

    //This s strictly code. Is that okay? Make static class/singleton?
    public EntityHashTable entityTable;

    public float respawnTime = 1f;

    private GameObject m_Player;

    public ObjectPool m_EnemyPool;
    public ObjectPool m_LaserShotPool;

    public Regulator globalRegulator;

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

        //Run Level Init (?) from inside GM
        //Mov to readng a LevelConfig?
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

        m_LaserShotPool = GameObject.Find("LaserShotPool").GetComponent<ObjectPool>();
        m_LaserShotPool.InitPool();

        if (m_EnemyPool == null) Debug.Log("Pool not intialized?");

        //Make this its own GameObject? Is that redundant?
        //Dont want to keep on GameManager since GM doesnt destroy on load (This might be 
        //what i want to happen
        entityTable = new EntityHashTable();
        entityTable.Init();
    }

    //Spawn Enemies and Player at predifined spawn points. Use Scriptable object for level config here?
    void InitLevel()
    {
        SpawnPlayer();
        SpawnEnemies();
    }

    void RegisterListeners()
    {
        //Create Callbacks for listener actions
        UnityEngine.Events.UnityAction deathAction = new UnityEngine.Events.UnityAction(OnEnemyDeath);

        //Register listeners
        EventManager.RegisterListener(MessageKey.EnemyDied, deathAction);
    }

    //When an enemyDied message is recieved do this
    void OnEnemyDeath()
    {
        m_KillCount += 1;
    }

    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void RespawnPlayer()
    {
        m_Player.transform.position = playerSpawn;
        m_Player.SetActive(true);
        EventManager.PostMessage(MessageKey.PlayerRespawned);
    }

    //Instantiate player prefab at player spawn or reset position
    void SpawnPlayer()
    {
        if(m_Player == null)
        {
            m_Player = Instantiate(player, playerSpawn, Quaternion.identity) as GameObject;
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
}
