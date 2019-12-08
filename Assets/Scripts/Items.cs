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

    [Header("Definiowane dynamicznie")]
    public float bornTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        bornTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (bornTime + lifeTime < Time.time)
            Destroy(this.gameObject);
    }
}
