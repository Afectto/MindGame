using Photon.Pun;
using UnityEngine;

public class GridItemChecker : MonoBehaviour
{
    [SerializeField]private LevelCreator levelCreator;
    private SerializableArray2D<bool> _task;
    private CreateGrid _myGrid;
    private PhotonView _photonView;
    private Cube[] _allCubs;
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        
        if (PhotonNetwork.IsMasterClient)
        {
            levelCreator.onCreateLevel += SetTask;
            GameEventManager.Instance.onStartNewGame += ClearMyGrid;
        }
        GameEventManager.Instance.onPlaceCube += OnPlaceCube;
        GameEventManager.Instance.onGrabCube += OnGrabCube;
        _myGrid = GetComponent<CreateGrid>();
    }
    
    private void CheckToWin()
    {
        for (int i = 0; i < _myGrid.columnCount; i++)
        {
            for (int j = 0; j < _myGrid.rowCount; j++)
            {
                if (_myGrid.slotItemGrid.GetGridObject(i, j).GetSlotIdentifier() != _task[i, j])
                {
                    return;
                }
            }
        }
        GameEventManager.Instance.TriggerOnTaskComplete();
    }

    #region GrabCube
    private void OnPlaceCube(Vector3 obj)
    {
        GrabCube(obj, true);
        _photonView.RPC("OnGrabCubeRPC", RpcTarget.OthersBuffered, obj, true);
    }

    private void OnGrabCube(Vector3 obj)
    {
        if (!CheckOtherCubeInSlot(obj))
        {
            GrabCube(obj, false);
            _photonView.RPC("OnGrabCubeRPC", RpcTarget.OthersBuffered, obj, false);
        }
    }

    private void GrabCube(Vector3 obj, bool isGrab)
    {
        var slot = _myGrid.slotItemGrid.GetGridObject(obj);
        slot?.GetSlot().ChangIdentifier(isGrab);
        CheckToWin();
    }

    private bool CheckOtherCubeInSlot(Vector3 obj)
    {
        var gridObject = _myGrid.slotItemGrid.GetGridObject(obj);
        var slot = gridObject?.GetSlot();
        if (_allCubs == null )
        {
            _allCubs = FindObjectsOfType<Cube>();
        }
        
        if (slot == null )
        {
            return false;
        }
        foreach (var cube in _allCubs)
        {
            var position = cube.transform.position;
            if (position != obj)
            {    
                var gridObjectByCube = _myGrid.slotItemGrid.GetGridObject(position);
                var slotByCube = gridObjectByCube?.GetSlot();

                if (slotByCube == slot)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
    
    private void SetTask(bool[,] obj)
    {
        _task = new SerializableArray2D<bool>(obj);
        
        _photonView.RPC("UpdateTaskRPC", RpcTarget.OthersBuffered, _task.GetArrayFlat(), _task.ElementSize);
    }

    private void ClearMyGrid()
    {
        ClearGrid();
        _photonView.RPC("ClearMyGridRPC", RpcTarget.OthersBuffered);
    }

    private void ClearGrid()
    {
        for (int i = 0; i < _myGrid.columnCount; i++)
        {
            for (int j = 0; j < _myGrid.rowCount; j++)
            {
                _myGrid.slotItemGrid.GetGridObject(i, j).GetSlot().ChangIdentifier(false);
            }
        }
    }

    #region PunRPC
    [PunRPC]
    private void OnGrabCubeRPC(Vector3 obj, bool isGrab)
    {
        GrabCube(obj, isGrab);
    }
    
    [PunRPC]
    private void UpdateTaskRPC(bool[] arrayFlat, int elementSize)
    {
        var task = new SerializableArray2D<bool>(elementSize, arrayFlat);
        _task = task;
    }

    [PunRPC]
    private void ClearMyGridRPC()
    {
        ClearGrid();
    }
    #endregion

    private void OnDestroy()
    {
        levelCreator.onCreateLevel -= SetTask;
        GameEventManager.Instance.onPlaceCube -= OnPlaceCube;
        GameEventManager.Instance.onGrabCube -= OnGrabCube;
        GameEventManager.Instance.onStartNewGame -= ClearMyGrid;
    }
}
