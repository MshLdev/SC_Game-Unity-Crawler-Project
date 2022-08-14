using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellMenager : MonoBehaviour
{
    public int currentSlotid = 0;
    public int currentSpellId = 0;
    public GameObject[] SpellBook;

    GameObject SpellBar_Hook;
    GameObject Player_Hook;
    AudioMenager audioM;

    void Start()
    {   
        audioM = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        Player_Hook = GameObject.Find("Player");
        SpellBar_Hook = GameObject.Find("SpellBar");
        pickSpell(currentSlotid);
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

    void pickSpell(int slotIndex)
    {   
        audioM.AudioAtPosition(3, Player_Hook.transform.position);
        //Debug.Log($"picked spell number{slotIndex}");
        SpellBar_Hook.transform.GetChild(currentSlotid).GetComponent<Image>().color = new Color32(75, 75, 75, 255);
        SpellBar_Hook.transform.GetChild(slotIndex).GetComponent<Image>().color = Color.green;
        currentSlotid = slotIndex;
    }
}
