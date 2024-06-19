using ScriptableObject;
using UnityEngine;

namespace Grid
{
    public class CreateGrid : ZoneBackgroundUpdate
    {
        [SerializeField] private Slot SlotPrefab;
        [SerializeField] private LevelBundleData[] LevelDataArray;

        private Grid<SlotGridObject> _slotItemGrid;
        private int _rowCount;
        private int _columnCount;
        private float _cellSize = 1f;
        private int _currentLevel;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;
        public float CellSize => _cellSize;
        public Grid<SlotGridObject> SlotItemGrid => _slotItemGrid;

        private readonly float OFFSET_CELLS = 0.6f;
        private void Awake()
        {
            GenerateLevel(0);
        }

        public void GenerateLevel(int levelNum)
        {
            if (LevelDataArray.Length <= 0) return;
            // if (levelNum >= LevelDataArray.Length)
            // {
            //     onCompleteGame?.Invoke();
            //     return;
            // }

            ClearGrid();
            
            _currentLevel = levelNum;
            
            FillLevelData();
            UpdateBackground(LevelDataArray[_currentLevel].LevelData);

            _slotItemGrid = new Grid<SlotGridObject>(_columnCount, _rowCount, _cellSize,
                transform.position, (grid, x, y) => new SlotGridObject(grid, x, y));
            
            FillingGrid();
        }

        private void FillLevelData()
        {
            var levelData = LevelDataArray[_currentLevel].LevelData;
            _rowCount = levelData.RowCount;
            _columnCount = levelData.ColumnCount;
            _cellSize = levelData.CellSize;
        }


        private void FillingGrid() 
        {
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    var newSlot = CreateNewSlot(i, j);
                    _slotItemGrid.GetGridObject(i, j).SetSlotItem(newSlot);
                }
            }
        }

        public void RefillGrid(bool[,] table)
        {
            if(table.GetLength(0) != _columnCount || table.GetLength(1) != _rowCount)return;
            
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    _slotItemGrid.GetGridObject(i, j).GetSlot().ChangIdentifier(table[i, j]);
                }
            }
        }
        
        private Slot CreateNewSlot(int x, int z)
        {
            var worldPosition = _slotItemGrid.GetWorldPosition(x, z);
            worldPosition.x += OFFSET_CELLS * _cellSize;
            worldPosition.z += OFFSET_CELLS * _cellSize;

            var newSlot = Instantiate(SlotPrefab, worldPosition, Quaternion.identity, transform);
            newSlot.ChangIdentifier(false);
            newSlot.transform.localScale = new Vector3(_cellSize - 0.15f, 0,_cellSize - 0.15f);
        
            return newSlot;
        }

        private void ClearGrid()
        {
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    _slotItemGrid.GetGridObject(i, j).Destroy();
                }
            }
        }
    }
}