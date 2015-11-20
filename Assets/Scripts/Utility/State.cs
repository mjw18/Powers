using UnityEngine;
using System.Collections;

public abstract class State {

	virtual public void Enter()
    {
        Debug.Log("Base class enter");
    }

    public void Execute()
    {
        Debug.Log("Base class execute");

    }

    public void Exit()
    {
        Debug.Log("Base class exit");
    }

}
