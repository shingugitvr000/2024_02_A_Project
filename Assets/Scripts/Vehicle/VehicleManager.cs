using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public Vehicle[] vehicles;                  //Ż�� ��ü �迭 ���� �Ѵ�. 

    public Car car;                             //�ڵ��� ����
    public Bicycle bicycle;                     //������ ����
        
    float Timer;                                //������ �ð� float ���� ����
    
    void Update()
    {
        for (int i = 0; i < vehicles.Length; i++)       //�迭�� �ִ� Ż�͵��� �����δ�. 
        {
            vehicles[i].Move();
        }

        Timer -= Time.deltaTime;                //�ð��� ���δ�.

        if(Timer < 0)                           //1�ʸ��� ȣ�� �ǰ� �Ѵ�. 
        {
            for (int i = 0; i < vehicles.Length; i++)   
            {
                vehicles[i].Horn();
            }
            Timer = 1;
        }
    }
}
