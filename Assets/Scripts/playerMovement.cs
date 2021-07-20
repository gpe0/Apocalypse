using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public float movementSpeed = 300f;
    public GameObject pistol;
    public Animator playerAnim;
    public Animator rightArmAnim;
    public bool pistolEquiped = false;


    private Rigidbody rb;
    private bool groundCheck = false;
    private bool isRunning = false;
    private bool jumped = false;
    private float runMultiplier = 1f;
    private bool reseted = false;
    private bool spaced = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pistol.SetActive(false);
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space) && groundCheck)
        {
            jumped = true;
            groundCheck = false;
            spaced = true;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && (!jumped))
        {
            if (pistolEquiped)
            {
                pistolEquiped = false;
                pistol.SetActive(false);
                rightArmAnim.enabled = true;

                reseted = true;
            }
            else
            {
                pistolEquiped = true;
                pistol.SetActive(true);
                rightArmAnim.enabled = false;
            }
        }

    }
    void FixedUpdate()
    {
        float movementHorizontal = Input.GetAxis("Horizontal");
        float movementVertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * movementVertical * runMultiplier + transform.right * movementHorizontal * runMultiplier;

        movement = movement.normalized * movementSpeed * Time.deltaTime * runMultiplier;

        movement = movement + new Vector3(0f, rb.velocity.y, 0f);

        if (spaced)
        {
            spaced = false;
            movement += new Vector3(0f, movementSpeed * Time.deltaTime / 1.4f, 0f);
        }

        if (groundCheck && jumped)
        {
            jumped = false;
        }


        if (jumped)
        {
            playerAnim.Play("Jump");
            rightArmAnim.Play("JumpArm");
        }
        else if (movementHorizontal != 0f || movementVertical != 0f)
        {
            if (isRunning)
            {
                playerAnim.SetFloat("multiplier", 2f);
                rightArmAnim.SetFloat("multiplier", 2f);
                if (reseted)
                {
                    playerAnim.Play("Walk", 0, 0f);
                    rightArmAnim.Play("WalkArm", 0, 0f);
                    reseted = false;
                }
                else
                {
                    playerAnim.Play("Walk");
                    rightArmAnim.Play("WalkArm");
                }
            }
            else
            {
                playerAnim.SetFloat("multiplier", 1f);
                rightArmAnim.SetFloat("multiplier", 1f);
                if (reseted)
                {
                    playerAnim.Play("Walk", 0, 0f);
                    rightArmAnim.Play("WalkArm", 0, 0f);
                    reseted = false;
                }
                else
                {
                    playerAnim.Play("Walk");
                    rightArmAnim.Play("WalkArm");
                }
            }      
        }
        else if (pistolEquiped)
        {
            playerAnim.Play("IdlePistol");
        }
        else
        {
            if (reseted)
            {
                playerAnim.Play("Idle", 0, 0f);
                rightArmAnim.Play("IdleArm", 0, 0f);
                reseted = false;
            }
            else
            {
                playerAnim.Play("Idle");
                rightArmAnim.Play("IdleArm");
            }
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
