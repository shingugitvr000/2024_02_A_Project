using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState   //��� �÷��̾� ������ �⺻�� �Ǵ� �߻� Ŭ����
{
    protected PlayerStateMachine stateMachine;          //���� �ӽſ� ���� ���� 
    protected PlayerController playerController;        //�÷��̾� ��Ʈ�ѷ��� ���� ����
    protected PlayerAnimationManager animationManager;

    public PlayerState(PlayerStateMachine stateMachine) //���� �ӽŰ� �÷��̾� ��Ʈ�ѷ� ���� �ʱ�ȭ 
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.playerController;
        this.animationManager = stateMachine.GetComponent<PlayerAnimationManager>();
    }

    //���� �޼��� �� : ���� Ŭ�������� �ʿ信 ���� �������̵� 
    public virtual void Enter() { } //���� ���� �� ȣ��
    public virtual void Exit() { }  //���� ���� �� ȣ��
    public virtual void Update() { }    //�� ������ ȣ��
    public virtual void FixedUpdate() { }   //���� �ð� �������� ȣ�� (���� �����)

    //���� ��ȯ ������ üũ�ϴ� �޼��� 
    protected void CheckTransitions()
    {
        if (playerController.isGrounded())   //���� ���� ���� ��ȯ ����
        {
            if (Input.GetKeyDown(KeyCode.Space))    //���� Ű�� ������ �� 
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)    //�̵� Ű�� ������ ��
            {
                stateMachine.TransitionToState(new MoveingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new IdleState(stateMachine));        //�ƹ� Ű�� ������ �ʾ��� ��
            }
        }
        // ���߿� �������� ���� ��ȯ ���� 
        else
        {
            if (playerController.GetVerticalVelocity() > 0)                         //Y�� �̵� �ӵ� ���� ��� �� �� ������
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else
            {                                                                       //Y�� �̵� �ӵ� ���� ���� �� �� ������
                stateMachine.TransitionToState(new FallingState(stateMachine));
            }           
        }     
    }
}

//IdleState : �÷��̾ ������ �ִ� ���� 
public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();                 //�� ������ ���� ���� ��ȯ ���� üũ 
    }
}

//MoveingState : �÷��̾ �̵� ���� ���� 
public class MoveingState : PlayerState
{
    private bool isRunning;
    public MoveingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        //�޸��� �Է� Ȯ��
        isRunning = Input.GetKey(KeyCode.LeftShift);

        CheckTransitions();                 //�� ������ ���� ���� ��ȯ ���� üũ 
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //���� ��� �̵� ó�� 
    }
}

//JumpingState : �÷��̾ ���� ���� ��
public class JumpingState : PlayerState
{
    public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();                 //�� ������ ���� ���� ��ȯ ���� üũ 
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //���� ��� �̵� ó�� 
    }
}

//FallingState : �÷��̾ ������ �� 
public class FallingState : PlayerState
{
    public FallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();                 //�� ������ ���� ���� ��ȯ ���� üũ 
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //���� ��� �̵� ó�� 
    }
}
