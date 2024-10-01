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

        // �θ޶��� ������ ��
        if (!isReturning)
        {
            float moveX = Mathf.Sin(_time) * _range;
            transform.position = new Vector2(startPosition.x + moveX, transform.position.y);

            // �θ޶��� Ư�� �Ÿ� �̻� ���� �� ���ƿ����� ����
            if (moveX >= _range)
            {
                isReturning = true;
                _time = 0;  // ���ƿ��� ��θ� �ʱ�ȭ
            }
        }
        // �θ޶��� ���ƿ� ��
        else
        {
            float moveX = Mathf.Sin(_time) * _range;
            transform.position = new Vector2(startPosition.x - moveX, transform.position.y);

            // �θ޶��� ���� ��ġ�� ���ƿ��� ��
            if (transform.position.x <= startPosition.x)
            {
                isReturning = false;
                _time = 0;
            }
        }
    }
}
