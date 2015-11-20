using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TestMessager : MonoBehaviour {

    private UnityAction testAction;

	void Awake ()
    {
        testAction = new UnityAction(TestMessage);
	}

    void OnEnable()
    {
        if(EventManager.instance == null)
        {
            Debug.Log("No instance of Event Managet");
        }
        EventManager.RegisterListener(EventManager.MessageKey.Test, testAction);
    }

    void TestMessage()
    {
        Debug.Log("The Test Worked!!!");
    }
}
