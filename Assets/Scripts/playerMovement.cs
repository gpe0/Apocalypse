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
    public RectTransform stamina;


    private Rigidbody rb;
    private bool groundCheck = false;
    private bool isRunning = false;
    private bool jumped = false;
    private float runMultiplier = 1f;
    private bool reseted = false;
    private bool spaced = false;
    private float staminaDrain = 30f;
    private float staminaRegen = 15f;
    private float jumpStaminaDrain = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pistol.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && groundCheck && stamina.sizeDelta.x > 30f)
        {
            isRunning = true;
            runMultiplier = 2f;
        }

        if (isRunning && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            if (stamina.sizeDelta.x < 0f)
            {
                stamina.sizeDelta = new Vector2(0f, stamina.sizeDelta.y);
            }
            else
            {
                stamina.sizeDelta = new Vector2(stamina.sizeDelta.x - Time.deltaTime * staminaDrain, 0f) + new Vector2(0f, stamina.sizeDelta.y);
                stamina.position = new Vector3(stamina.position.x + Time.deltaTime * staminaDrain / -2, stamina.position.y, stamina.position.z);
            }   
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || stamina.sizeDelta.x <= 0f)
        {
            isRunning = false;
            runMultiplier = 1f;
        }

        if ((!isRunning || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)) && !jumped)
        {
            if (stamina.sizeDelta.x >= 200f)
            {
                stamina.sizeDelta = new Vector2(200f, stamina.sizeDelta.y);
            }
            else
            {
                stamina.sizeDelta = new Vector2(stamina.sizeDelta.x + Time.deltaTime * staminaRegen, 0f) + new Vector2(0f, stamina.sizeDelta.y);
                stamina.position = new Vector3(stamina.position.x + Time.deltaTime * staminaRegen / 2, stamina.position.y, stamina.position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && groundCheck && stamina.sizeDelta.x > jumpStaminaDrain)
        {
            jumped = true;
            groundCheck = false;
            spaced = true;
            stamina.sizeDelta = new Vector2(stamina.sizeDelta.x - jumpStaminaDrain, 0f) + new Vector2(0f, stamina.sizeDelta.y);
            stamina.position = new Vector3(stamina.position.x + jumpStaminaDrain / -2, stamina.position.y, stamina.position.z);
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
