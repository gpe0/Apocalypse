using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLookAround : MonoBehaviour
{
    public float mouseSensivity = 100f;

    public Transform playerBody;
    public playerMovement playerMov;
    public Transform rightArm;
    public Transform head;


    private float rotation = 0f;
    private bool isPistolEquiped = false;
    private Vector3 headStart;
    private Vector3 headFinish;
    private Vector3 rightArmStart;
    private Vector3 rightArmFinish;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        headStart = head.position - playerBody.position;
        headFinish = new Vector3(head.position.x - playerBody.position.x, head.position.y - playerBody.position.y - 0.35f, head.position.z - playerBody.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -65f, 65f);

        head.localRotation = Quaternion.Euler(rotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        if (playerMov.pistolEquiped)
        {
            rightArm.localRotation = Quaternion.Euler(-rotation - 85f, -173.54f, 7.853f);
        }
        if (playerMov.isCrouched)
        {
            if (Mathf.Abs((head.position.y - playerBody.position.y) - headFinish.y) > 0.01f)
            {
                head.position = new Vector3(head.position.x, head.position.y - 0.05f, head.position.z);
                rightArm.position = new Vector3(rightArm.position.x, rightArm.position.y - 0.05f, rightArm.position.z);
            }
            
        }
        else
        {
            if (Mathf.Abs((head.position.y - playerBody.position.y) - headStart.y) > 0.01f)
            {
                head.position = new Vector3(head.position.x, head.position.y + 0.05f, head.position.z);
                rightArm.position = new Vector3(rightArm.position.x, rightArm.position.y + 0.05f, rightArm.position.z);
            }
        }
    }
}
