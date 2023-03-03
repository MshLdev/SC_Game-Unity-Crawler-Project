using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_tooltip : MonoBehaviour
{
    Vector3 offset = new Vector3(0, -90, 0);
    public GameObject ui_tt;

    public Color32[] bgColors;
    public Color32[] txtColors;

    private string Tname = "";
    private string Tdsc = "";
    private bool Tsimple = false;

    public void Init()
    {
        ui_tt.SetActive(false); 
    }

    void Update()
    {
        ui_tt.transform.position = Input.mousePosition + offset;


        if (Tsimple)
            ui_tt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Tname;
        else if(!Input.GetKey(KeyCode.LeftAlt))
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
        Tsimple = false;

        ui_tt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = txtColors[bg];
        ui_tt.GetComponent<Image>().color = bgColors[bg];

        ui_tt.SetActive(true); 
    }

    public void DisplayTooltip(string name)
    {
        DisplayTooltipItem(name, "", 0);
        Tsimple = true;
        ui_tt.SetActive(true); 
    }

    public void DisableTooltip()
    {
        ui_tt.SetActive(false); 
    }
}
