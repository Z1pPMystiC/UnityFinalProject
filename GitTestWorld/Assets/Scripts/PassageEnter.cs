using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageEnter : MonoBehaviour
{
    [SerializeField] public playerMotor player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Entered Passage");
            player.keyCountText.SetText("");
        }
    }

}