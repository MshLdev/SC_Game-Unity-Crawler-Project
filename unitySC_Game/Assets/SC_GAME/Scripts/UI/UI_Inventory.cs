using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



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
    public Vector3 slotStartVector = new Vector3 (-125, 180, 0);

    //Items Manipulation
    private int[] itemsList;
    private int pageID = 0;
    private int itemHand = 0;
    
    //depends on
    private GameObject ui_inv;
    private AudioMenager audioM;
    private DATABASE db;

    void Start()
    {
        itemsList =     new int[numberOfX * numberOfY];                             //start ItemArray with empty slots
        ui_inv =        GameObject.Find("UI_Inventory");                            //For UI elements to spawn
        audioM =        GameObject.Find("_GAME").GetComponent<AudioMenager>();      //for Audio
        db =            GameObject.Find("_GAME").GetComponent<DATABASE>();          //for game db
        //add starting items
        startingItems();
        //InitTheUI
        InitSlots(numberOfX, numberOfY);
        //Switch On/Off
        switchInventory();
    }

    void Update()
    {
        updateHand();
        if(Input.GetKeyDown(KeyCode.I))
            switchInventory();
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
                int currID = (i*numY) + j;
                //how to find Slot Id
                ///Debug.Log("Initing slot id -> " + ((i*numY) + j+1));

                //Why I did it as a child 3???? No idea....
                GameObject newSlot = GameObject.Instantiate(ui_invSlot, Vector3.zero, Quaternion.identity, ui_inv.transform.GetChild(3).transform);
                newSlot.GetComponent<RectTransform>().localPosition = slotStartVector + new Vector3((slotSize+RowsMargin) * j, (slotSize+ColumnsMargin) * i * -1, 0);
                newSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
                newSlot.name = "eq_slot["+ currID +"]";
                
                ///set an actual icon
                int itemid = itemsList[currID];
                newSlot.transform.GetChild(0).GetComponent<Image>().sprite = db.DATA_item[itemid].icon;

                ////EVENTYY
                /////AUDIO jako event trigger (SKOPIOWANE PROSTO Z DOKUMENTACJI, TROCHE DZIWNE UGUŁEM)
                EventTrigger trigger = newSlot.GetComponent<EventTrigger>();

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); } );

                trigger.triggers.Add(entry);
                /////ON CLICK
                //newSlot.GetComponent<Button>().onClick.AddListener( () => slotClicked(i) );
                Button btn = newSlot.GetComponent<Button>();
                btn.onClick.AddListener(delegate {slotClicked(currID); });
                ////EVENTY/
            }
        }
    }

    void switchInventory()
    {
        Cursor.visible = !Cursor.visible;
        ui_inv.SetActive(!ui_inv.activeSelf);

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
            
    }

    void startingItems()
    {
        itemsList[0] = 1;   // add hp potiom
        itemsList[1] = 2;   // add mp potion
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

    private void slotClicked(int sID)
    {
        ///Switch items in hand/slot
        //Debug.Log("There was item["+itemHand+"] in the hand, now there is item["+itemsList[sID]+"] in the hand");

        //change id's
        int itemToswitch = itemsList[sID];
        itemsList[sID] = itemHand;
        itemHand = itemToswitch;
        //update icons
        ui_handImage.sprite = db.DATA_item[itemHand].icon;
        //For some rason I didnt store the reference to the slot holder to we do it by accessing child 3, WHAT THE FUCK????????
        ui_inv.transform.GetChild(3).GetChild(sID).GetChild(0).GetComponent<Image>().sprite = db.DATA_item[itemsList[sID]].icon;
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

}
