using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform skinTransform; 
    [SerializeField] private float speed;
    private GameObject grabbedCube;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grabbedCube == null)
            {
                GrabCube();
            }
            else
            {
                ReleaseCube();
            }
        }

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
    
    void GrabCube()
    {
        if (Physics.Raycast(transform.position, skinTransform.forward, out var hit, 2f))
        {
            if (hit.collider.CompareTag("Cube"))
            {
                grabbedCube = hit.collider.gameObject;
                grabbedCube.transform.SetParent(transform);
                grabbedCube.GetComponent<Rigidbody>().isKinematic = true;
                
                GameEventManager.Instance.TriggerGrabCube(grabbedCube.transform.position);
            }
        }
    }

    void ReleaseCube()
    {
        grabbedCube.transform.SetParent(null);
        
        GameEventManager.Instance.TriggerPlaceCube(grabbedCube.transform.position);
        
        grabbedCube = null;
    }
}