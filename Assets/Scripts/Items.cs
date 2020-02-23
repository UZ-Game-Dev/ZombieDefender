using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eIteamsType
{
    none,
    eCoin,
    eGoldBar,
    eLife
}

[RequireComponent(typeof(Outline))]
public class Items : MonoBehaviour
{
    [Header("Definiowane w panelu inspekcyjnym")]
    public eIteamsType type = eIteamsType.none;
    public float lifeTime = 10;
    public float speedProgress = 1f;

    private Image _progressbarItems;
    private Outline _itemOutlone;
    private float _progress = 0;
    private bool _onMouseDownOn;

    private float time=0;

    void Start()
    {
        _progressbarItems = GameObject.FindGameObjectWithTag("ProgressbarItems").GetComponent<Image>();
        _progressbarItems.enabled = false;
        _itemOutlone = GetComponent<Outline>();
        _itemOutlone.enabled = false;
    }
    
    void Update()
    {
        time += 1 * Time.deltaTime;

        if (_onMouseDownOn)
        {
            _progressbarItems.fillAmount = _progress;
            _progressbarItems.transform.position = Input.mousePosition;
            _progress +=  Time.deltaTime * 1 / speedProgress;

            if (_progress > 1.1)
            {
                _progressbarItems.enabled = false;
                Debug.Log("Zebrałem: " + type);
                SoundsMenager.S.PlayItemPickedup();
                Main.S.PickUpItem(type);
                Destroy(this.gameObject);
            }
        }

        if (time >= lifeTime)
        {
            if(_onMouseDownOn) _progressbarItems.enabled = false;
            this.gameObject.transform.GetChild(0).GetComponent<Outline>().enabled = false;
            this.GetComponent<MeshCollider>().isTrigger = true;
            Destroy(this.gameObject,0.2f);
        }
        
    }

    private void OnMouseEnter()
    {
        _itemOutlone.enabled = true;
        _progressbarItems.enabled = true;
        _onMouseDownOn = true;
    }

    private void OnMouseExit()
    {
        _itemOutlone.enabled = false;
        _progressbarItems.enabled = false;
        _onMouseDownOn = false;
        _progress = 0;
    }
}
