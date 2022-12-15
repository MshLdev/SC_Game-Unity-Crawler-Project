using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioMenager.clips;

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
        DATA_item.Add(new item(0, "", "", "icons/f", false, AudioMenager.clips.NONE, uniqueness.COMMON));
        DATA_item.Add(new item(1, "Health Potion", "This magic drink made out of goblins blood and cave mushroom\n regenerates your health", "icons/hp", true, AudioMenager.clips.ui_splash, uniqueness.COMMON));
        DATA_item.Add(new item(2, "Mana Potion", "This magic drink is an actual tears of swamp hornet", "icons/mp", true, AudioMenager.clips.ui_splash, uniqueness.UNCOMMON));
        DATA_item.Add(new item(3, "Mleczny Człowiek's Drink", "This drink belongs to Mleczny Człowiek\n It stinks horribly and draws attention of near by rats", "icons/mcn", true, AudioMenager.clips.ui_splash, uniqueness.LEGENDARY));
        DATA_item.Add(new item(4, "Mleczny Człowiek's Apple", "This is Mleczny Człowiek's\n Personal Apple, he never leaves home without it\n what is it doing in your inventory?? o.0", "icons/apple", true, AudioMenager.clips.ui_squish, uniqueness.RARE));
    }


    public struct item 
    {
    
        public int                  Id;
        public string               Name;
        public string               Desc;
        public bool                 Consumable;
        public Sprite               icon;
        public uniqueness           rarity;
        public AudioMenager.clips   sound;
    
        //Constructor (not necessary, but helpful)
        public item(int id, string name, string desc, string iconsource, bool cns, AudioMenager.clips s, uniqueness r)
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

    public enum uniqueness
    {
        COMMON      = 0,
        UNCOMMON    = 1,
        RARE        = 2,
        LEGENDARY   = 3, 
    }
}


