using System;
using Grid;
using UnityEngine;

public class GridItemChecker : MonoBehaviour
{
    [SerializeField]private LevelCreator levelCreator;
    private bool[,] _task;
    private CreateGrid _myGrid;

    private void Start()
    {
        levelCreator.onCreatelevel += SetTask;
        GameEventManager.Instance.onPlaceCube += OnPlaceCube;
        GameEventManager.Instance.onGrabCube += OnGrabCube;
        GameEventManager.Instance.onStartNewGame += ClearMyGrid;
        
        _myGrid = GetComponent<CreateGrid>();
    }

    private void OnGrabCube(Vector3 obj)
    {
        var slot = _myGrid.SlotItemGrid.GetGridObject(obj);
        slot?.GetSlot().ChangIdentifier(false);
        CheckToWin();
    }

    private void OnPlaceCube(Vector3 obj)
    {
        var slot = _myGrid.SlotItemGrid.GetGridObject(obj);
        slot.GetSlot().ChangIdentifier(true);
        CheckToWin();
    }

    private void CheckToWin()
    {
        for (int i = 0; i < _myGrid.ColumnCount; i++)
        {
            for (int j = 0; j < _myGrid.RowCount; j++)
            {
                if (_myGrid.SlotItemGrid.GetGridObject(i, j).GetSlotIdentifier() != _task[i, j])
                {
                    return;
                }
            }
        }
        GameEventManager.Instance.TriggerOnTaskComplete();
    }

    private void SetTask(bool[,] obj)
    {
        _task = obj;
    }

    private void ClearMyGrid()
    {
        for (int i = 0; i < _myGrid.ColumnCount; i++)
        {
            for (int j = 0; j < _myGrid.RowCount; j++)
            {
                _myGrid.SlotItemGrid.GetGridObject(i, j).GetSlot().ChangIdentifier(false);
            }
        }
    }
    
    private void OnDestroy()
    {
        levelCreator.onCreatelevel -= SetTask;
        GameEventManager.Instance.onPlaceCube -= OnPlaceCube;
        GameEventManager.Instance.onGrabCube -= OnGrabCube;
        GameEventManager.Instance.onStartNewGame -= ClearMyGrid;
    }
}
