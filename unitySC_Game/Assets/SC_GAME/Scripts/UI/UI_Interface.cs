using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



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

    ///Init in script loadup, becouse this data can be needed before Start() is called
    void Awake()
    {
        ///to access the healthbars
        UI_Object = GameObject.Find("UI_Interface");
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


    public void updateBar(int barID, float value, float maxvalue)
    {
        UI_Object.transform.GetChild(barID).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(value/maxvalue, 1f, 1f);  
        UI_Object.transform.GetChild(barID).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{(int)value}/{(int)maxvalue}";
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
