using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    // 게임 시작을 처리하는 메서드
    void GameStart();

    // 게임 종료를 처리하는 메서드
    void GameEnd();
}