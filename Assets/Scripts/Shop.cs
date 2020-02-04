using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public void NextWave()
    {
        Debug.Log("NEXT WAVE");
        Main.S.StopWaveCoroutine();
    }
}
