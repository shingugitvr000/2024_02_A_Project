using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;          //이동 속도
    public float jumpForce = 5.0f;          //점프 힘
    public float rotationSpeed = 10.0f;     //회전 속도

    //카메라 설정 변수 
    [Header("Camera Settings")]
    public Camera firstPersonCamera;        //1인칭 카메라 
    public Camera thirdPersonCamera;        //3인칭 카메라 

    private float mouseSenesitivity = 200.0f;  //마우스 감도

    public float cameraDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;

    private float currentX = 0.0f;              //수평 회전 각도
    private float currentY = 45.0f;             //수직 회전 각도 

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public float radius = 5.0f;                 //3인칭 카메라와 플레이어 간의 거리
    public float minRadius = 1.0f;              //카메라 최소 거리
    public float maxRadius = 10.0f;             //카메라 최대 거리 

    public float yMinLimit = -90;               //카메라 수직 회전 최소각
    public float yMaxLimit = 90;               //카메라 수직 회전 최대각

    private float theta = 0.0f;                 //카메라의 수평 회전 각도
    private float phi = 0.0f;                   //카메라의 수직 회전 각도
    private float targetVericalRotation = 0;    //목표 수직 회전 각도
    private float verticalRoatationSpeed = 240f;    //수직 회전 각도

    //내부 변수들 
    public bool isFirstPerson = true;      //1인칭 모드 인지 여부 
    //private bool isGrounded;                //플레이어가 땅에 있는지 여부
    private Rigidbody rb;                   //플레이어의 Rigidbody

    public float fallingThreshold = -0.1f;              //떨어지는것으로 간주할 수직 속도 임계값

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                     //등반 가능한 최대 경사 각도 

    void Start()
    {
        rb = GetComponent<Rigidbody>();     //RigidBody 컴포넌트를 가져온다.

        Cursor.lockState = CursorLockMode.Locked;       //마우스 커서를 잠그고 숨긴다.
        SetupCameras();
        SetActiveCamera();
    }

    void Update()
    {       
        HandleRotation();       
        HandleCameraToggle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    //활성화할 카메라를 설정하는 함수
    void SetActiveCamera()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);      //1인칭 카메라 활성화 여부
        thirdPersonCamera.gameObject.SetActive(!isFirstPerson);     //3인칭 카메라 활성화 여부
    }


    //카메라 초기 위치 및 회전을 설정하는 함수 
    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0.0f, 0.6f, 0.0f);      //1인칭 카메라 위치
        firstPersonCamera.transform.localRotation = Quaternion.identity;                //1인칭 카메라 회전 초기화 
    }

    //카메라 및 캐릭터 회전 처리하는 함수
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity * Time.deltaTime;            //마우스 좌우 입력
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity * Time.deltaTime;            //마우스 상하 입력

        if (isFirstPerson)
        {
            //1인칭 카메라 로직은 유지 
            transform.rotation = Quaternion.Euler(0.0f, currentX, 0.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(currentY, 0.0f, 0.0f);
        }
        else
        {
            //3인칭 카메라 로직 수정
            currentX += mouseX;
            currentY -= mouseY;         //마우스 Y축 반전

            //수직 회전 제한
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            //카메라 위치 및 회전 계산
            Vector3 dir = new Vector3(0, 0, -cameraDistance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            thirdPersonCamera.transform.position = transform.position + rotation * dir;
            thirdPersonCamera.transform.LookAt(transform.position);                         //LookAt 바라보는 방향 함수 (캐릭터를 계속 본다)

            //줌 처리 
            cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
        }
    }

    //1인칭과 3인칭 카메라를 전환하는 함수

    void HandleCameraToggle()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;     //카메라 모드 전환
            SetActiveCamera();
        }
    }


    //플레이어 점프를 처리하는 함수
    public void HandleJump()
    {   
        if(isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //위쪽으로 힘을 가해 점프      
        }         
    }

    //플레이어의 이동을 처리하는 함수 
    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");             //좌우 입력 (-1 ~ 1)
        float moveVertical = Input.GetAxis("Vertical");                 //앞뒤 입력 (1 ~ -1)

        Vector3 movement;
        if(!isFirstPerson)//3인칭 모드일때 ,카메라 방향으로 이동 처리 
        {
            Vector3 cameraForward = thirdPersonCamera.transform.forward;    //카메라 앞 방향
            cameraForward.y = 0.0f;
            cameraForward.Normalize();  //방향 벡터 정규화 (0~1) 사이 값으로 만들어준다. 

            Vector3 cameraRight = thirdPersonCamera.transform.right;        //카메라 오른쪽 방향
            cameraRight.y = 0.0f;
            cameraRight.Normalize();

            movement = cameraRight * moveHorizontal + cameraForward * moveVertical;            
        }
        else
        {           
            movement = transform.right * moveHorizontal + transform.forward * moveVertical;          
        }        

        //이동 방향으로 캐릭터 회전
        if(movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    public bool isGrounded()        //땅 체크 확인
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }


    public bool isFalling()         //떨어지는것 확인
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public float GetVerticalVelocity()  //플레이어의 Y축 속도 확인
    {
        return rb.velocity.y;
    }

}
