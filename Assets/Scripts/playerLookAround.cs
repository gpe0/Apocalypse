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
    public Animator armAnim;

    private float rotation = 0f;

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
        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -65f, 65f);

        head.localRotation = Quaternion.Euler(rotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
        
        if (playerMov.pistolEquiped && !playerMov.reload)
        {
            rightArm.localRotation = Quaternion.Euler(-rotation - 85f, -173.54f, 7.853f);
        }
    }
}
