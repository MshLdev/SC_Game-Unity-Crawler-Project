using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{
    public float mana = 100f;
    public float maxmana = 100f;
    public float manaregen = 5f;

    public float health = 100f;
    public float maxhealth = 100f;
    public float healthregen = 10f;

    public int level = 1;
    public int exp = 0;
    public int nextexp = 800;

    float regColldown = 0;
    
    UI_Interface uiinterface;
    GameObject heroArms;

    void Start()
    {
        uiinterface = GameObject.Find("_GAME").GetComponent<UI_Interface>();
        uiinterface.updateBar(1, mana, maxmana);
        uiinterface.updateBar(0, health, maxhealth);
        
        //To access the Arms
        heroArms = GameObject.Find("Arms_Mesh");
        updateArms();
    }

    void Update()
    {
        if(health < maxhealth || mana < maxmana)
            regen();
    }

    void regen()
    {
        if(regColldown >= 1f)
        {
        mana += manaregen;
            if(mana>maxmana)
                mana = maxmana;
        health += healthregen;
            if(health>maxhealth)
                health = maxhealth;
        
        regColldown = 0f;
        uiinterface.updateBar(1, mana, maxmana);
        uiinterface.updateBar(0, health, maxhealth);
        updateArms();
        }
        regColldown += Time.deltaTime;
    }

    public void addToBars(int type, float value)
    {
        switch (type)
        {
            case 1:
                health += value;
                uiinterface.updateBar(0, health, maxhealth);
                break;
            case 2:
                mana += value;
                uiinterface.updateBar(1, mana, maxmana);
                updateArms();
                break;
            default:
                break;
        }
    }

    void updateArms()
    {
        float glowPotential = maxmana/(16 + (maxmana/50));

        heroArms.GetComponent<Renderer>().material.SetFloat("MaskGlow", glowPotential * mana/maxmana);
    }

}
