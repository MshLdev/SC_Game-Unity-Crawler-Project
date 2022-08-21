using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    GameObject playerCamera;
    UI_Interface SM;
    Hero hero;

    float spellcost = -12.5f;


    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("_GAME").GetComponent<Hero>();

        playerCamera = GameObject.Find("Player_Camera");
        SM = GameObject.Find("_GAME").GetComponent<UI_Interface>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && hero.mana > spellcost*-1 && !Cursor.visible)
            castSpell();
    }

    void castSpell()
    {
        hero.addToBars(2, spellcost);
        GameObject SpellInstation = Instantiate(SM.SpellBook[SM.currentSpellId], playerCamera.transform.position + playerCamera.transform.forward, Quaternion.identity);
        SpellInstation.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 500f);
    }
}
