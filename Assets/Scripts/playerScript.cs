using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class playerScript : MonoBehaviour
{
    public RectTransform lifeBar;
    public GameObject deadBody;
    public playerLookAround playerLA;
    public enemyScript enemyS;
    public ParticleSystem bleed;
    public PostProcessVolume volume;
    public bool isDead = false;


    private float life = 100f;
    private float zombieDamage = 20f;
    private bool isBleeding = false;
    private float lifeDrain = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isBleeding) {
            lifeBar.sizeDelta = new Vector2(lifeBar.sizeDelta.x - Time.deltaTime * lifeDrain, 0f) + new Vector2(0f, lifeBar.sizeDelta.y);
            lifeBar.position = new Vector3(lifeBar.position.x + Time.deltaTime * lifeDrain / -2, lifeBar.position.y, lifeBar.position.z);
        }
        if (lifeBar.sizeDelta.x <= 0f)
        {
            isDead = true;
            Destroy(gameObject);
            GameObject deadBodyGO = Instantiate(deadBody, transform.position, transform.rotation);
            playerLA.enabled = false;
        }
    }

    public void TakeDamage()
    {
        float rand = Random.Range(0f, 1f);
        if (rand <= 0.4f)
        {
            if (!isBleeding)
            {
                isBleeding = true;
                bleed.Play();
                volume.profile.TryGetSettings<Vignette>(out var vignette);
                var colorParameter = new UnityEngine.Rendering.PostProcessing.ColorParameter();
                colorParameter.value = Color.red;
                vignette.color.Override(colorParameter);
            }
        }
        life -= zombieDamage;
        lifeBar.sizeDelta = new Vector2(lifeBar.sizeDelta.x - zombieDamage * 3, 0f) + new Vector2(0f, lifeBar.sizeDelta.y);
        lifeBar.position = new Vector3(lifeBar.position.x + zombieDamage * 3 / -2, lifeBar.position.y, lifeBar.position.z);   
    }
}
