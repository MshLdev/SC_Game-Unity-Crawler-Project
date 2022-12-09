using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



///This script currently draws and picks the spell bars and Modifies the 
///character Mesh properties
///Better to rename it in the future or split it and integrate into other scripts

///This script Depends on AudioMenager which actually is acceptable


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
        ///to access the healthbars
        UI_Object = GameObject.Find("UI_Interface");
        //To access the Arms
        heroArms = GameObject.Find("Arms_Mesh");
        //This is not spaghetti, ok?
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

        ///TO JEST DO WYJEBANIA REEEEEEEEE
        ///REEEEEEEEEEEEEEEEEEEEe (╯°□°）╯︵ ┻━┻
        if(!UI_Object)
            Start();

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
        audioM.AudioAtPlayer(AudioMenager.clips.ui_select);
        //Debug.Log($"picked spell number{slotIndex}");
        SpellBar_Hook.transform.GetChild(currentSlotid).GetComponent<Image>().color = new Color32(75, 75, 75, 255);
        SpellBar_Hook.transform.GetChild(slotIndex).GetComponent<Image>().color = Color.green;
        currentSlotid = slotIndex;
    }

    void closeApp()
    {
        
    }
}
