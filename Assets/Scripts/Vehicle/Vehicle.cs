using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�߻� Ŭ���� : Ż�� 
public abstract class Vehicle : MonoBehaviour
{
    public float speed = 10.0f;                //�̵� �ӵ� ����

    //���� �޼��� : �̵� 
    public virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);      //�������� �̵� 
    }

    //�߻� �޼��� : ����
    public abstract void Horn();
}
