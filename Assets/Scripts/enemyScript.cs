using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private float life = 100f;

    private void Update()
    {
        if (life <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage()
    {
        life -= 20f;
    }

}
