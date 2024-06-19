using System;
using System.Collections.Generic;
using Grid;
using ScriptableObject;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : ZoneBackgroundUpdate
{
    [SerializeField] private CreateGrid zone1;
    [SerializeField] private GameObject cubePrefab;
    private bool[,] table;

    public event Action<bool[,]> onCreatelevel;
    private List<GameObject> _cubsList;

    private void Start()
    {
        _cubsList = new List<GameObject>();
        CreateLevelTask();
        GameEventManager.Instance.onStartNewGame += CreateLevelTask;
    }

    private void CreateLevelTask()
    {
        ClearAll();
        
        table = new bool[zone1.ColumnCount, zone1.RowCount];
        
        UpdateBackground(zone1.CellSize, zone1.ColumnCount, zone1.RowCount);
        
        for (int i = 0; i < zone1.ColumnCount; i++)
        {
            for (int j = 0; j < zone1.RowCount; j++)
            {
                var value = Random.Range(0, 500) < 250;
                table[i, j] = value;
                if (value)
                {
                    _cubsList.Add(Instantiate(cubePrefab, GetRandomPositionWithinBackground(), Quaternion.identity));
                }
            }
        }
        zone1.RefillGrid(table);
        onCreatelevel?.Invoke(table);
    }

    Vector3 GetRandomPositionWithinBackground()
    {
        Vector3 backgroundSize = background.localScale;

        var position = background.position;
        float minX = position.x - backgroundSize.x / 2f;
        float maxX = position.x + backgroundSize.x / 2f;
        float minZ = position.z - backgroundSize.y / 2f;
        float maxZ = position.z + backgroundSize.y / 2f;

        float randomX = Random.Range(minX, maxX) +  backgroundSize.x / 2f;
        float randomZ = Random.Range(minZ, maxZ) + backgroundSize.y / 2f;
        
        return new Vector3(randomX, 0.75f, randomZ);
    }

    protected override void UpdateBackground(LevelData levelData)
    {
        base.UpdateBackground(levelData);
        ChangeColor();
    }

    protected override void UpdateBackground(float cellSize, int columnCount, int rowCount)
    {
        base.UpdateBackground(cellSize, columnCount, rowCount);
        ChangeColor();
    }

    private void ChangeColor()
    {
        var sprite = background.GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }
    
    
    private void ClearAll()
    {
        foreach (var cube in _cubsList)
        {
            Destroy(cube.gameObject);
        }
        _cubsList.Clear();
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.onStartNewGame -= CreateLevelTask;
    }
}
