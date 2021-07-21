using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistolScript : MonoBehaviour
{

    public Transform pistolTracker;
    public Animator anim;
    public Camera fpsCam;
    public Transform bulletSpawn;
    public ParticleSystem shootEffect;
    public ParticleSystem muzzleFlash;
    public ParticleSystem smoke;
    public GameObject impactPlayer;
    public GameObject impact;
    public bool headshot = false;


    private float impactForce = 120f;
    private float range = 40f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pistolTracker.position;
        transform.rotation = pistolTracker.rotation;

        if (Input.GetMouseButtonDown(0))
        {
            anim.Play("PistolShoot");
            shootEffect.Play();
            muzzleFlash.Play();
            smoke.Play();
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.point.y - transform.position.y >= 0.33f)
            {
                headshot = true;
            }
            enemyScript target = hit.transform.GetComponent<enemyScript>();
            if (target != null)
            {
                target.TakeDamage(hit);
                headshot = false;
            }
            if (hit.transform.tag == "Enemy")
            {
                GameObject impactPlayerGO = Instantiate(impactPlayer, hit.point, Quaternion.LookRotation(hit.normal));
                impactPlayerGO.transform.SetParent(hit.transform);
                impactPlayerGO.transform.localScale = new Vector3(1f, 1f, 1f);
                Destroy(impactPlayerGO, 2f);
            }
            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            impactGO.transform.SetParent(hit.transform);
            impactGO.transform.localScale = new Vector3(1f, 1f, 1f);
            Destroy(impactGO, 2f);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point);
            }
        }
    }
}
