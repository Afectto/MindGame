using Photon.Pun;
using UnityEngine;

public class CubeGrabber : MonoBehaviour
{
    [SerializeField] private Transform skinTransform; 
    
    private GameObject _grabbedCube;
    private Vector3 _initialOffset;
    private bool _isGrabbed;
    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }
    
    void Update()
    {
        if (_isGrabbed && _grabbedCube != null)
        {
            _grabbedCube.transform.position = transform.TransformPoint(_initialOffset);
            _grabbedCube.transform.rotation = transform.rotation;
        }
        if(!_photonView.IsMine) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grabbedCube == null)
            {
                GrabCube();
            }
            else
            {
                ReleaseCube();
            }
        }
    }

    void GrabCube() {
        if (_grabbedCube != null) return;

        if (Physics.Raycast(transform.position, skinTransform.forward, out var hit, 2f)) {
            if (hit.collider.CompareTag("Cube")) {
                var grabbedCube = hit.collider.gameObject;
                var cube = grabbedCube.GetComponent<Cube>();
                    
                if(cube != null && cube.IsTaken) return;

                var grabbedCubePosition = grabbedCube.transform.position;
                _grabbedCube = grabbedCube;
                GameEventManager.Instance.TriggerOnGrabCube(grabbedCubePosition);
                
                _initialOffset = transform.InverseTransformPoint(grabbedCubePosition);
                _isGrabbed = true;
                
                cube.SetTaken(true);
                
                _photonView.RPC("SetIsGrab", RpcTarget.AllBuffered, _grabbedCube.GetPhotonView().ViewID, _isGrabbed, _initialOffset); 
            }
        }
    }

    void ReleaseCube() {
        if (_grabbedCube == null) return;

        GameEventManager.Instance.TriggerOnPlaceCube(_grabbedCube.transform.position);
        var cube = _grabbedCube.GetComponent<Cube>();
        cube.SetTaken(false);
        
        _photonView.RPC("ReleaseIsGrab", RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    void SetIsGrab(int cubeId, bool isGrabbed, Vector3 initialOffset)
    {
        PhotonView cubeView = PhotonView.Find(cubeId);
        _grabbedCube = cubeView.gameObject;
        _isGrabbed = isGrabbed;
        _initialOffset = initialOffset;
    }

    [PunRPC]
    void ReleaseIsGrab()
    {
        _grabbedCube.transform.position = transform.TransformPoint(_initialOffset);
        _grabbedCube.transform.rotation = transform.rotation;
        
        _grabbedCube = null;
        _isGrabbed = false;
    }
}