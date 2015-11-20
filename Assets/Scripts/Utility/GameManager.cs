using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject testMessager;
    public GameObject player;
    public GameObject enemy;
    private Text killCount;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;

    public List<Enemy> enemies;

    private GameObject m_Player;

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
        Init();
	}

    void OnLevelWasLoaded()
    {
        Debug.Log("LoadLevel");
        Init();
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
        if(!m_Player.activeSelf)
        {
            RespawnPlayer();
        }
        if(Input.GetKeyDown("r"))
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
        Instantiate(testMessager);
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

    void OnEnemyDeath()
    {
        m_KillCount += 1;
        Debug.Log("Kills: " + m_KillCount);
    }

    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void RespawnPlayer()
    {
        m_Player.transform.position = playerSpawn;
        m_Player.SetActive(true);
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
