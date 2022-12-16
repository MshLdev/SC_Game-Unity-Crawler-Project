using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;


///SKRYPCIK EKWIPINKU
///
///
///
public class UI_Inventory : MonoBehaviour
{
    
    public GameObject ui_invSlot;
    public Image ui_handImage;
    //eq Size 
    public int numberOfX;
    public int numberOfY;
    //Margins
    public float RowsMargin;
    public float ColumnsMargin;
    //Size of an icon
    public float slotSize;
    public Vector3 slotStartPosition= new Vector3 (-125, 180, 0);

    //Items Manipulation
    private int[] itemsList;
    private int pageID = 0;
    private int itemHand = 0;

    ///Hotbar, needs refactor, Monolith workflow
    public int currentSlotid = 0;
    public int currentSpellId = 0;
    public GameObject[] SpellBook;
    public float spellcost = -12.5f;
    GameObject UI_Object;
    
    //depends on
    private GameObject ui_inv;
    private UI_tooltip ui_tooltip;
    private AudioMenager audioM;
    private DATABASE db;

    void Start()
    {
        itemsList =     new int[numberOfX * numberOfY + 10];           //start ItemArray with empty slots
        ui_inv =        GameObject.Find("UI_Slots");                  //For UI elements to spawn
        UI_Object =     GameObject.Find("UI_Interface");
        ui_tooltip =    gameObject.GetComponent<UI_tooltip>();        //For UI elements to spawn
        audioM =        gameObject.GetComponent<AudioMenager>();      //for Audio
        db =            gameObject.GetComponent<DATABASE>();          //for game db
        //add starting items
        startingItems();
        //InitTheUI
        InitHotbarSlots();
        InitSlots(numberOfX, numberOfY);
        //Switch On/Off
        switchInventory();
    }

    void Update()
    {   
        updateBar(1, db.DATA_Hero.mana.value, db.DATA_Hero.mana.maxvalue);
        updateBar(0, db.DATA_Hero.health.value, db.DATA_Hero.health.maxvalue);

        hotbarInput();
        updateHand();
        if(Input.GetKeyDown(KeyCode.I))
            switchInventory();
    }

    void InitHotbarSlots()
    {
        int i = 0;
        foreach(Transform hbSlot in ui_inv.transform)
        {
            ///???????????????????????????????????????????????????????
            int Slot = i;//???????????????????????????????????????????
            //????????????????????????????????????????????????????????
            EventTrigger trigger = hbSlot.GetComponent<EventTrigger>();

            EventTrigger.Entry EnterEvent = new EventTrigger.Entry();
            EnterEvent.eventID = EventTriggerType.PointerEnter;
            EnterEvent.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); ui_tooltip.DisplayTooltipItem(db.DATA_item[itemsList[Slot]].Name, db.DATA_item[itemsList[Slot]].Desc , (int)db.DATA_item[itemsList[Slot]].rarity); } );
            trigger.triggers.Add(EnterEvent);

            
            EventTrigger.Entry ExitEvent = new EventTrigger.Entry();
            ExitEvent.eventID = EventTriggerType.PointerExit;
            ExitEvent.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); ui_tooltip.DisableTooltip(); } );
            trigger.triggers.Add(ExitEvent);
            i++;

        }
    }
    //To create the Grid of Cells
    //In the future also for refresing the eq
    void InitSlots(int numX, int numY)
    {
        for(int i = 0; i < numX; i++)
        {
            for(int j = 0; j < numY; j++)
            {   
                //This prevents Bug in 'AddListener()'
                //ADDING + 10 IN VERSION 0.6 ONWARD FOR HOTBAR FUNCTIONALITY!!!!
                int currID = (i*numY) + j + 10;
                //how to find Slot Id
                ///Debug.Log("Initing slot id -> " + ((i*numY) + j+1));

                //Why I did it as a child 3???? No idea....
                GameObject newSlot = GameObject.Instantiate(ui_invSlot, Vector3.zero, Quaternion.identity, ui_inv.transform);
                newSlot.GetComponent<RectTransform>().localPosition = slotStartPosition + new Vector3((slotSize+RowsMargin) * j, (slotSize+ColumnsMargin) * i * -1, 0);
                newSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
                //Also resize the Icon GameObject
                newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
                newSlot.name = "eq_slot["+ currID +"]";
                
                ///set an actual icon
                int itemid = itemsList[currID];
                newSlot.transform.GetChild(0).GetComponent<Image>().sprite = db.DATA_item[itemid].icon;

                ////EVENTYY
                /////AUDIO jako event trigger (SKOPIOWANE PROSTO Z DOKUMENTACJI, TROCHE DZIWNE UGUŁEM)
                EventTrigger trigger = newSlot.GetComponent<EventTrigger>();

                EventTrigger.Entry EnterEvent = new EventTrigger.Entry();
                EnterEvent.eventID = EventTriggerType.PointerEnter;
                EnterEvent.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); ui_tooltip.DisplayTooltipItem(db.DATA_item[itemsList[currID]].Name, db.DATA_item[itemsList[currID]].Desc , (int)db.DATA_item[itemsList[currID]].rarity); } );

                EventTrigger.Entry ExitEvent = new EventTrigger.Entry();
                ExitEvent.eventID = EventTriggerType.PointerExit;
                ExitEvent.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); ui_tooltip.DisableTooltip(); } );

                trigger.triggers.Add(EnterEvent);
                trigger.triggers.Add(ExitEvent);

                /////ON CLICK
                newSlot.GetComponent<Button>().onClick.AddListener(delegate {slotClicked(currID); });;
                ////EVENTY/
            }
        }
    }

    ///do refaktoru, kto to narzigoł w taki sposób, zagryza
    void switchInventory()
    {
        Cursor.visible = !Cursor.visible;
        //using Parent so the background also goes offline(becouse I cant get Find() to work properly)
        ui_inv.transform.parent.parent.gameObject.SetActive(!ui_inv.transform.parent.parent.gameObject.activeSelf);
        
        if(Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            audioM.AudioAtPlayer(AudioMenager.clips.ui_close);
        }
            
        else
        {
            Cursor.lockState = CursorLockMode.Locked; 
            audioM.AudioAtPlayer(AudioMenager.clips.ui_close);
        }
            
        //Exit event is not called(I Assume thats becouse the GO is being disabled before it can react) so we call it manually here
        ui_tooltip.DisableTooltip();
    }

    void startingItems()
    {
        itemsList[10] = 1;   // add hp potiom
        itemsList[11] = 2;   // add mp potion
        itemsList[12] = 3;   // add drink
        itemsList[13] = 4;   // add apple
    }
  
    void pickupItem(int iID)
    {
        for (int i = 0; i < itemsList.Length; i++ )
            if(itemsList[i] == 0)
            {
                itemsList[i] = iID;
                break;
            }
    }

    public void slotClicked(int sID)
    {
        audioM.AudioAtPlayer(db.DATA_item[itemsList[sID]].sound);
        //change id's
        int itemToswitch = itemsList[sID];
        itemsList[sID] = itemHand;
        itemHand = itemToswitch;
        //update icons
        ui_handImage.sprite = db.DATA_item[itemHand].icon;
        //For some rason I didnt store the reference to the slot holder to we do it by accessing child 3, WHAT THE FUCK????????
        ui_inv.transform.GetChild(sID).GetChild(0).GetComponent<Image>().sprite = db.DATA_item[itemsList[sID]].icon;

        if(sID < 10)
            UI_Object.transform.GetChild(2).GetChild(sID).GetChild(0).GetComponent<Image>().sprite = db.DATA_item[itemsList[sID]].icon;
    }

    private void updateHand()
    {
        ui_handImage.transform.position = Input.mousePosition;
    }

    //why I did it like that?? no idea o0
    public void UI_switchInventory()
    {
        switchInventory();
    }
    
    //////////////////////////////////////////////
    //  HOTBAR
    void hotbarInput()
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
        UI_Object.transform.GetChild(2).GetChild(currentSlotid).GetComponent<Image>().color = new Color32(75, 75, 75, 255);
        UI_Object.transform.GetChild(2).GetChild(slotIndex).GetComponent<Image>().color = Color.green;
        currentSlotid = slotIndex;
    }

}

