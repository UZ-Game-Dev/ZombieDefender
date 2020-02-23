﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner S;

    [Header("Definiowane w panelu inspekcyjnym")]
    public GameObject[] spawnerPoints;
    public GameObject[] enemyPrefabs;

    public Vector3 gameAreaPosition;
    public Vector3 gameAreaScale;

    [Header("Definiowane dynamicznie")]
    public float bornTime = 0;
    public bool isEnableToSpawn = true;
    public IEnumerator corutine;

    // Start is called before the first frame update
    void Awake()
    {
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnStart(int sum)
    {
        corutine = SpawnZombie(sum);
        StartCoroutine(corutine);
    }

    public IEnumerator SpawnZombie(int sum)
    {
        for (int i = 0; i < sum; i++)
        {
            Spawn(Random.Range(0, enemyPrefabs.Length));
            float range = Random.Range(Main.S.levelArray[Main.S.currentLevel].minMaxSpawnDelay.x, Main.S.levelArray[Main.S.currentLevel].minMaxSpawnDelay.y);
            yield return new WaitForSeconds(range);
        }
        //StopCoroutine(corutine);
    }

    private void Spawn(int enemyIndex)
    {
        int randomPosition = Random.Range(0, spawnerPoints.Length-1);
        GameObject obj = Instantiate<GameObject>(enemyPrefabs[enemyIndex]);

        Transform objTransform = obj.GetComponent<Transform>();
        objTransform.position = spawnerPoints[randomPosition].GetComponent<Transform>().position;

        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.SetMaxHP(Main.S.levelArray[Main.S.currentLevel].hpZombie);
        float range = Random.Range(Main.S.levelArray[Main.S.currentLevel].minMaxZombieSpeed.x, Main.S.levelArray[Main.S.currentLevel].minMaxZombieSpeed.y);
        enemy.SetSpeed(range);

        //Main.S.countEnemy++; //Nie potrzebne 
    }

    //Rysuje przestrzeń w której będzie odbywać się gra
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.DrawWireCube(gameAreaPosition, gameAreaScale);
    }
}
