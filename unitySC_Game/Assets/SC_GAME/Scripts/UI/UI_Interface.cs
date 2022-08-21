using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interface : MonoBehaviour
{
    public int currentSlotid = 0;
    public int currentSpellId = 0;
    public GameObject[] SpellBook;

    GameObject SpellBar_Hook;
    AudioMenager audioM;

    GameObject UI_Object;
    GameObject heroArms;

    void Start()
    {
        UI_Object = GameObject.Find("UI_Interface");
        heroArms = GameObject.Find("Arms_Mesh");

        audioM = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        SpellBar_Hook = GameObject.Find("SpellBar");
        //pickSpell(currentSlotid);
    }

    void Update()
    {
        if(Input.inputString != "")
        {
            int selector;
            if (int.TryParse(Input.inputString, out selector))
                pickSpell(selector);
        }
    }

    public void updateBars(float mana, float maxmana, float health, float maxhealth)
    {
        float mscalefactor = (mana/maxmana);
        float hscalefactor = (health/maxhealth);

        ////ABSYŚ NIE PSOĆ TUTAJ (ʘ‿ʘ)
        UI_Object.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(mscalefactor, 1f, 1f);   //1 to jes HealthBar, kcemy od jego childa 'fill'
        UI_Object.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = $"{(int)mana}/{(int)maxmana}";

        UI_Object.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(hscalefactor, 1f, 1f); 
        UI_Object.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = $"{(int)health}/{(int)maxhealth}";


        //Update also hands
        heroArms.GetComponent<Renderer>().material.SetFloat("MaskGlow", 10f * mscalefactor);
    }

    void pickSpell(int slotIndex)
    {   
        audioM.AudioAtPlayer(3);
        //Debug.Log($"picked spell number{slotIndex}");
        SpellBar_Hook.transform.GetChild(currentSlotid).GetComponent<Image>().color = new Color32(75, 75, 75, 255);
        SpellBar_Hook.transform.GetChild(slotIndex).GetComponent<Image>().color = Color.green;
        currentSlotid = slotIndex;
    }

    void closeApp()
    {
        
    }
}
