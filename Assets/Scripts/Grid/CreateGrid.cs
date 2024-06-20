using Photon.Pun;
using ScriptableObject;
using UnityEngine;

public class CreateGrid : ZoneBackgroundUpdate
{
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private LevelBundleData[] levelDataArray;
    
    private readonly float OFFSET_CELLS = 0.6f;

    private int _currentLevel;
    private PhotonView _photonView;

    public int rowCount { get; private set; }
    public int columnCount { get; private set; }
    public float cellSize { get; private set; } = 1f;
    public Grid<SlotGridObject> slotItemGrid { get; private set; }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        
        GenerateLevel(0);
    }

    private void GenerateLevel(int levelNum)
    {
        if (levelDataArray.Length <= 0) return;
        ClearGrid();
        
        _currentLevel = levelNum;
        
        FillLevelData();
        UpdateBackground(levelDataArray[_currentLevel].LevelData);

        slotItemGrid = new Grid<SlotGridObject>(columnCount, rowCount, cellSize,
            transform.position, (grid, x, y) => new SlotGridObject(grid, x, y));
        
        FillingGrid();
    }

    private void FillLevelData()
    {
        var levelData = levelDataArray[_currentLevel].LevelData;
        rowCount = levelData.RowCount;
        columnCount = levelData.ColumnCount;
        cellSize = levelData.CellSize;
    }
    
    private void FillingGrid() 
    {
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                var newSlot = CreateNewSlot(i, j);
                slotItemGrid.GetGridObject(i, j).SetSlotItem(newSlot);
            }
        }
    }

    private Slot CreateNewSlot(int x, int z)
    {
        var worldPosition = slotItemGrid.GetWorldPosition(x, z);
        worldPosition.x += OFFSET_CELLS * cellSize;
        worldPosition.z += OFFSET_CELLS * cellSize;

        var newSlot = Instantiate(slotPrefab, worldPosition, Quaternion.identity, transform);
        newSlot.ChangIdentifier(false);
        newSlot.transform.localScale = new Vector3(cellSize - 0.15f, 0,cellSize - 0.15f);
    
        return newSlot;
    }

    private void FillGrid(SerializableArray2D<bool> table)
    {
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                slotItemGrid.GetGridObject(i, j).GetSlot().ChangIdentifier(table[i, j]);
            }
        }
    }
    
    private void ClearGrid()
    {
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                slotItemGrid.GetGridObject(i, j).Destroy();
            }
        }
    }
            
    public void CallRefillGridRPC(bool[,] table)
    {
        var serializableArray = new SerializableArray2D<bool>(table);
        _photonView.RPC("RefillGridRPC", RpcTarget.OthersBuffered, serializableArray.GetArrayFlat(), serializableArray.ElementSize);
        FillGrid(serializableArray);
    }
        
    [PunRPC]
    public void RefillGridRPC(bool[] arrayFlat, int elementSize)
    {
        var table = new SerializableArray2D<bool>(elementSize, arrayFlat);
        FillGrid(table);
    }

}
