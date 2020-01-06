using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    //TEMP
    public enum eIteamsType
    {
        none,
        eCoin,
        eGoldBar,
        eLife
    }
    //END TEMP

    public static Main S;

    [Header("Definiowane w panelu inspekcyjnym")]
    public int gold = 5;
    public float waveDelay = 2f;

    [Header("Definiowane dynamicznie")]
    public int countEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        if (S != null)
            Debug.LogError("Sigleton Main juz istnieje");
        S = this;        
    }

    // Update is called once per frame
    void Update()
    {
        WaveMenagment();
    }

    void PickUpItem(eIteamsType type)
    {
        switch(type)
        {
            case eIteamsType.eCoin:
                gold += 1;
                break;

            case eIteamsType.eGoldBar:
                gold += 10;
                break;

            case eIteamsType.eLife:
                //Player.HP += 5;
                break;

            default:
                Debug.Log("Nierozpoznany obiekt " + type);
                break;
        }
    }

    void WaveMenagment()
    {
        if (countEnemy <= 0)
            Spawner.S.isEnableToSpawn = true;
        if (countEnemy >= 5) //Zamiast 5 liczba przeciwnikow z danego poziomu
            Spawner.S.isEnableToSpawn = false;
    }
}
