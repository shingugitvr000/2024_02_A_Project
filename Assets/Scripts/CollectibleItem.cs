using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public ItemType itemType;               //������ ���� (��: ũ����Ż, �Ĺ�, ��Ǯ, ����)
    public string itemName;                 //������ �̸�
    public float respawnTime = 30.0f;       //������ �ð�(�������� �ٽ� ���� �� �� ������ ��� �ð�)
    public bool canCollect = true;          //���� ���� ����(���� �� �� �ִ��� ���θ� ��Ÿ��)

    //�������� �����ϴ� �޼��� , PlayerInventory�� ���� �κ��丮�� �߰� 
    public void CollectItem(PlayerInventory inventory)
    {
        //���� ���� ���θ� üũ 
        if (!canCollect) return;

        inventory.AddItem(itemType);            //�������� �κ��丮�� �߰� 
        Debug.Log($"{itemName} ���� �Ϸ�");     //������ ���� �Ϸ� �޼��� ���
        StartCoroutine(RespawnRoutione());      //������ ������ �ڷ�ƾ ���� 
    }

    //������ �������� ó���ϴ� �ڷ�ƾ
    private IEnumerator RespawnRoutione()
    {
        canCollect = false;                                 //���� �Ұ��� ���·� ����
        GetComponent<MeshRenderer>().enabled = false;       //�������� MeshRenderer�� ���� ������ �ʰ� ��
        GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(respawnTime);       //������ ������ �ð� ��ŭ ��� 

        GetComponent<MeshRenderer>().enabled = true;       //�������� �ٽ� ���̰� ��
        GetComponent<MeshCollider>().enabled = true;
        canCollect = true;                                 //���� �Ұ��� ���·� ����
    }
}
