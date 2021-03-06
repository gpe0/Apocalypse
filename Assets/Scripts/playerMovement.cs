using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{


    public GameObject pistol;
    public Animator playerAnim;
    public Animator rightArmAnim;
    public Animator leftArmAnim;
    public bool pistolEquiped = false;
    public bool isCrouched = false;
    public RectTransform stamina;
    public uiHandler uiHand;
    public Text bullets;
    public int bulletsNum;
    public bool reload = false;
    public Transform head;
    public Transform rightArm;
    public Transform groundObj;
    public LayerMask ground;

    private bool groundCheck;
    private float groundDistance = 0.4f;
    private float movementSpeed = 12f;
    private CharacterController characterController;
    private Vector3 velocity;
    private float gravity = -30f;
    private bool isRunning = false;
    private bool jumped = false;
    private float runMultiplier = 1f;
    private bool reseted = false;
    private float staminaDrain = 30f;
    private float staminaRegen = 15f;
    private float jumpStaminaDrain = 15f;
    private float movementHorizontal;
    private float movementVertical;
    private bool isCrouching = false;
    private bool isAscending = false;
    private int bulletsMax = 12;
    private float jumpHeight = 3f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        pistol.SetActive(false);
        bulletsNum = int.Parse(bullets.text);
    }

    private void Update()
    {
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis("Vertical");

        groundCheck = Physics.CheckSphere(groundObj.position, groundDistance, ground);

        Vector3 move = transform.right * movementHorizontal + transform.forward * movementVertical;

        move = move.normalized;

        characterController.Move(move * Time.deltaTime * movementSpeed * runMultiplier);

        if (groundCheck && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        if (Input.GetKeyDown(KeyCode.R) && pistolEquiped && bulletsNum < 12)
        {
            rightArmAnim.enabled = true;
            reload = true;
            bullets.text = "reloading...";
            bulletsNum = bulletsMax;
            if (!isCrouched)
            {
                rightArmAnim.Play("PistolReloadRight");
                leftArmAnim.Play("PistolReloadLeft");
            }
            else
            {
                rightArmAnim.Play("CrouchPistolReloadRight");
                leftArmAnim.Play("CrouchPistolReloadLeft");
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && groundCheck && stamina.sizeDelta.x > 30f && !isCrouched)
        {
            isRunning = true;
            runMultiplier = 2f;
        }

        if (isRunning && (movementHorizontal != 0 || movementVertical != 0))
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

        if ((!isRunning || (movementHorizontal == 0 && movementVertical == 0)) && !jumped)
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
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
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
                uiHand.ChangeWeaponInfo();
            }
            else
            {
                pistolEquiped = true;
                pistol.SetActive(true);
                rightArmAnim.enabled = false;
                uiHand.ChangeWeaponInfo();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && groundCheck && !reload && !isCrouched)
        {
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && groundCheck && !reload && isCrouched)
        {
            isAscending = true;
        }

    }


    private void FixedUpdate()
    {
        if (isCrouching)
        {
            isCrouched = true;
            isCrouching = false;
            head.position = new Vector3(head.position.x, head.position.y - 0.35f, head.position.z);
            rightArm.position = new Vector3(rightArm.position.x, transform.position.y + 2.05f, rightArm.position.z);
            playerAnim.Play("Crouch");
            if (!reload)
            {
                rightArmAnim.Play("CrouchArm");
                leftArmAnim.Play("CrouchLeftArm");
            }
            playerAnim.SetFloat("multiplier", 0.5f);
            rightArmAnim.SetFloat("multiplier", 0.5f);
            leftArmAnim.SetFloat("multiplier", 0.5f);
            runMultiplier = 0.5f;
        }
        if (isAscending)
        {
            isCrouched = false;
            isAscending = false;
            head.position = new Vector3(head.position.x, head.position.y + 0.35f, head.position.z);
            rightArm.position = new Vector3(rightArm.position.x, transform.position.y + 2.433894f, rightArm.position.z);
            playerAnim.Play("Ascend");
            if (!reload)
            {
                rightArmAnim.Play("AscendArm");
                leftArmAnim.Play("AscendLeftArm");
            }
            playerAnim.SetFloat("multiplier", 1f);
            rightArmAnim.SetFloat("multiplier", 1f);
            leftArmAnim.SetFloat("multiplier", 1f);
            runMultiplier = 1f;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if (groundCheck && jumped)
        {
            jumped = false;
        }

        if ((!leftArmAnim.GetCurrentAnimatorStateInfo(0).IsName("PistolReloadLeft") && !leftArmAnim.GetCurrentAnimatorStateInfo(0).IsName("CrouchPistolReloadLeft")) && reload)
        {
            reload = false;
            rightArmAnim.enabled = false;
            bullets.text = bulletsMax.ToString();
            if (!isCrouched)
            {
                rightArm.position = new Vector3(rightArm.position.x, transform.position.y + 2.433894f, rightArm.position.z);
            }
            else
            {
                rightArm.position = new Vector3(rightArm.position.x, transform.position.y + 2.05f, rightArm.position.z);
            }
        }

        if (!(playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Ascend") || playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Crouch")))
        {
            if (isCrouched)
            {
                if (movementHorizontal != 0f || movementVertical != 0f)
                {
                    if (reseted)
                    {
                        playerAnim.Play("CrouchWalk", 0, 0f);
                        if (!reload)
                        {
                            rightArmAnim.Play("CrouchWalkArm", 0, 0f);
                            leftArmAnim.Play("CrouchWalkLeftArm", 0, 0f);
                        }

                        reseted = false;
                    }
                    else
                    {
                        playerAnim.Play("CrouchWalk");
                        if (!reload)
                        {
                            rightArmAnim.Play("CrouchWalkArm");
                            leftArmAnim.Play("CrouchWalkLeftArm");
                        }
                    }
                }
                else
                {
                    if (reseted)
                    {
                        playerAnim.Play("CrouchIdle", 0, 0f);
                        if (!reload)
                        {
                            rightArmAnim.Play("CrouchIdleArm", 0, 0f);
                            leftArmAnim.Play("CrouchIdleLeftArm", 0, 0f);
                        }
                    }
                    else
                    {
                        playerAnim.Play("CrouchIdle");
                        if (!reload)
                        {
                            rightArmAnim.Play("CrouchIdleArm");
                            leftArmAnim.Play("CrouchIdleLeftArm");
                        }
                    }
                }
            }
            else if (jumped)
            {
                playerAnim.Play("Jump");
                if (!reload)
                {
                    rightArmAnim.Play("JumpArm");
                    leftArmAnim.Play("JumpLeftArm");
                }
            }
            else if (movementHorizontal != 0f || movementVertical != 0f)
            {
                if (isRunning)
                {
                    playerAnim.SetFloat("multiplier", 2f);
                    rightArmAnim.SetFloat("multiplier", 2f);
                    leftArmAnim.SetFloat("multiplier", 2f);
                    if (reseted)
                    {
                        playerAnim.Play("Walk", 0, 0f);
                        if (!reload)
                        {
                            rightArmAnim.Play("WalkArm", 0, 0f);
                            leftArmAnim.Play("WalkLeftArm", 0, 0f);
                        }

                        reseted = false;
                    }
                    else
                    {
                        playerAnim.Play("Walk");
                        if (!reload)
                        {
                            rightArmAnim.Play("WalkArm");
                            leftArmAnim.Play("WalkLeftArm");
                        }
                    }
                }
                else
                {
                    playerAnim.SetFloat("multiplier", 1f);
                    rightArmAnim.SetFloat("multiplier", 1f);
                    leftArmAnim.SetFloat("multiplier", 1f);
                    if (reseted)
                    {
                        playerAnim.Play("Walk", 0, 0f);
                        if (!reload)
                        {
                            rightArmAnim.Play("WalkArm", 0, 0f);
                            leftArmAnim.Play("WalkLeftArm", 0, 0f);
                        }
                        reseted = false;
                    }
                    else
                    {
                        playerAnim.Play("Walk");
                        if (!reload)
                        {
                            rightArmAnim.Play("WalkArm");
                            leftArmAnim.Play("WalkLeftArm");
                        }
                    }
                }
            }
            else if (pistolEquiped)
            {
                playerAnim.Play("IdlePistol");
                if (!reload)
                {
                    leftArmAnim.Play("IdleLeftArm");
                }
            }
            else
            {
                if (reseted)
                {
                    playerAnim.Play("Idle", 0, 0f);
                    if (!reload)
                    {
                        rightArmAnim.Play("IdleArm", 0, 0f);
                        leftArmAnim.Play("IdleLeftArm", 0, 0f);
                    }
                    reseted = false;
                }
                else
                {
                    playerAnim.Play("Idle");
                    if (!reload)
                    {
                        rightArmAnim.Play("IdleArm");
                        leftArmAnim.Play("IdleLeftArm");
                    }
                }
            }
        }
    }
}


