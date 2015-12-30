using UnityEngine;
using ExtendedEvents;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameManager gameManager;
    public EventManager eventManager;

    void Awake()
    {
        if (EventManager.instance == null)
        {
            EventManager.instance = Instantiate(eventManager);
            if(EventManager.instance == null)
            {
                Debug.Log("EventManager Initialization failed");
            }
        }
        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameManager);
            if (GameManager.instance == null)
            {
                Debug.Log("GameManager Initialization failed");
            }
        }
        
    }
}
