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
    private int itemHand = 5;
    
    //depends on
    private GameObject ui_inv;
    private AudioMenager audioM;

    void Start()
    {
        //start ItemArray with empty slots
        itemsList = new int[numberOfX * numberOfY];
        //For UI elements to spawn
        ui_inv = GameObject.Find("UI_Inventory");
        //for Audio
        audioM = GameObject.Find("_GAME").GetComponent<AudioMenager>();
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
                GameObject newSlot = GameObject.Instantiate(ui_invSlot, Vector3.zero, Quaternion.identity, ui_inv.transform.GetChild(3).transform);
                newSlot.GetComponent<RectTransform>().localPosition = slotStartVector + new Vector3((slotSize+RowsMargin) * j, (slotSize+ColumnsMargin) * i * -1, 0);
                newSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);

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

        int itemToswitch = itemsList[sID];
        itemsList[sID] = itemHand;
        itemHand = itemToswitch;
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
