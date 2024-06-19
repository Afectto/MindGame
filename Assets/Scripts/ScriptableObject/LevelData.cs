using System;
using UnityEngine;

namespace ScriptableObject
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _rowCount;
        [SerializeField] private int _columnCount;
        [SerializeField] private float _cellSize;
    
        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;
        public float CellSize => _cellSize;
    }
}