using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///SKRYPCIK EKWIPINKU
///
///
///
public class UI_Inventory : MonoBehaviour
{
    
    public GameObject ui_invSlot;

    public int numberOfRows;
    public int numberOfColumns;

    public float RowsMargin;
    public float ColumnsMargin;

    public float slotSize;
    public Vector3 slotStartVector = new Vector3 (-125, 180, 0);
    
    private GameObject ui_inv;
    private AudioMenager audioM;

    void Start()
    {
        ui_inv = GameObject.Find("UI_Inventory");
        audioM = GameObject.Find("_GAME").GetComponent<AudioMenager>();
        audioM.AudioAtPlayer(5);

        InitSlots(numberOfColumns, numberOfRows);
        switchCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
            switchCursor();
    }

    void InitSlots(int numX, int numY)
    {
        

        for(int i = 0; i < numX; i++)
        {
            for(int j = 0; j < numY; j++)
            {
                GameObject newSlot = GameObject.Instantiate(ui_invSlot, Vector3.zero, Quaternion.identity, ui_inv.transform.GetChild(3).transform);
                newSlot.GetComponent<RectTransform>().localPosition = slotStartVector + new Vector3((slotSize+RowsMargin) * j, (slotSize+ColumnsMargin) * i * -1, 0);
                newSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize, slotSize);
            }
        }
    }

    void switchCursor()
    {
        Cursor.visible = !Cursor.visible;
        ui_inv.SetActive(!ui_inv.activeSelf);

        if(Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            audioM.AudioAtPlayer(4);
        }
            
        else
        {
            Cursor.lockState = CursorLockMode.Locked; 
            audioM.AudioAtPlayer(4);
        }
            
    }
}
