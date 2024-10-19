using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EatTheLand : MonoBehaviour
{
    public enum EPlayer_State
    {
        DEFAULT,
        EXPLOSION,
        FREEZE
    }
    public EPlayer_State _state;
    // Start is called before the first frame update
    void Start()
    {
        _state = EPlayer_State.DEFAULT;
    }

    
}
