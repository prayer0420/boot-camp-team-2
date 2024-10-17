using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigidbody;
    public GameObject Paddle;
    private bool moving = false;

    // 임시로 패널 불러서 종료하기 위함
    public event Action OnTouchBottom;
    private void Awake()
    {
        GameManager.Instance.SetBallMovement(this);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Paddle = GameObject.Find("Paddle");
    }

    private void Update()
    {
        if ((moving is false) && (Input.GetKeyDown(KeyCode.Space)))
        {
            moving = true;
            Launch();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("DownWall"))
        {
            TouchBottom();
        }
        
        if (collision.collider.gameObject.CompareTag("Brick"))
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();
            brick.Hit();

            if(brick.type.Equals(BrickType.Flow))
            {
                Vector2 dir = ((Vector2)transform.position - collision.GetContact(0).point).normalized;
                rigidbody.AddForce(dir * 3f, ForceMode2D.Impulse);
            }
        }
    }

    private void Launch()
    {
        float x = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

        rigidbody.velocity = new Vector2(x * speed, 1.0f * speed);
    }

    public void Reset()
    {
        rigidbody.velocity = Vector2.zero;
        moving = false;
        Vector3 ResetPosition = Paddle.transform.position;
        transform.position = new Vector2(ResetPosition.x, ResetPosition.y + 0.175f);
    }

    private void TouchBottom()
    {
        OnTouchBottom?.Invoke();
        // 라이프 생기면 남은 라이프에 따라 리셋 정도만 시켜주기
        Reset();
    }
}
