using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public GameObject pistol;
    public int maxEnemiesAlive = 10;

    private float minX = 2.3f;
    private float maxX = 98.8f;
    private float minZ = -33f;
    private float maxZ = -18f;
    private float minimumDistanceToSpawn = 40f;


    // Update is called once per frame
    void Update()
    {
        if (maxEnemiesAlive != 0)
        {
            MoveSpawnRandom();
            float distanceToPlayer = Mathf.Sqrt(Mathf.Pow((player.transform.position.x - transform.position.x), 2) + Mathf.Pow((player.transform.position.y - transform.position.y), 2) + Mathf.Pow((player.transform.position.z - transform.position.z), 2));

            if (distanceToPlayer > minimumDistanceToSpawn)
            {
                EnemySpawn();
            }
        }
    }

    void EnemySpawn()
    {
        GameObject enemyGO = Instantiate(enemy, transform.position, transform.rotation);
        enemyGO.GetComponent<enemyScript>().player = player.GetComponent<Transform>();
        enemyGO.GetComponent<enemyScript>().playerS = player.GetComponent<playerScript>();
        enemyGO.GetComponent<enemyScript>().playerM = player.GetComponent<playerMovement>();
        enemyGO.GetComponent<enemyScript>().ps = pistol.GetComponent<pistolScript>();
        enemyGO.GetComponent<enemyScript>().esScript = gameObject.GetComponent<enemySpawn>();
        maxEnemiesAlive--;
    }

    void MoveSpawnRandom()
    {
        float xPos = Random.Range(minX, maxX);
        float zPos = Random.Range(minZ, maxZ);
        transform.position = new Vector3(xPos, transform.position.y, zPos);
        transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
}
