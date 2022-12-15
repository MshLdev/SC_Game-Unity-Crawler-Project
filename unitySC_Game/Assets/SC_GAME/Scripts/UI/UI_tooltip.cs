using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_tooltip : MonoBehaviour
{
    Vector3 offset = new Vector3(0, -90, 0);
    GameObject ui_tt;

    public Color32[] bgColors;
    public Color32[] txtColors;

    private string Tname = "";
    private string Tdsc = "";

    void Awake()
    {
        ui_tt = GameObject.Find("UI_Tooltip");
        ui_tt.SetActive(false); 
    }

    void Update()
    {
        ui_tt.transform.position = Input.mousePosition + offset;

        if(!Input.GetKey(KeyCode.LeftAlt))
            ui_tt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Tname + "(Hold Alt)";
        else
            ui_tt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Tname + "\n\n" + Tdsc;
    }

    public void DisplayTooltipItem(string name, string desc, int bg)
    {
        if(name == "")
            return;

        Tname = name;
        Tdsc = desc;

        ui_tt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = txtColors[bg];
        ui_tt.GetComponent<Image>().color = bgColors[bg];

        ui_tt.SetActive(true); 
    }

    public void DisableTooltip()
    {
        ui_tt.SetActive(false); 
    }
}
