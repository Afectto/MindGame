using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform skinTransform; 
    [SerializeField] private float speed;
    private bool _canMove;
    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _canMove = true;
        GameEventManager.Instance.OnChangeCanMove += ChangeCanMove;
    }
    private void ChangeCanMove(bool value)
    {
        _canMove = value;
    }

    void Update()
    {
        if(!_photonView.IsMine || !_canMove) return;
        
        Move();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        transform.Translate(movement * speed * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            skinTransform.rotation = Quaternion.LookRotation(movement);
        }
    }
    
    private void OnDestroy()
    {
        GameEventManager.Instance.OnChangeCanMove -= ChangeCanMove;
    }
}