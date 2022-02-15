using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupWeapon : MonoBehaviour
{

    public WeaponSwaper weaponSwaper;
    public TextMeshProUGUI centerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 100f * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            weaponSwaper.teslaEnabled = true;
            centerText.SetText("New Weapon Unlocked: Tesla Coil!");
            Invoke("ClearText", 2f);
            gameObject.SetActive(false);
        }
    }

    public void ClearText()
    {
        if (centerText != null)
        {
            centerText.SetText("");
        }
        Destroy(gameObject);
    }
}
