using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerState : MonoBehaviour
{
    protected PlayerControl _playerControl;

    protected APlayerState(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();    
    
}
