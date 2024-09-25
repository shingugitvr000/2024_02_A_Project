using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;          //이동 속도
    public float jumpForce = 5.0f;          //점프 힘

    //내부 변수들 
    private bool isGrounded;                //플레이어가 땅에 있는지 여부
    private Rigidbody rb;                   //플레이어의 Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>();     //RigidBody 컴포넌트를 가져온다.
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    //플레이어 점프를 처리하는 함수
    void HandleJump()
    {
        //점프 버튼을 누르고 땅에 있을때 
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //위쪽으로 힘을 가해 점프
            isGrounded = false;
        }
    }

    //플레이어의 이동을 처리하는 함수 
    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");             //좌우 입력 (-1 ~ 1)
        float moveVertical = Input.GetAxis("Vertical");                 //앞뒤 입력 (1 ~ -1)

        //캐릭터 기준으로 이동
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);       //물리 기반 이동 
    }

    //플레이어가 땅에 닿아 있는지 감지 
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;              //충돌 중이면 플레이어는 땅에 있다. 
    }

}
