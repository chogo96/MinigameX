using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EPlayer_State
{
    DEFAULT,
    EXPLOSION,
    FREEZE
}
public class Player_EatTheLand : MonoBehaviour
{



    public EPlayer_State _state;
    // Start is called before the first frame update
    void Start()
    {
        _state = EPlayer_State.DEFAULT;
    }

    private void Update()
    {
        switch (_state)
        {
            case EPlayer_State.DEFAULT:

                break;
            case EPlayer_State.EXPLOSION:
                break;
            case EPlayer_State.FREEZE:
                break;
        }
    }


}
