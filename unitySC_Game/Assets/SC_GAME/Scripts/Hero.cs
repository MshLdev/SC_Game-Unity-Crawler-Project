using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{

    public barValues health = new barValues("health", 100, 100, 10);
    public barValues mana = new barValues("mana", 80, 80, 5);

    public int level = 1;
    public int exp = 0;
    public int nextexp = 800;
    
    UI_Interface uiinterface;
    GameObject heroArms;

    void Start()
    {
        uiinterface = GameObject.Find("_GAME").GetComponent<UI_Interface>();
        uiinterface.updateBar(1, mana.value, mana.maxvalue);
        uiinterface.updateBar(0, health.value, health.maxvalue);

        InvokeRepeating("Regen", 0, 1.5f);
        
        //To access the Arms
        heroArms = GameObject.Find("Arms_Mesh");
        updateArms();
    }

    void Update()
    {
        uiinterface.updateBar(1, mana.value, mana.maxvalue);
        uiinterface.updateBar(0, health.value, health.maxvalue);

        //cASTING INTEGRATION
        if(Input.GetKeyDown(KeyCode.Mouse0) && !Cursor.visible)
            castSpell();

        mana.ValueToTarget();
        health.ValueToTarget();        
        updateArms();    
    }

    void Regen()
    {
        mana.Regen();
        health.Regen();
    }

    void updateArms()
    {
        heroArms.transform.parent.GetComponent<Animator>().enabled = !Cursor.visible;

        float glowPotential = mana.maxvalue/(16 + (mana.maxvalue/50));
        heroArms.GetComponent<Renderer>().material.SetFloat("MaskGlow", glowPotential * mana.value/mana.maxvalue);
    }

    ///cASTING iNTEGRATION
    void castSpell()
    {
        if(mana.targetValue < uiinterface.spellcost*-1)
            return;

        mana.Deal(uiinterface.spellcost);
        GameObject SpellInstation = Instantiate(uiinterface.SpellBook[uiinterface.currentSpellId], Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
        SpellInstation.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 500f);
    }

}


public struct barValues
{
    public string name;

    public float value;
    public float maxvalue;
    public float regenvalue;

    public float targetValue;
    
    public barValues(string n, float v, float mv, float rv)
    {
        this.name = n;

        this.value = v;
        this.maxvalue = mv;
        this.regenvalue = rv;

        this.targetValue = mv;
    }

    //We will call this every frame I think...
    public void ValueToTarget()
    {
        if(this.targetValue > this.maxvalue)
            this.targetValue = this.maxvalue;
        // Use Mathf.Lerp to interpolate between the current value and the target value
        this.value = Mathf.Lerp(this.value, this.targetValue, Time.deltaTime);
    }

    public void Regen()
    {
        if(this.value < this. maxvalue)
            this.targetValue = this.value + this.regenvalue;
        else
            return;
    }

    public void Deal(float ammount)
    {
        this.targetValue += ammount;
    }
    
}
