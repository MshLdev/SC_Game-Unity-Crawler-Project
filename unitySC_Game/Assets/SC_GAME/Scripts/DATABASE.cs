using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioMenager.clips;


public class DATABASE : MonoBehaviour
{
    public itemSlot itemHand;
    public List<itemSlot> itemsList;
    public List<item> DATA_item;            //list of all items in the game
    public HeroDB DATA_Hero;                //Player class

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
        DATA_item.Add(new item(0, "", "", "icons/f", AudioMenager.clips.NONE, uniqueness.COMMON));
        DATA_item.Add(new item(1, "Health Potion", "This magic drink made out of goblins blood and cave mushroom\n regenerates your health", "icons/hp", AudioMenager.clips.ui_splash, uniqueness.COMMON, stk: true));
        DATA_item.Add(new item(2, "Mana Potion", "This magic drink is an actual tears of swamp hornet", "icons/mp", AudioMenager.clips.ui_splash, uniqueness.UNCOMMON, stk: true));
        DATA_item.Add(new item(3, "Mleczny Człowiek's Drink", "This drink belongs to Mleczny Człowiek\n It stinks horribly and draws attention of near by rats", "icons/mcn", AudioMenager.clips.ui_splash, uniqueness.LEGENDARY, stk:true));
        DATA_item.Add(new item(4, "Mleczny Człowiek's Apple", "This is Mleczny Człowiek's\n Personal Apple, he never leaves home without it\n what is it doing in your inventory?? o.0", "icons/apple", AudioMenager.clips.ui_squish, uniqueness.RARE));
        itemHand = new itemSlot(DATA_item[0]);
    }

    public void fillItemList(int[] its)
    {
        itemsList = new List<itemSlot>();
        foreach (int item in its)
        {
            itemsList.Add(new itemSlot(DATA_item[item]));
        }
    }

    public item itemFromSlot(int index)
    {
        return DATA_item[itemsList[index].itemID];
    }

    public void useItem(itemSlot slot)
    {
       switch(slot.itemID)
        {
            case 1: //HEALTH POTION
                break;
            case 2: //HEALTH POTION
                DATA_Hero.mana.targetValue += 50;        ///Add + 50 regenration target
                if(slot.itemAmmount == 1)                    ///if this was our last potion
                    slot.itemID = 0;
                else
                    slot.itemAmmount --;
                break;
            default:
                break;
        }
    }



    /////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////


    public class itemSlot 
    {
        public int      itemID;         //id of item held by this slot
        public int      itemAmmount;    //ammount of items
        public int      AmmountCap;     //max ammount per stack

        public itemSlot(item i)
        {
            this.itemID =        i.Id;
            this.itemAmmount =   1;
            this.AmmountCap =    32;
        }
    }

    public struct item 
    {
        
        public int                  Id;         //not actually used by anything, might remove this later on
        public string               Name;
        public string               Desc;
        public bool                 Consumable;
        public bool                 stackable;
        public Sprite               icon;
        public uniqueness           rarity;
        public AudioMenager.clips   sound;
        
        public item(int id, string name, string desc, string iconsource, AudioMenager.clips s, uniqueness r, bool cns = false, bool stk = false)
        {
            this.Id         = id;       
            this.Name       = name;
            this.Desc       = desc;
            this.icon       = Resources.Load<Sprite>(iconsource);
            this.Consumable = cns;
            this.stackable  = stk;
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

}

