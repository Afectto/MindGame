using System;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }
    
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;

    private TextMesh[,] debugTexArray;
    private TGridObject[,] _gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject> , int, int, TGridObject> createGridObject = null)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                if (createGridObject != null) 
                    _gridArray[x, z] = createGridObject(this,x,z);
            }
        }

        bool isDebug = false;
        if (isDebug)
        {
            ShowDebug(width, height);
        }

    }

    private void ShowDebug(int width, int height)
    {
        
        debugTexArray = new TextMesh[width, height];
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                // debugTexArray[x, z] = Utils.CreateWorldText(_gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(_cellSize, 0,_cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);

                Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x + 1, z), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
        {
            debugTexArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
        };
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        var pos = new Vector3(x, 0, z) * _cellSize + _originPosition;
        pos.y = 0;
        return pos;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < _width && z < _height)
        {
            _gridArray[x, z] = value;
        }
    }

    public void TriggerGridObjectChange(int x, int z)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{x= x, z = z});
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetXY(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }


    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < _width && z < _height)
        {
            return _gridArray[x, z];
        }

        return default(TGridObject);
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXY(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }
}
