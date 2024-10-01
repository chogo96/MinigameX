using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private bool _fired = false;
    private Vector2 startPosition;
    private float _time;
    private float _speed;
    public float _range = 5f;
    private bool isReturning = false;
    private void Update()
    {
        _time += Time.deltaTime * _speed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_fired == false)
            {
                _fired = true;
                startPosition = transform.position;
            }
        }

        // 부메랑이 던져질 때
        if (!isReturning)
        {
            float moveX = Mathf.Sin(_time) * _range;
            transform.position = new Vector2(startPosition.x + moveX, transform.position.y);

            // 부메랑이 특정 거리 이상 갔을 때 돌아오도록 설정
            if (moveX >= _range)
            {
                isReturning = true;
                _time = 0;  // 돌아오는 경로를 초기화
            }
        }
        // 부메랑이 돌아올 때
        else
        {
            float moveX = Mathf.Sin(_time) * _range;
            transform.position = new Vector2(startPosition.x - moveX, transform.position.y);

            // 부메랑이 시작 위치로 돌아왔을 때
            if (transform.position.x <= startPosition.x)
            {
                isReturning = false;
                _time = 0;
            }
        }
    }
}
