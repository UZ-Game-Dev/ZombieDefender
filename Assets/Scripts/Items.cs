using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eIteamsType
{
    none,
    eCoin,
    eGoldBar,
    eLife
}


public class Items : MonoBehaviour
{
    [Header("Definiowane w panelu inspekcyjnym")]
    public eIteamsType type = eIteamsType.none;
    public float lifeTime = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, lifeTime);
    }

    private void OnMouseDown()
    {
        //Main.S.PickUpItem(type);
        Destroy(this.gameObject);
    }
}
