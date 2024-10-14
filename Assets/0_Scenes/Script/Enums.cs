using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameInit, GameLose, GameReady, GameStart
}

public enum Direction
{
    Up, Down, Left, Right
}
public enum ItemTypePlane
{
    Weapon, Shield, Heal,
}
public enum FlagActionType
{
    WhiteUp = 2, WhiteDown = 1, WhiteNotUp = -2, WhiteNotDown = -1,
    BlueUp = 4, BlueDown = 3, BlueNotUp = -4, BlueNotDown = -3,
    Stay = 0,
}
public enum FlagState
{
    Up = 1, Down = -1, Stay = 0
}

// public enum FlagColor
// {
//     Blue, White
// }