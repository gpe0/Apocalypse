using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiHandler : MonoBehaviour
{
    public Text weaponInfo;
    public GameObject bullets;

    // Update is called once per frame
    public void ChangeWeaponInfo()
    {
        if (weaponInfo.text == "Unarmed")
        {
            weaponInfo.text = "pistol";
            bullets.SetActive(true);
        } else
        {
            weaponInfo.text = "Unarmed";
            bullets.SetActive(false);
        }
    }
}
