using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light light, source;
    private int _nextChange = 10, _changesCount=0;

    void FixedUpdate()
    {
        _nextChange--;
        if(_nextChange==0)
        {
            light.intensity = Random.Range(0.0f, 1.0f);
            source.intensity = (light.intensity - 0.55f) * (4f / 9f);
            _changesCount++;
            if (_changesCount == 6)
            {
                _nextChange = Random.Range(100, 150);
                _changesCount = 0;
            }
            else _nextChange = Random.Range(8, 20);
        }
    }
}
