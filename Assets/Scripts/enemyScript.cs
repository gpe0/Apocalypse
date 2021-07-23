using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    public Transform player;
    public playerScript playerS;
    public pistolScript ps;
    public playerMovement playerM;
    public Animator zombieAnim;
    public Animator armsAnim;
    public GameObject deadBody;
    public GameObject blood;
    public NavMeshAgent agent;
    public bool heardShot = false;

    
    private Rigidbody rb;
    private float life = 100f;
    private bool dead = false;
    private Vector3 lastBlood;
    private float range = 30f;
    private float hearRange = 45f;
    private float attackRange = 2.5f;
    private float attackCool = 2f;
    private float lastAttack = -2f;
    private float zombieVisionRange = 70f;
    private bool tookDamage = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    private void Update()
    {
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow((player.position.x - transform.position.x), 2) + Mathf.Pow((player.position.y - transform.position.y), 2) + Mathf.Pow((player.position.z - transform.position.z), 2));


        if (!dead)
        {

            if (ps.shot)
            {
                if (distanceToPlayer < hearRange)
                {
                    heardShot = true;
                }
            }

            if (heardShot || tookDamage || (Vector3.Angle(transform.forward, player.position - transform.position) < zombieVisionRange && Vector3.Angle(transform.forward, player.position - transform.position) > -zombieVisionRange))
            {
            
                if ((heardShot || tookDamage || distanceToPlayer <= range && !playerM.isCrouched) || (distanceToPlayer <= range/2 && playerM.isCrouched))
                {
                    if (distanceToPlayer <= attackRange && lastAttack + attackCool <= Time.timeSinceLevelLoad)
                    {
                        armsAnim.Play("AttackZombie");
                        playerS.TakeDamage();
                        lastAttack = Time.timeSinceLevelLoad;
                    }
                    zombieAnim.Play("WalkZombie");
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                    agent.SetDestination(player.position);

                        //rb.velocity = new Vector3(player.position.x - transform.position.x, 0f, player.position.z - transform.position.z) * speed * Time.deltaTime + new Vector3(0f, rb.velocity.y, 0f);
                }
                else
                {
                    zombieAnim.Play("IdleZombie");
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }
            } else
            {
                if ((distanceToPlayer <= range / 1.5 && !playerM.isCrouched) || (distanceToPlayer <= -1 && playerM.isCrouched))
                {
                    if (distanceToPlayer <= attackRange && lastAttack + attackCool <= Time.timeSinceLevelLoad)
                    {
                        armsAnim.Play("AttackZombie");
                        playerS.TakeDamage();
                        lastAttack = Time.timeSinceLevelLoad;
                    }
                    zombieAnim.Play("WalkZombie");
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                    agent.SetDestination(player.position);

                }
                else
                {
                    zombieAnim.Play("IdleZombie");
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }
            }
        }
  
        if (life <= 0f)
        {
            dead = true;
            Destroy(gameObject);
            GameObject deadBodyGO = Instantiate(deadBody, transform.position, transform.rotation);
            GameObject bloodGO = Instantiate(blood, lastBlood , transform.rotation);
            Destroy(bloodGO, 2f);
            Destroy(deadBodyGO, 10f);
        }
    }
    public void TakeDamage(RaycastHit hit)
    {
        tookDamage = true;
        if (ps.headshot == true)
        {
            life -= 50f;
        }
        else
        {
            life -= 20f;
        } 

        if (life <= 0f)
        {
            lastBlood = hit.point;
        }
    }

}
