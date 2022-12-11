using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATABASE : MonoBehaviour
{

    public List<item> DATA_item;

    ///call on script loadup
    void Awake()
    {
       
        initItems();
    }    
    
     //create items list
    public void initItems()
    {
        DATA_item = new List<item>();
        DATA_item.Add(new item(0, "Empty", "Empty", "icons/f", false));
        DATA_item.Add(new item(1, "Health Potion", "This magic drink made out of goblins blood and cave mushroom regenerates your health", "icons/hp", true ));
        DATA_item.Add(new item(2, "Mana Potion", "This magic drink is an actual tears of swamp hornet", "icons/mp", true ));
    }
}


public struct item 
{
   
    public int Id;
    public string Name;
    public string Desc;
    public Sprite icon;
    public bool Consumable;
   
    //Constructor (not necessary, but helpful)
    public item(int id, string name, string desc, string iconsource, bool cns)
    {
        this.Id         = id;
        this.Name       = name;
        this.Desc       = desc;
        this.icon       = Resources.Load<Sprite>(iconsource);
        this.Consumable = cns;
    }
}