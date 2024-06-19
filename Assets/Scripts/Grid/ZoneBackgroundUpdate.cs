using ScriptableObject;
using UnityEngine;

public abstract class ZoneBackgroundUpdate : MonoBehaviour
{
    [SerializeField] protected Transform background;

    protected virtual void UpdateBackground(LevelData levelData)
    {
        var cellSize = levelData.CellSize;
        var scale = background.localScale * cellSize;
        scale.x = levelData.ColumnCount * cellSize + 0.1f * cellSize;
        scale.y = levelData.RowCount * cellSize + 0.1f * cellSize;
        background.localScale = scale;
    }

    protected virtual void UpdateBackground(float cellSize, int columnCount, int rowCount)
    {
        var scale = background.localScale * cellSize;
        scale.x = columnCount * cellSize + 0.1f * cellSize;
        scale.y = rowCount * cellSize + 0.1f * cellSize;
        background.localScale = scale;
    }
}