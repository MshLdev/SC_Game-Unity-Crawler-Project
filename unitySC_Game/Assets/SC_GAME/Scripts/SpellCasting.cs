using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    GameObject playerCamera;
    SpellMenager SM;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Player_Camera");
        SM = GameObject.Find("_GAME").GetComponent<SpellMenager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            castSpell();
    }

    void castSpell()
    {
        GameObject SpellInstation = Instantiate(SM.SpellBook[SM.currentSpellId], playerCamera.transform.position + playerCamera.transform.forward, Quaternion.identity);
        SpellInstation.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 500f);
    }
}
