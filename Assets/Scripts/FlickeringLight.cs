using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public new Light light;
    public Light light2, source, source2;
    public MeshRenderer poswiata, poswiata2;
    private int[] _nextChange = { 18, 8}, _changesCount= { 0, 0 };

    void FixedUpdate()
    {
        _nextChange[0]--;
        _nextChange[1]--;

        if (_nextChange[0]==0)
        {
            light.intensity = Random.Range(0.0f, 1.0f);
            source.intensity = (light.intensity - 0.55f) * (4f / 9f);
            poswiata.material.color = new Color(1, 0.7070738f, 0, light.intensity* 0.2980392f);
            _changesCount[0]++;
            if (_changesCount[0] == 6)
            {
                _nextChange[0] = Random.Range(100, 150);
                _changesCount[0] = 0;
            }
            else _nextChange[0] = Random.Range(8, 20);
        }

        if (_nextChange[1] == 0)
        {
            light2.intensity = Random.Range(0.0f, 1.0f);
            source2.intensity = (light2.intensity - 0.55f) * (4f / 9f);
            poswiata2.material.color = new Color(1, 0.7070738f, 0, light2.intensity * 0.2980392f);
            _changesCount[1]++;
            if (_changesCount[1] == 6)
            {
                _nextChange[1] = Random.Range(100, 150);
                _changesCount[1] = 0;
            }
            else _nextChange[1] = Random.Range(8, 20);
        }
    }
}
