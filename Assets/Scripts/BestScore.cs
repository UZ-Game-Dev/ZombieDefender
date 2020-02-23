using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textMeshPro = this.gameObject.GetComponent<TextMeshProUGUI>();
        int wave = PlayerPrefs.GetInt("wave");
        int bestWave = PlayerPrefs.GetInt("bestWave");

        textMeshPro.text = "Survived waves " + wave + "\nMost waves survived " + bestWave;
    }
}
