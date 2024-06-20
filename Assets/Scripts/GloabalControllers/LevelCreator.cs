using System;
using System.Collections.Generic;    
using Photon.Pun;
using ScriptableObject;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : ZoneBackgroundUpdate
{
    [SerializeField] private CreateGrid zone1;
    [SerializeField] private GameObject cubePrefab;
    
    private bool[,] _table;
    private List<GameObject> _cubsList;
    private PhotonView _photonView;

    public List<GameObject> CubsList => _cubsList;

    public event Action<bool[,]> onCreateLevel;
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        GameEventManager.Instance.onJoinInRoom += JoinInRoom;
    }

    private void JoinInRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _cubsList = new List<GameObject>();
            GameEventManager.Instance.onStartNewGame += CreateLevelTask;
        }
    }

    private void CreateLevelTask()
    {
        ClearAll();
        
        _table = new bool[zone1.columnCount, zone1.rowCount];
        
        UpdateBackground(zone1.cellSize, zone1.columnCount, zone1.rowCount);
        
        for (int i = 0; i < zone1.columnCount; i++)
        {
            for (int j = 0; j < zone1.rowCount; j++)
            {
                var value = Random.Range(0, 500) < 250;
                _table[i, j] = value;
                if (value)
                {
                    _cubsList.Add(PhotonNetwork.Instantiate(cubePrefab.name, GetRandomPositionWithinBackground(), Quaternion.identity));
                }
            }
        }
        zone1.CallRefillGridRPC(_table);
        onCreateLevel?.Invoke(_table);
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
    
    private void ChangeColor()
    {
        var sprite = background.GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }
    
    private void ClearAll()
    {
        foreach (var cube in _cubsList)
        {
            PhotonNetwork.Destroy(cube.gameObject);
        }
        _cubsList.Clear();
    }

    protected override void UpdateBackground(LevelData levelData)
    {
        base.UpdateBackground(levelData);
        ChangeColor();
        
        _photonView.RPC("UpdateBackgroundRPC", RpcTarget.OthersBuffered, levelData);
    }
    
    protected override void UpdateBackground(float cellSize, int columnCount, int rowCount)
    {
        base.UpdateBackground(cellSize, columnCount, rowCount);
        ChangeColor();
        _photonView.RPC("UpdateBackgroundRPC", RpcTarget.OthersBuffered, cellSize, columnCount, rowCount);
    }

    #region PunRPC
    [PunRPC]
    private void UpdateBackgroundRPC(LevelData levelData)
    {
        base.UpdateBackground(levelData);
        ChangeColor();
    }
    
    [PunRPC]
    private void UpdateBackgroundRPC(float cellSize, int columnCount, int rowCount)
    {
        base.UpdateBackground(cellSize, columnCount, rowCount);
        ChangeColor();
    }
    #endregion
    
    private void OnDestroy()
    {
        GameEventManager.Instance.onStartNewGame -= CreateLevelTask;
        GameEventManager.Instance.onJoinInRoom -= JoinInRoom;
    }
}
