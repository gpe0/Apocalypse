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
    public GameObject impactPlayer;
    public GameObject impact;


    private float impactForce = 3000f;
    private float range = 27f;

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
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            enemyScript target = hit.transform.GetComponent<enemyScript>();
            if (target != null)
            {
                target.TakeDamage();
                GameObject impactPlayerGO = Instantiate(impactPlayer, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactPlayerGO, 2f);

            }
            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point);
            }
        }
    }
}
