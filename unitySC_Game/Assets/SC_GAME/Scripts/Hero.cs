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

    void Start()
    {
        uiinterface = GameObject.Find("_GAME").GetComponent<UI_Interface>();
        uiinterface.updateBars(mana, maxmana, health, maxhealth);
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
        uiinterface.updateBars(mana, maxmana, health, maxhealth);
        }
        regColldown += Time.deltaTime;
    }

    public void addToBars(int type, float value)
    {
        switch (type)
        {
            case 1:
                health += value;
                break;
            case 2:
                mana += value;
                break;
            default:
                break;
        }
        uiinterface.updateBars(mana, maxmana, health, maxhealth);
    }

}
