using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{

    [Serializable]
    private class DropItem
    {
        public GameObject item=null;
        public int dropWeight=10;//posiadana szansa obiektu
    }

    [Header("Definiowane w panelu")]
    [SerializeField]
    private int _dropChance = 100;
    [SerializeField]
    private List<DropItem> _itemList = new List<DropItem>();

    public void randomItemDrop()
    {
        int calc_dropChance = UnityEngine.Random.Range(0, 101);

        if (calc_dropChance > _dropChance)
        {
            return;
        }

        if (calc_dropChance <= _dropChance)
        {
            int itemWeight = 0;

            for (int i = 0; i < _itemList.Count; i++)
            {
                itemWeight += _itemList[i].dropWeight;
            }

            int randomValue = UnityEngine.Random.Range(0, itemWeight);

            for (int j = 0; j < _itemList.Count; j++)
            {
                if (randomValue <= _itemList[j].dropWeight)
                {
                    GameObject items;

                    if (_itemList[j].item.name.Equals("First-AidItem"))
                    {
                        if ((float)Player.S.GetHP() / (float)Player.S.GetMaxHP() > 0.75f)
                        {
                            items = Instantiate(_itemList[0].item, this.transform.position, UnityEngine.Random.rotation) as GameObject;
                        }
                        else
                        {
                            items = Instantiate(_itemList[j].item, this.transform.position, UnityEngine.Random.rotation) as GameObject;
                        }
                    }
                    else
                    {
                        items = Instantiate(_itemList[j].item, this.transform.position, UnityEngine.Random.rotation) as GameObject;
                    }
                    items.GetComponent<Rigidbody>().AddForce(-this.transform.right * UnityEngine.Random.Range(100f, 125f) + this.transform.up * UnityEngine.Random.Range(0, 25));
                    
                    return;
                }
                randomValue -= _itemList[j].dropWeight;
            }
        }
    }
}
