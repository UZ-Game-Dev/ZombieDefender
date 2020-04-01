using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int health;
    public int gold;
    public int currentLevel;
    //public DefensiveObject defObject;
    //public DefensiveSpikes defSpikes;
   // public Weapon weapon;

    public SaveData(int HP, Main main, Shop shop, Weapon _weapon)
    {
        //defSpikes = shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>();
        //defObject = shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>();
        //weapon = _weapon;
        health = HP;
        gold = main.gold;
        currentLevel = main.currentLevel;
    }
}
