using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;                         //아이템 감지 범위
    public Vector3 lastPostion;                             //플레이어의 마지막 위치
    public float moveThreshold = 0.1f;                      //이동 감지 임계값
    public ConstructibleBuilding currentNearbyBuilding;

    // Start is called before the first frame update
    void Start()
    {
        lastPostion = transform.position;                   //시작 시 현재 위치를 마지막 위치로 설정
        CheckForBuilding();                                    //초기 건물 체크 수행 
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastPostion, transform.position) > moveThreshold)   //플레이어가 일정 거리 이상 이동했는지 체크
        {
            CheckForBuilding();                                                    //이동시 건물 체크
            lastPostion = transform.position;                                   //현재 위치를 마지막 위치로 업데이트 
        }

        //가까운 아이템이 있고 E키를 눌렀을 때 아이템 수집
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            currentNearbyBuilding.StartConstruction(GetComponent<PlayerInventory>());         //PlayerInventroy 참조하여 건물 건설
        }
    }

    private void CheckForBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);       //감지 범위 내의 모든 콜라이더를 찾아옴

        float closestDistance = float.MaxValue;                             //가장 가까운 거리의 초기값
        ConstructibleBuilding closestBuilding = null;                                 //가장 가까운 건물 초기값

        foreach (Collider collider in hitColliders)                         //각 콜라이더를 검사하여 수집 가능한 건물 찾음
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();            //건물 감지
            if (building != null && building.canBuild && !building.isConstructed)        //건물 있고 수집 가능한지 확인
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);     //거리 계산
                if (distance < closestDistance)                                                     //더 가까운 건물 발견 시 업데이트 
                {
                    closestDistance = distance;
                    closestBuilding = building;
                }
            }
        }
        if (closestBuilding != currentNearbyBuilding)        //가장 가까운 건물이 변경되었을 때 메세지 표시
        {
            currentNearbyBuilding = closestBuilding;        //가장 가까운 건물 업데이트
            if (currentNearbyBuilding != null)
            {
                if (FloatingTextManager.instance != null)
                {
                    Vector3 textPostion = transform.position + Vector3.up * 0.5f;                   //아이템 위치보다 약간 위에 텍스트 생성
                    FloatingTextManager.instance.Show(
                        $"[F] 키로 {currentNearbyBuilding.buildingName} 건설 (나무 {currentNearbyBuilding.requiredTree} 개 필요)"
                        , currentNearbyBuilding.transform.position + Vector3.up
                        );
                }

            }
        }
    }
}
