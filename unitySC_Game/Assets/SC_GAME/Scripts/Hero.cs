using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    

    GameObject UI_Object;
    GameObject heroArms;

    void Start()
    {
        UI_Object = GameObject.Find("UI_Canvas");
        heroArms = GameObject.Find("Arms_Mesh");

        updateBars();
    }

    void Update()
    {
        if(health < maxhealth || mana < maxmana)
            regen();
    }

    void updateBars()
    {
        float mscalefactor = (mana/maxmana);
        float hscalefactor = (health/maxhealth);

        ////ABSYŚ NIE PSOĆ TUTAJ (ʘ‿ʘ)
        UI_Object.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(mscalefactor, 1f, 1f);   //1 to jes HealthBar, kcemy od jego childa 'fill'
        UI_Object.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = $"{(int)mana}/{(int)maxmana}";

        UI_Object.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(hscalefactor, 1f, 1f); 
        UI_Object.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = $"{(int)health}/{(int)maxhealth}";


        //Update also hands
        heroArms.GetComponent<Renderer>().material.SetFloat("MaskGlow", 10f * mscalefactor);
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
        updateBars();
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
        updateBars();
    }

}
