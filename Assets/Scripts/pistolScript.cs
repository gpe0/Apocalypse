using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistolScript : MonoBehaviour
{

    public Transform pistolTracker;
    public Animator anim;
    public Camera fpsCam;
    public Transform bulletSpawn;

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
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawn.position, fpsCam.transform.forward, out hit))
        {
            //CODE
        }
    }
}
