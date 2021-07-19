using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLookAround : MonoBehaviour
{
    public float mouseSensivity = 100f;

    public Transform playerBody;
    public playerMovement playerMov;
    public Transform rightArm;

    private float rotationCamera = 0f;
    private float rotationArm = 0f;
    private bool isPistolEquiped = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        rotationCamera -= mouseY;
        rotationCamera = Mathf.Clamp(rotationCamera, -65f, 65f);

        rotationArm += mouseY;
        rotationArm = Mathf.Clamp(rotationArm, -155f, -25f);

        transform.localRotation = Quaternion.Euler(rotationCamera, 0f, 0f);
        
        playerBody.Rotate(Vector3.up * mouseX);


        if (playerMov.pistolEquiped)
        {
            rightArm.localRotation = Quaternion.Euler(rotationArm, -173.54f, 7.853f);
        }
        else
        {
            rightArm.localRotation = Quaternion.Euler(180f, 180f, 18f);
        }
        
        
    }
}
