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
                    Instantiate(_itemList[j].item, _itemList[j].item.transform.position, _itemList[j].item.transform.rotation);
                    return;
                }
                randomValue -= _itemList[j].dropWeight;
            }
        }
    }
}
