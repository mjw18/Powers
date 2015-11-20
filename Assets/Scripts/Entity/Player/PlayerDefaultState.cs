using UnityEngine;
using System.Collections;

public class PlayerDefaultState : State {

    override public void Enter()
    {
        Debug.Log("Entered Default State");
        base.Enter();
    }

    override public void Execute()
    {

    }

    public void Exit()
    {

    }

}
