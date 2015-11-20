using UnityEngine;
using System.Collections;

public class PlayerStateMachine : StateMachine {

    public Player player;

    private PlayerDefaultState m_DefaultState;
    private PlayerPowerUpState m_PoweringUpState;

    public void Init()
    {
        //Create States or grab from list in Player
        m_DefaultState = new PlayerDefaultState();
        m_PoweringUpState = new PlayerPowerUpState();
    }

	void ExecuteStateMachine ()
    {
	    
	}

}
