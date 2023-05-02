using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Dialogue : MonoBehaviour

{
  public GameObject ui_DialoguePrefab;
  GameObject newObject;
  

    private void OnTriggerEnter(Collider NPC_Ghost) 
    {
       Transform dialogparent = GameObject.Find("UI_Dialogue_Parent").transform;
        
        if (NPC_Ghost.gameObject.tag == "Player")
        {
          
          //create Dialogue window
          newObject = Instantiate(ui_DialoguePrefab,  dialogparent);
          //get picking option A button
            Button UI_Dialogue_OptionA_Button = newObject.transform.GetChild(5).GetComponent<Button>();
          //get picking option B button
            Button UI_Dialogue_OptionB_Button = newObject.transform.GetChild(6).GetComponent<Button>();
          //get closing button
          Button UI_Dialogue_OptionC_Button = newObject.transform.GetChild(7).GetComponent<Button>();
          //assign function to the picking button A on click event
          UI_Dialogue_OptionA_Button.onClick.AddListener(PickingOptionA);
          //assign function to the picking button B on click event
          UI_Dialogue_OptionB_Button.onClick.AddListener(PickingOptionB);
          //assign function to the closing button on click event 
          UI_Dialogue_OptionC_Button.onClick.AddListener(DestroyDialogue);
          //making visable cursor
          Cursor.visible = true;
          Cursor.lockState = CursorLockMode.None;
        }
    
    }

   void PickingOptionA() 
   {
    TMP_Text UI_Text_Dialogue_NPC_Speech = newObject.transform.GetChild(3).GetComponentInChildren<TMP_Text>();
    UI_Text_Dialogue_NPC_Speech.text = "test";
   }

   void PickingOptionB()
   { 
    TMP_Text UI_Text_Dialogue_NPC_Speech = newObject.transform.GetChild(3).GetComponentInChildren<TMP_Text>();
    UI_Text_Dialogue_NPC_Speech.text = "Sample";
   }

   void DestroyDialogue()

   {
    
    Destroy(newObject);
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
   }

}