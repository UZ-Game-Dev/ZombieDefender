using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner S;

    [Header("Definiowane w panelu inspekcyjnym")]
    public GameObject[] spawnerPoints;
    public GameObject[] enemyPrefabs;
    public float spawnDelay = 5f;

    [Header("Definiowane dynamicznie")]
    public float bornTime = 0;
    public bool isEnableToSpawn = true;

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
        StartCoroutine(SpawnZombie(sum));
    }

    IEnumerator SpawnZombie(int sum)
    {
        for (int i = 0; i < sum; i++)
        {
            Spawn(0);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Spawn(int enemyIndex)
    {
        int randomPosition = Random.Range(0, spawnerPoints.Length-1);
        GameObject obj = Instantiate<GameObject>(enemyPrefabs[enemyIndex]);

        Transform objTransform = obj.GetComponent<Transform>();
        objTransform.position = spawnerPoints[randomPosition].GetComponent<Transform>().position;

        Main.S.countEnemy++;
    }

    //Rysuje przestrzeń w której będzie odbywać się gra
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.DrawWireCube(new Vector3(0,2.5f,0), new Vector3(10,5, 5));
    }
}
