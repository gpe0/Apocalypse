using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public float movementSpeed = 300f;
    public GameObject pistol;
    public Animator anim;
    public bool pistolEquiped = false;


    private Rigidbody rb;
    private bool groundCheck = false;
    private bool isRunning = false;
    private bool jumped = false;
    private float runMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pistol.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movementHorizontal = Input.GetAxis("Horizontal");
        float movementVertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && groundCheck)
        {
            isRunning = true;
            runMultiplier = 2f;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            runMultiplier = 1f;
        }


        Vector3 movement = transform.forward * movementVertical * runMultiplier + transform.right * movementHorizontal * runMultiplier;

        movement = movement.normalized * movementSpeed * Time.deltaTime * runMultiplier;

        movement = movement + new Vector3(0f, rb.velocity.y, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && groundCheck)
        {
            jumped = true;
            movement += new Vector3(0f, movementSpeed * Time.deltaTime, 0f);
            groundCheck = false;
        }

        if (groundCheck && jumped)
        {
            jumped = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (pistolEquiped)
            {
                pistolEquiped = false;
                pistol.SetActive(false);
            }
            else
            {
                pistolEquiped = true;
                pistol.SetActive(true);
            }
        }

        if (jumped)
        {
            anim.Play("Jump");
        }
        else if (movementHorizontal != 0f || movementVertical != 0f)
        {
            anim.Play("Walk");
        }
        else if (pistolEquiped)
        {
            anim.Play("IdlePistol");
        }
        else
        {
            anim.Play("Idle");
        }
        rb.velocity = movement;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Floor")
        {
            groundCheck = true;
        }
    }
}
