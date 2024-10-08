using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ü Ŭ���� : ������
public class Bicycle : Vehicle
{
    // Move �޼��� ������
    public override void Move()
    {
        base.Move();    //�⺻ �̵� 
        //������ ���� �߰� ����
        transform.Rotate(0, 10 * Time.deltaTime, 0);
    }

    public override void Horn()
    {
        Debug.Log("������ ���� : ���� ");
    }
}
