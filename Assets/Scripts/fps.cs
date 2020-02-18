using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fps : MonoBehaviour {
	private float _frameCount;
	private float _dt;
	private float _fps;
	private float _updateRate = 4.0f;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = this.GetComponent<TextMeshProUGUI>();
    }

    void Update (){
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (_text.color.a != 0)
            {
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
            }
            else
            {
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
            }
        }

        if (_text.color.a != 0)
        {
            _frameCount++;
            _dt += Time.deltaTime;
            if (_dt > 1.0f / _updateRate)
            {
                _fps = _frameCount / _dt;
                _frameCount = 0;
                _dt -= 1.0f / _updateRate;
            }

            _text.text = _fps.ToString("0");
        }
	}
}
