using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public GameObject pistol;

    private void Awake()
    {
        EnemySpawn();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemySpawn()
    {
        GameObject enemyGO = Instantiate(enemy, transform.position, transform.rotation);
        enemyGO.GetComponent<enemyScript>().player = player.GetComponent<Transform>();
        enemyGO.GetComponent<enemyScript>().playerS = player.GetComponent<playerScript>();
        enemyGO.GetComponent<enemyScript>().playerM = player.GetComponent<playerMovement>();
        enemyGO.GetComponent<enemyScript>().ps = pistol.GetComponent<pistolScript>();

    }
}
