using UnityEngine;
using System.Collections;

public abstract class StateMachine
{
    public State currentState;

    public void ExecuteStateMachine()
    {
        currentState.Execute();
    }

    public void ChangeNext(State nextState)
    {
        currentState.Exit();

        currentState = nextState;

        currentState.Enter();
    }
}
