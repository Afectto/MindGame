using System;
using UnityEngine;

namespace ScriptableObject
{
    [Serializable]
    public class LevelData
    { 
        [SerializeField] private int rowCount;
        [SerializeField] private int columnCount;
        [SerializeField] private float cellSize;
    
        public int RowCount => rowCount;
        public int ColumnCount => columnCount;
        public float CellSize => cellSize;
    }
}