using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int maxEnemyCountOnLevel;
    public float waveDelay = 2f;
    public float spawnDelay = 5f;
}

public class Main : MonoBehaviour
{
    public static Main S;

    [Header("Definiowane w panelu inspekcyjnym")]
    public int gold = 5;
    public Level[] levelArray;
    public GameObject shopPanel;

    [Header("Definiowane dynamicznie")]
    public bool isEnableToShoot = true;
    public int countEnemy;
    public int currentLevel = 0;
    public bool isWaitingForNextWave = false;
    private Player _player;
    public IEnumerator shopCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        if (S != null)
            Debug.LogError("Sigleton Main juz istnieje");
        S = this;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.GetComponentInChildren<Player>();
    }

    private void Start()
    {
        shopCoroutine = EnableShop();
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if(countEnemy <= 0 && !isWaitingForNextWave)
        {
            StartCoroutine(shopCoroutine);
        }

        //Debug.Log("MAIN: LVL:" + currentLevel + "   ENEMY LEFT: " + countEnemy + "    COIN: " + gold + "   HP: " + _player.GetHP());
    }

    public void StopWaveCoroutine()
    {
        StopCoroutine(shopCoroutine);
        shopCoroutine = EnableShop();
        shopPanel.SetActive(false);
        isEnableToShoot = true;
        isWaitingForNextWave = false;
        currentLevel++;
        LoadLevel();
    }

    private IEnumerator EnableShop()
    {
        isWaitingForNextWave = true;
        isEnableToShoot = false;
        shopPanel.SetActive(true);
        shopPanel.GetComponent<Shop>().SetTimer(levelArray[currentLevel].waveDelay);
        yield return new WaitForSeconds(levelArray[currentLevel].waveDelay);
        StopWaveCoroutine();
    }

    public void PickUpItem(eIteamsType type)
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
                _player.Heal(25);
                break;

            default:
                Debug.Log("Nierozpoznany obiekt " + type);
                break;
        }
    }

    void LoadLevel()
    {
        if (currentLevel >= levelArray.Length)
            currentLevel = levelArray.Length - 1;

        countEnemy = levelArray[currentLevel].maxEnemyCountOnLevel;
        Spawner.S.SpawnStart(levelArray[currentLevel].maxEnemyCountOnLevel);
    }
}
