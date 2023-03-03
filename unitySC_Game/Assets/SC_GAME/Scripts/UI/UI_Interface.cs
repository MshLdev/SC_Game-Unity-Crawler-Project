using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static DATABASE.itemSlot;
using UnityEngine.SceneManagement;


///SKRYPCIK EKWIPINKU
/// od WERSJI v0.6 TEN SKRYPT ROBI SPORO, PIERWOTNE ZAŁOŻENIE POSZŁO SIE JEBAŚ
///

public class UI_Interface: MonoBehaviour
{
    //Slot Prefab
    public GameObject ui_invSlot;
    //hand Icon
    public Animator hitmark;
    public Image ui_handImage;
    public Sprite crosshair;
    public Sprite hotbar_Idle;
    public Sprite hotbar_Active;
    private Color crosshairColor = Color.cyan;
    //eq Size 
    public int numberOfX;
    public int numberOfY;
    //Margins
    public float RowsMargin;
    public float ColumnsMargin;
    //Size of an icon
    public float slotSize;
    //Inventory start
    public Vector3 slotStartPosition= new Vector3 (-125, 180, 0);

    ///Hotbar, needs refactor, Monolith workflow
    public int currentSlotid = 0;
    public int currentSpellId = 0;

    public  float           spellcost = -7.5f;
    public  GameObject[]    SpellBook;
    private GameObject      UI_Object;
    private GameObject      ui_inv;
    
    //depends on
    private UI_tooltip          ui_tooltip;
    private AudioMenager        audioM;
    private DATABASE            db;

    //timers
    float cooldown_hitmark = 0f;

    public void Create()
    {
        ui_tooltip =    gameObject.GetComponent<UI_tooltip>();        //For UI elements to spawn
        audioM =        gameObject.GetComponent<AudioMenager>();      //for Audio
        db =            gameObject.GetComponent<DATABASE>();          //for game db

        ui_inv =        GameObject.Find("UI_Slots");                  //For UI elements to spawn
        UI_Object =     GameObject.Find("UI_Interface");

        int[] tmpList = new int[numberOfX * numberOfY + 10];           //start ItemArray with empty slots
        db.fillItemList(tmpList);

        //add starting items
        startingItems();
        //InitTheUI
        InitHotbarSlots();
        InitSlots(numberOfX, numberOfY);
        //Switch On/Off
        switchInventory();
        pickSlot(1);
    }

    void Update()
    {   
        updateBar(1, db.DATA_Hero.mana.value, db.DATA_Hero.mana.maxvalue);
        updateBar(0, db.DATA_Hero.health.value, db.DATA_Hero.health.maxvalue);
        db.DATA_Hero.mana.ValueToTarget();
        db.DATA_Hero.health.ValueToTarget();    

        inputHotbar();
        inputInventory();
        updateHand();
        updateCrosshairColor();

        cooldown_hitmark += Time.deltaTime;
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
            EnterEvent.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); ui_tooltip.DisplayTooltipItem(db.itemFromSlot(Slot).Name, db.itemFromSlot(Slot).Desc , (int)db.itemFromSlot(Slot).rarity); } );
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
                GameObject newSlot = GameObject.Instantiate(ui_invSlot, ui_inv.transform);
                newSlot.GetComponent<RectTransform>().localPosition = slotStartPosition + new Vector3((slotSize+RowsMargin) * j, (slotSize+ColumnsMargin) * i * -1, 0);
                newSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
                //Also resize the Icon GameObject
                newSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
                newSlot.name = "eq_slot["+ currID +"]";
                
                ///set an actual icon
                updateSlot(currID);

                ////EVENTYY
                /////AUDIO jako event trigger (SKOPIOWANE PROSTO Z DOKUMENTACJI, TROCHE DZIWNE UGUŁEM)
                EventTrigger trigger = newSlot.GetComponent<EventTrigger>();

                EventTrigger.Entry EnterEvent = new EventTrigger.Entry();
                EnterEvent.eventID = EventTriggerType.PointerEnter;
                EnterEvent.callback.AddListener( (eventData) => { audioM.AudioAtPlayer(AudioMenager.clips.ui_hover); ui_tooltip.DisplayTooltipItem(db.itemFromSlot(currID).Name, db.itemFromSlot(currID).Desc , (int)db.itemFromSlot(currID).rarity); } );

                EventTrigger.Entry ExitEvent = new EventTrigger.Entry();
                ExitEvent.eventID = EventTriggerType.PointerExit;
                ExitEvent.callback.AddListener( (eventData) => { ui_tooltip.DisableTooltip(); } );

                trigger.triggers.Add(EnterEvent);
                trigger.triggers.Add(ExitEvent);

                /////ON CLICK
                newSlot.GetComponent<Button>().onClick.AddListener(delegate {slotClicked(currID); });;
                ////EVENTY/
            }
        }
    }

    void inputInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
            switchInventory();
        
        if(Input.GetKeyDown(KeyCode.Escape))
            if(ui_inv.transform.parent.parent.gameObject.activeInHierarchy)
                switchInventory();
            else
                SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }

    ///do refaktoru, kto to narzigoł w taki sposób, zagryza
    void switchInventory()
    {
        Cursor.visible = !Cursor.visible;
        //using Parent so the background also goes offline(becouse I cant get Find() to work properly)
        ui_inv.transform.parent.parent.gameObject.SetActive(!ui_inv.transform.parent.parent.gameObject.activeSelf);
        
        //Do if we are in UI mode
        if(Cursor.visible)
        {
            ui_handImage.color = Color.white;
            ui_handImage.sprite = db.DATA_item[db.itemHand.itemID].icon; //change the crosshair to item in hand(should be 'None' at this point o.0)
            Cursor.lockState = CursorLockMode.None;
            audioM.AudioAtPlayer(AudioMenager.clips.ui_close);
        }
        //Or else Do that    
        else
        {
            ui_handImage.color = Color.cyan;
            ui_handImage.sprite = crosshair; //change back to the crosshair
            Cursor.lockState = CursorLockMode.Locked; 
            audioM.AudioAtPlayer(AudioMenager.clips.ui_close);
        }
            
        //Exit event is not called(I Assume thats becouse the GO is being disabled before it can react) so we call it manually here
        ui_tooltip.DisableTooltip();
    }

    void startingItems()
    {
        pickupItem(1);   // add hp potiom
        pickupItem(2);   // add hp potiom
        pickupItem(3);   // add hp potiom
        pickupItem(1);   // add hp potiom
        pickupItem(1);   // add hp potiom
        pickupItem(1);   // add hp potiom
        pickupItem(2);   // add hp potiom
        pickupItem(1);   // add hp potiom
        pickupItem(3);   // add hp potiom
        pickupItem(4);   // add hp potiom
        pickupItem(3);   // add hp potiom
        pickupItem(3);   // add hp potiom
        pickupItem(3);   // add hp potiom
        pickupItem(4);   // add hp potiom
        pickupItem(4);   // add hp potiom
    }
  
    void pickupItem(int iID)
    {
        if(db.DATA_item[iID].stackable)
            for( int i = 0 ; i < db.DATA_Inventory.Count; i++)
            {   
                if(db.DATA_Inventory[i].itemID == iID)
                    if(db.DATA_Inventory[i].itemAmmount < db.DATA_Inventory[i].AmmountCap)
                    {
                        db.DATA_Inventory[i].itemAmmount++;
                        updateSlot(i);
                        return;
                    }
            }

        for( int i = 0 ; i < db.DATA_Inventory.Count; i++)
        {   
            if(db.DATA_Inventory[i].itemID == 0)
                {
                    //if item is 0, it should have default values, that is, id, 1, 32(v0.6a)
                    db.DATA_Inventory[i].itemID = iID;
                    updateSlot(i);
                    return;
                } 
        }

    }

    public void slotClicked(int Slot)
    {
        audioM.AudioAtPlayer(db.itemFromSlot(Slot).sound);
        //change id's
        DATABASE.itemSlot itemToswitch = db.DATA_Inventory[Slot];
        db.DATA_Inventory[Slot] = db.itemHand;
        db.itemHand = itemToswitch;
        updateSlot(Slot);
        ui_handImage.sprite = db.DATA_item[db.itemHand.itemID].icon;
    }

    void updateSlot(int Slot)
    {
        ui_inv.transform.GetChild(Slot).GetChild(0).GetComponent<Image>().sprite = db.itemFromSlot(Slot).icon;
        if(Slot < 10)
            UI_Object.transform.GetChild(2).GetChild(Slot).GetChild(0).GetComponent<Image>().sprite = db.itemFromSlot(Slot).icon;

        updateSlotAmmount(Slot);
    }

    void updateSlotAmmount(int Slot)
    {

        string ammount = "";

        if (db.DATA_Inventory[Slot].itemID != 0)
            {
                if(db.itemFromSlot(Slot).stackable == false)
                    ammount = "~";
                else
                    ammount += "" + db.DATA_Inventory[Slot].itemAmmount;
            }
        
        ui_inv.transform.GetChild(Slot).GetChild(1).GetComponent<TextMeshProUGUI>().text = ammount;

        if(Slot < 10)
            UI_Object.transform.GetChild(2).GetChild(Slot).GetChild(2).GetComponent<TextMeshProUGUI>().text = ammount;
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
    void inputHotbar()
    {
        if(Input.inputString != "")
        {
            int selector;
            if (int.TryParse(Input.inputString, out selector))
                pickSlot(selector);
        }

        ///Using Item
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            db.useItem(db.DATA_Inventory[currentSlotid]);
            updateSlot(currentSlotid);
        }
    }

    void pickSlot(int Slot)
    {   
        if(Slot == currentSlotid)
            return;

        audioM.AudioAtPlayer(AudioMenager.clips.ui_select);
        audioM.AudioAtPlayer(db.itemFromSlot(Slot).sound);
        UI_Object.transform.GetChild(2).GetChild(currentSlotid).GetComponent<Image>().sprite = hotbar_Idle;
        UI_Object.transform.GetChild(2).GetChild(Slot).GetComponent<Image>().sprite = hotbar_Active;
        currentSlotid = Slot;
        
        ////use item
        db.selectItem(db.DATA_Inventory[Slot]);
        //db.useItem(db.DATA_Inventory[Slot]);
        //updateSlot(Slot);//we might have run out of item

    }

    void crosshairEnemy(bool isEnemy)
    {
        if(isEnemy)
            crosshairColor = Color.red;
        else
            crosshairColor = Color.cyan;
    }

    void updateCrosshairColor()
    {
        ui_handImage.color = Color.Lerp(ui_handImage.color, crosshairColor, 10 * Time.deltaTime);
    }

    void hitMarkPlay(Color color)
    {
        if(cooldown_hitmark < 0.3f)
            return;

        cooldown_hitmark = 0f;
        hitmark.Play("hitmark_hit");
        audioM.AudioAtPlayer(AudioMenager.clips.ui_hitmark, 5f);
    }

    ///////////////////////////////////////////////////
    //// HEALTH/MANA BARS

    public void updateBar(int barID, float value, float maxvalue)
    {
        UI_Object.transform.GetChild(barID).GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(value/maxvalue, 1f, 1f);  
        UI_Object.transform.GetChild(barID).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{(int)value}/{(int)maxvalue}";
    }


    ////////////////////////////
    ////Events

    private void OnEnable()
    {
        // Subscribe to the OnSomethingFound event
        Enemy.MouseOnEnemy  += crosshairEnemy;
        Enemy.EnemyHit      += hitMarkPlay;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnSomethingFound event
        Enemy.MouseOnEnemy  -= crosshairEnemy;
        Enemy.EnemyHit      -= hitMarkPlay;
    }

}

