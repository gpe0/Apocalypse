using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public Transform player;
    public pistolScript ps;
    public Animator anim;
    public GameObject deadBody;
    public GameObject blood;
    
    
    
    private Rigidbody rb;
    private float life = 100f;
    private bool dead = false;
    private float speed = 100f;
    private Vector3 lastBlood;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    private void Update()
    {
        if (!dead)
        {
            anim.Play("WalkZombie");
            transform.LookAt(player, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            rb.velocity = new Vector3(player.position.x - transform.position.x, 0f, player.position.z - transform.position.z) * speed * Time.deltaTime + new Vector3(0f, rb.velocity.y, 0f);
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
