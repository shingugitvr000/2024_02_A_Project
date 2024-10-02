using UnityEngine;

//GridCell Ŭ������ �� �׸��� ���� ���¿� �����͸� ���� �մϴ�. 
public class GridCell
{
    public Vector3Int Position;                         //���� �׸��� �� ��ġ
    public bool IsOccupied;                             //���� �ǹ��� ������ ����
    public GameObject Building;                         //���� ��ġ�� �ǹ� ��ü

    public GridCell(Vector3Int position)                //Ŭ���� �̸��� ������ �Լ� (������) Ŭ������ �����ɶ� ȣ�� 
    {
        Position = position;
        IsOccupied = false;
        Building = null;
    }
}

public class GridBuildingSystem : MonoBehaviour
{

    [SerializeField] private int width = 10;                        //�׸����� ���� ũ��
    [SerializeField] private int height = 10;                       //�׸����� ���� ũ��
    [SerializeField] private float cellSize = 1.0f;                 //�� ���� ũ�� 
    [SerializeField] private GameObject cellPrefabs;                //�� ������
    [SerializeField] private GameObject builingPrefabs;                //���� ������

    [SerializeField] private PlayerController playerController;     //�÷��̾� ��Ʈ�ѷ� ����
     
    [SerializeField] private Grid grid;
    private GridCell[,] cells;                          //GridCell Ŭ������ 2���� �迭�� ����
    private Camera firstPersonCamera;

        // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera = playerController.firstPersonCamera;         //�÷��̾��� ī�޶� ��ü�� �����´�. 
        CreateGrid();
    }
    //�׸��带 �����ϰ� ���� �ʱ�ȭ�ϴ� �޼���
    private void CreateGrid()
    {
        grid.cellSize = new Vector3(cellSize, cellSize, cellSize);
        
        cells = new GridCell[width, height];
        Vector3 gridCenter = playerController.transform.position;       //�÷��̾� ��ġ�� �޾ƿͼ�
        gridCenter.y = 0;
        transform.position = gridCenter - new Vector3(width * cellSize / 2.0f, 0, height * cellSize / 2.0f);    //�÷��̾� ���߾� �������� �����.

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                Vector3Int cellPosition = new Vector3Int(x, 0, z);  //�� ��ġ 
                Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition);  //�׸��� �Լ��� ���ؼ� ���� ������ ��ġ�� �����´�.
                GameObject cellObject = Instantiate(cellPrefabs, worldPosition, cellPrefabs.transform.rotation);
                cellObject.transform.SetParent(transform);

                cells[x, z] = new GridCell(cellPosition);
            }
        }
    }    
    // Update is called once per frame
    void Update()
    {
        Vector3 lookPosition = GetLookPosition();
        if(lookPosition != Vector3.zero)                                //���� �ִ� ��ǥ�� �ִ��� �˻� 
        {
            Vector3Int gridPosition = grid.WorldToCell(lookPosition);   //�׸��� ���� ������ ��ȯ
            if (isValidGridPosition(gridPosition))                      //��ġ�� ��ȿ ���� Ȯ��
            {
                HighlightCell(gridPosition);

                if (Input.GetMouseButton(0))
                {
                    PlaceBuilding(gridPosition);
                }
                if (Input.GetMouseButton(1))
                {
                    RemoveBuilding(gridPosition);
                }
            }
        }
    }

    //�׸��� ���� �ǹ��� ��ġ�ϴ� �޼���
    private void PlaceBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];  //��ġ ������� cell�� �޾ƿ´�.
        if(!cell.IsOccupied)                                    //�ش� ��ġ�� �ǹ��� �ִ��� Ȯ���Ѵ�. 
        {   
            Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);  //���� ��ġ ��ȯ ��
            GameObject building = Instantiate(builingPrefabs, worldPosition, Quaternion.identity);  //�ǹ��� ���� 
            cell.IsOccupied = true;                                         //�ǹ� Ȯ�� ��
            cell.Building = building;                                       //Cell �� ���� ����
        }
    }

    //�׸��� ������ �Ź��� �����ϴ� �޼��� 
    private void RemoveBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];  //��ġ ������� cell�� �޾ƿ´�.
        if (cell.IsOccupied)                                    //�ش� ��ġ�� �ǹ��� �ִ��� Ȯ���Ѵ�. 
        {
            Destroy(cell.Building);                                     //Cell �ǹ��� �����Ѵ�. 
            cell.IsOccupied = false;                                    //�ǹ� Ȯ�� ��
            cell.Building = null;                                       //Cell �� ���� ���� null ������ ����
        }
    }


    //���õ� ���� ���̶���Ʈ�ϴ� �޼���
    private void HighlightCell(Vector3Int gridPosition)
    {
        for(int x = 0; x < width; x++)              //Cell �� ���鼭
        {
            for(int z = 0; z < height; z++)
            {
                //�ǹ��� ������ �Ͼ������ 
                GameObject cellObject = cells[x, z].Building != null ? cells[x, z].Building : transform.GetChild(x * height + z).gameObject;
                cellObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        //Ư�� ���� �ǹ��� ������ ������ �ƴϸ� �ʷϻ� 
        GridCell cell = cells[gridPosition.x , gridPosition.z ];
        GameObject highlightObject = cell.Building != null ? cell.Building : transform.GetChild(gridPosition.x * height + gridPosition.z).gameObject;
        highlightObject.GetComponent<Renderer>().material.color = cell.IsOccupied ? Color.red : Color.green;
    }

    //�׸��� �������� ��ȿ���� Ȯ���ϴ� �޼���
    private bool isValidGridPosition(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width && 
            gridPosition.z >= 0 && gridPosition.z < height; 
    }


    //�÷��̾ ���� �ִ� ��ġ�� ����ϴ� �޼���
    private Vector3 GetLookPosition()
    {
        if(playerController.isFirstPerson)      //1��Ī ����� ���
        {
            Ray ray = new Ray(firstPersonCamera.transform.position , firstPersonCamera.transform.forward);  //ī�޶� �� �������� ray�� ���.
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);     //Ray ������ �����ش�. 
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white);     //Ray ������ �����ش�. 
            }
        }
        //3��Ī ���
        else 
        {
            Vector3 characterPosition = playerController.transform.position;            //�÷��̾� ��ġ
            Vector3 characterFoward = playerController.transform.forward;               //�÷��̾��� �� ���� 
            Vector3 rayOrigin = characterPosition + Vector3.up * 1.5f + characterFoward * 0.5f; //ĳ���� ����
            Vector3 rayDirection = (characterFoward - Vector3.up).normalized;          //ĳ���� ���� ���� �� �밢��

            Ray ray = new Ray (rayOrigin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.blue);     //Ray ������ �����ش�. 
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white);     //Ray ������ �����ش�. 
            }
        }
        return Vector3.zero;
    }

    //�׸��� ���� GIzmo�� ǥ���ϴ� �޼��� 
    private void OnDrawGizmos()                 //����Ƽ Sceneâ�� ���̴� Debug �׸� 
    {
        Gizmos.color = Color.blue;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 cellCenter = grid.GetCellCenterWorld(new Vector3Int(x, 0, z));
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }


}
