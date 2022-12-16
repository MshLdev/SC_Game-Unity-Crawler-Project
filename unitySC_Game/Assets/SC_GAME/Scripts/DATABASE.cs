using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioMenager.clips;

public class DATABASE : MonoBehaviour
{

    public List<item> DATA_item;
    public HeroDB DATA_Hero;

    void Awake()
    {   
        initHero();
        initItems();
    }    

    public void initHero()
    {
        DATA_Hero = new HeroDB();
    }
    
    public void initItems()
    {
        DATA_item = new List<item>();
        DATA_item.Add(new item(0, "", "", "icons/f", false, AudioMenager.clips.NONE, uniqueness.COMMON));
        DATA_item.Add(new item(1, "Health Potion", "This magic drink made out of goblins blood and cave mushroom\n regenerates your health", "icons/hp", true, AudioMenager.clips.ui_splash, uniqueness.COMMON));
        DATA_item.Add(new item(2, "Mana Potion", "This magic drink is an actual tears of swamp hornet", "icons/mp", true, AudioMenager.clips.ui_splash, uniqueness.UNCOMMON));
        DATA_item.Add(new item(3, "Mleczny Człowiek's Drink", "This drink belongs to Mleczny Człowiek\n It stinks horribly and draws attention of near by rats", "icons/mcn", true, AudioMenager.clips.ui_splash, uniqueness.LEGENDARY));
        DATA_item.Add(new item(4, "Mleczny Człowiek's Apple", "This is Mleczny Człowiek's\n Personal Apple, he never leaves home without it\n what is it doing in your inventory?? o.0", "icons/apple", true, AudioMenager.clips.ui_squish, uniqueness.RARE));
    }

}


public struct item 
{
    
    public ushort               Id;
    public string               Name;
    public string               Desc;
    public bool                 Consumable;
    public Sprite               icon;
    public uniqueness           rarity;
    public AudioMenager.clips   sound;
    
    public item(ushort id, string name, string desc, string iconsource, bool cns, AudioMenager.clips s, uniqueness r)
    {
        this.Id         = id;
        this.Name       = name;
        this.Desc       = desc;
        this.icon       = Resources.Load<Sprite>(iconsource);
        this.Consumable = cns;
        this.sound = s;
        this.rarity = r;
    }
}


public class HeroDB
{
    public ushort level         = 1;
    public ushort exp           = 0;
    public ushort expNext       = 75;
    
    public ushort vitality      = 4;
    public ushort inteligence   = 9;
    public ushort strengh       = 2;
    public ushort agility       = 3;

    public barSystem health = new barSystem("health", 100, 100, 10);
    public barSystem mana = new barSystem("mana", 80, 80, 5);
}

//Finished structure, nothing to change now
//Everything about the health and mana bars in here
public struct barSystem
{
    public string name;

    public float value;
    public float maxvalue;
    public float regenvalue;

    public float targetValue;
    
    public barSystem(string n, float v, float mv, float rv)
    {
        this.name = n;

        this.value = v;
        this.maxvalue = mv;
        this.regenvalue = rv;

        this.targetValue = mv;
    }

    public void ValueToTarget()
    {
        if(this.targetValue > this.maxvalue)
            this.targetValue = this.maxvalue;
        this.value = Mathf.Lerp(this.value, this.targetValue, Time.deltaTime);
    }

    public void Regen()
    {
        if(this.value < this. maxvalue)
            this.targetValue = this.value + this.regenvalue;
        else
            return;
    }

    public void Deal(float ammount)
    {
        this.targetValue += ammount;
    }
}
//Finished structure, nothing to change now
//simple enum for rarity
public enum uniqueness
{
    COMMON      = 0,
    UNCOMMON    = 1,
    RARE        = 2,
    LEGENDARY   = 3, 
}


