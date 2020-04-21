using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int maxEnemyCountOnLevel;
    public float waveDelay = 2f;
    public Vector2 minMaxSpawnDelay = new Vector2(1,5);
    public Vector2 minMaxZombieSpeed = new Vector2(1, 3);
    public int hpZombie = 5;
}

public class Main : MonoBehaviour
{
    public static Main S;

    [Header("Definiowane w panelu inspekcyjnym")]
    public int gold = 5;
    public Level[] levelArray;
    public GameObject shopPanel;
    public Texture2D cursorCrosshairs;

    [Header("Definiowane dynamicznie")]
    public bool isEnableToShoot = true;
    public int countEnemy;
    public int currentLevel = 0;
    public bool isWaitingForNextWave = false;
    private Player _player;
    private Weapon _weapon;
    public IEnumerator shopCoroutine;
    public int waveCounter;

    

    // Start is called before the first frame update
    void Awake()
    {
        if (S != null)
            Debug.LogError("Sigleton Main juz istnieje");
        S = this;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.GetComponentInChildren<Player>();
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        _weapon = weapon.GetComponentInChildren<Weapon>();
    }

    private void Start()
    {
        shopCoroutine = EnableShop();
        if (SaveSystem.isGameLoaded)
        {
            //Player
            Player.S.OnLoadGame();
            //Global
            gold = SaveSystem.GetData().gold;
            waveCounter = SaveSystem.GetData().currentWave;
            currentLevel = SaveSystem.GetData().currentLevel;
            //Weapons
            _weapon.OnLoadLevel();

            //Defensive Objects
            Shop shop = this.GetComponent<Shop>();
            shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>().currentLevel = SaveSystem.GetData().FencyCurrentLevel;
            shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>().health = SaveSystem.GetData().FencyHealth;
            shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>().upgradePrice = SaveSystem.GetData().FencyUpgradePrice;

            shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().currentLevel = SaveSystem.GetData().SpikeCurrentLevel;
            shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().health = SaveSystem.GetData().SpikeHealth;
            shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().damageEnemy = SaveSystem.GetData().SpikeDamageEnemy;
            shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().upgradePrice = SaveSystem.GetData().SpikeUpgradePrice;
            shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().takeDamage = SaveSystem.GetData().SpikeTakeDamage;

            foreach (ObjectData value in SaveSystem.GetData().objectOnMap)
            {
                if (value.tag == "DefensiveObject")
                {
                    GameObject gameObject = Instantiate(shop.DefensiveObjectsArray[1].prefabs);
                    gameObject.transform.position = new Vector3(value.x, value.y, value.z);
                    gameObject.GetComponent<DefensiveObject>().health = value.Health;
                    gameObject.transform.SetParent(shop.objectAnchor, true);
                    gameObject.SetActive(true);
                }
                if (value.tag == "DefensiveSpikes")
                {
                    GameObject gameObject = Instantiate<GameObject>(shop.DefensiveObjectsArray[0].prefabs);
                    gameObject.transform.position = new Vector3(value.x, value.y, value.z);
                    gameObject.GetComponent<DefensiveSpikes>().health = value.Health;
                    gameObject.transform.SetParent(shop.objectAnchor, true);
                    gameObject.SetActive(true);
                }
            }
        }
        else
        {
            LoadLevel();
            waveCounter = currentLevel + 1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnableToShoot && !PauseMenu.S.GetIsPaused()) Cursor.SetCursor(cursorCrosshairs, new Vector2(cursorCrosshairs.width / 2, cursorCrosshairs.height / 2), CursorMode.ForceSoftware);
        else Cursor.SetCursor(null, Vector2.zero.normalized, CursorMode.ForceSoftware);

        if (countEnemy <= 0 && !isWaitingForNextWave)
        {
            isWaitingForNextWave = true;
            StartCoroutine(AreThereItems());
        }

        //Debug.Log("MAIN: LVL:" + currentLevel + "   ENEMY LEFT: " + countEnemy + "    COIN: " + gold + "   HP: " + _player.GetHP());
    }

    private IEnumerator AreThereItems()
    {
        while (true)
        {
            Items[] _gos = FindObjectsOfType(typeof(Items)) as Items[];

            if (_gos.Length == 0)
            {
                SaveGame();
                StartCoroutine(shopCoroutine);
                break;
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }

    public void StopWaveCoroutine()
    {
        StopCoroutine(shopCoroutine);
        shopCoroutine = EnableShop();
        shopPanel.SetActive(false);
        isEnableToShoot = true;
        isWaitingForNextWave = false;
        currentLevel++;
        waveCounter++;
        UI.S.wave.text= "Wave: " + (int)(Main.S.waveCounter);
        LoadLevel();
    }

    private IEnumerator EnableShop()
    {
        UI.S.SetAmmoTexts();

        isWaitingForNextWave = true;
        isEnableToShoot = false;
        shopPanel.SetActive(true);
        this.GetComponent<Shop>().SetTimer(levelArray[currentLevel].waveDelay);
        yield return new WaitForSeconds(levelArray[currentLevel].waveDelay);
        
        UI.S.gold.text = "Gold: " + Main.S.gold;
        StopWaveCoroutine();
        
       
    }

    public void PickUpItem(eIteamsType type)
    {
        switch(type)
        {
            case eIteamsType.eCoin:
                {
                    if (currentLevel < 5) gold += 3;
                    else gold += 5;

                    UI.S.gold.text = "Gold: " + Main.S.gold;

                    break;
                }
            case eIteamsType.eGoldBar:
                gold += 8;
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

    private void SaveGame()
    {
        Shop shopObject = Camera.main.GetComponent<Shop>();
        GameObject gameObject = GameObject.FindGameObjectWithTag("Weapon");
        Weapon weapon = gameObject.GetComponent<Weapon>();
        SaveSystem.SaveGame(Player.S.GetHP(), Main.S, shopObject, weapon);
    }
}
