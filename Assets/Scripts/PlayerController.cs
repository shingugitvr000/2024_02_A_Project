using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾��� ������ �ӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;          //�̵� �ӵ�
    public float jumpForce = 5.0f;          //���� ��

    //���� ������ 
    private bool isGrounded;                //�÷��̾ ���� �ִ��� ����
    private Rigidbody rb;                   //�÷��̾��� Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>();     //RigidBody ������Ʈ�� �����´�.
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    //�÷��̾� ������ ó���ϴ� �Լ�
    void HandleJump()
    {
        //���� ��ư�� ������ ���� ������ 
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //�������� ���� ���� ����
            isGrounded = false;
        }
    }

    //�÷��̾��� �̵��� ó���ϴ� �Լ� 
    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");             //�¿� �Է� (-1 ~ 1)
        float moveVertical = Input.GetAxis("Vertical");                 //�յ� �Է� (1 ~ -1)

        //ĳ���� �������� �̵�
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);       //���� ��� �̵� 
    }

    //�÷��̾ ���� ��� �ִ��� ���� 
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;              //�浹 ���̸� �÷��̾�� ���� �ִ�. 
    }

}
