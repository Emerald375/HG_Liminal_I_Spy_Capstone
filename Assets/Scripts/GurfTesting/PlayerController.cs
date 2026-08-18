using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    private float cameraPitch = 0f;
    public ChecklistManager checklistManager;
    public float zoomedFOV = 20f;
    public float baseFOV = 60f;

    private Camera fovCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        fovCamera = playerCamera.GetComponent<Camera>();
    }



    private void Update()
    {
        // Mouse looking
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        // Looking at ChecklistItem object

        Ray interactionRay = new Ray(playerCamera.position, playerCamera.forward);

        if(Physics.Raycast(interactionRay, out RaycastHit hit, 15f))
        {
            ChecklistItem item = hit.collider.GetComponent<ChecklistItem>();

            if(item != null && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(item.propName + " found!");

                checklistManager.IsFound(item.propName);
            }
        }


        // Zooming
        if(Input.GetMouseButton(1))
        {
            fovCamera.fieldOfView = zoomedFOV;
        }
        else
        {
            fovCamera.fieldOfView = baseFOV;
        }
        

    }

    

}