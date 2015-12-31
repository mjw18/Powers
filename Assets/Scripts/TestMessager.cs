using UnityEngine;
using ExtendedEvents;
using UnityEngine.Events;
using System.Collections;

public class TestMessager : MonoBehaviour {

	void Start ()
    {
        UnityAction<System.DBNull> testAction = TestMessage;
        if (EventManager.instance == null)
        {
            Debug.Log("No instance of Event Managet");
        }

        Debug.Log("Registering...");
        //EventManager.RegisterListener(MessageKey.Test, testAction);
    }

    void OnEnable()
    {
        
    }

    void TestMessage(System.DBNull t)
    {
        Debug.Log(string.Format("It worked (maybe). Here's the param: {0}" , 9));
    }
}
