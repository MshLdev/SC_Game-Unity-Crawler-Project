using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{
    UI_Interface uiinterface;
    GameObject heroArms;
    public GameObject castOrigin;
    DATABASE db;
    controller_arms armsCTRL;

    public void Init()
    {
        uiinterface     = GameObject.Find("_GAME").GetComponent<UI_Interface>();
        db              = GameObject.Find("_GAME").GetComponent<DATABASE>();
        armsCTRL        = GameObject.Find("ARMS").GetComponent<controller_arms>();
        InvokeRepeating("Regen", 0, 1.5f);
        
        //To access the Arms
        heroArms = GameObject.Find("Arms_Mesh");
        //updateArms();
    }

    void Update()
    {
        //cASTING INTEGRATION
        if(Input.GetKeyDown(KeyCode.Mouse0) && !Cursor.visible)
            castSpell();    
        //updateArms();    
    }

    void Regen()
    {
        db.DATA_Hero.mana.Regen();
        db.DATA_Hero.health.Regen();
    }

    void updateArms()
    {
        heroArms.transform.parent.GetComponent<Animator>().enabled = !Cursor.visible;

        float glowPotential = db.DATA_Hero.mana.maxvalue/(16 + (db.DATA_Hero.mana.maxvalue/50));
        heroArms.GetComponent<Renderer>().material.SetFloat("MaskGlow", glowPotential * db.DATA_Hero.mana.value/db.DATA_Hero.mana.maxvalue);
    }

    ///cASTING iNTEGRATION
    void castSpell()
    {
        //do we have enough mana??
        if(db.DATA_Hero.mana.targetValue < uiinterface.spellcost*-1)
            return;

        db.DATA_Hero.mana.Deal(uiinterface.spellcost);
        GameObject SpellInstation = Instantiate(uiinterface.SpellBook[uiinterface.currentSpellId], castOrigin.transform.position, Quaternion.identity);
        SpellInstation.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 1500);
        armsCTRL.Attack();
    }

    void camRay()
    {
    
    }

}
