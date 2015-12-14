using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject player;
    public GameObject enemy;
    private Text killCount;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;

    //Move to enemy class as static list
    public List<Enemy> enemies;

    public float respawnTime = 1f;

    private GameObject m_Player;

    public ObjectPool laserShotPool;

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

        enemies = new List<Enemy>();
        if (!laserShotPool) laserShotPool = GetComponent<ObjectPool>();

        globalRegulator = GetComponent<Regulator>();

        Init();
	}

    /*void OnLevelWasLoaded()
    {
        Debug.Log("LoadedLevel");
        Init();
        laserShotPool.InitPool();
    }*/

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
            //Invoke("RespawnPlayer", 1f);
        }
        if (Input.GetKeyDown("r"))
        {
            Restart();
        }
    }

    //Spawn Enemies and Player at predifined spawn points. Use Scriptable object for level config here?
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

        enemies.Clear();
        SpawnPlayer();
        SpawnEnemies();
    }

    void RegisterListeners()
    {
        //Create Callbacks for listener actions
        UnityEngine.Events.UnityAction deathAction = new UnityEngine.Events.UnityAction(OnEnemyDeath);

        //Register listeners
        EventManager.RegisterListener(EventManager.MessageKey.EnemyDied, deathAction);
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
        EventManager.PostMessage(EventManager.MessageKey.PlayerRespawned);
    }

    void SpawnPlayer()
    {
        if(m_Player == null)
        {
            m_Player = Instantiate(player, playerSpawn, Quaternion.identity) as GameObject;
        }
        else
        {
            RespawnPlayer();
        }
    }

    void SpawnEnemies()
    {
        Enemy temp = Instantiate(enemy, GetSpawnPos(), Quaternion.identity) as Enemy;
        enemies.Add(temp);
        Debug.Log("EnemyCount: " + enemies.Count);
    }

    Vector3 GetSpawnPos()
    {
        float x = Random.Range(-1f, 1f);
        return new Vector3(x, 1, 0); 
    }
}
