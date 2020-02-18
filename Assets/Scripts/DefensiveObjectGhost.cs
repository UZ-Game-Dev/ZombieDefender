using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveObjectGhost : MonoBehaviour
{
    public static DefensiveObjectGhost S;

    [Header("Definiowane w panelu")]
    public Material materiaCollisionFalse;
    public Material materiaCollisionTrue;

    private float _actualDistance;
    private bool _collision=true;

    public void SetActualDistance(float actualDistance)
    {
        _actualDistance = actualDistance;
    }

    public bool GetCollision()
    {
        return _collision;
    }

    [System.Serializable]
    private class DefaultMaterial
    {
        public int numberChild { get; set; }
        public int numberMaterial { get; set; }
    }

    private List<DefaultMaterial> theDefaultMaterial = new List<DefaultMaterial>();

    private void Awake()
    {
        if (S != null)
            Debug.LogError("Sigleton DefensiveObjectGhost juz istnieje");
        S = this;

        for (int i1 = 0; i1 < this.transform.childCount; i1++)
        {
            for (int i2 = 0; i2 < this.transform.GetChild(i1).GetComponent<MeshRenderer>().materials.Length; i2++)
            {
                theDefaultMaterial.Add(new DefaultMaterial() { numberChild = i1, numberMaterial = i2});
            }
        }
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _actualDistance;
        Vector3 transformPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        this.transform.position = new Vector3(Mathf.Clamp(transformPosition.x, -2.25f, 6), transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 2 || collider.gameObject.layer == 8)
        {
            for (int i1 = 0; i1 < theDefaultMaterial.Count; i1++)
            {
                _collision = true;
                this.transform.GetChild(theDefaultMaterial[i1].numberChild).GetComponent<MeshRenderer>().materials[theDefaultMaterial[i1].numberMaterial].color = materiaCollisionTrue.color;
            }
        }
        else
        {
            for (int i1 = 0; i1 < theDefaultMaterial.Count; i1++)
            {
                _collision = false;
                this.transform.GetChild(theDefaultMaterial[i1].numberChild).GetComponent<MeshRenderer>().materials[theDefaultMaterial[i1].numberMaterial].color = materiaCollisionFalse.color;
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == 2 || collider.gameObject.layer == 8)
        {
            for (int i1 = 0; i1 < theDefaultMaterial.Count; i1++)
            {
                _collision = true;
                this.transform.GetChild(theDefaultMaterial[i1].numberChild).GetComponent<MeshRenderer>().materials[theDefaultMaterial[i1].numberMaterial].color = materiaCollisionTrue.color;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == 2 || collider.gameObject.layer == 8)
        {
            for (int i1 = 0; i1 < theDefaultMaterial.Count; i1++)
            {
                _collision = false;
                this.transform.GetChild(theDefaultMaterial[i1].numberChild).GetComponent<MeshRenderer>().materials[theDefaultMaterial[i1].numberMaterial].color = materiaCollisionFalse.color;
            }
        }
    }
}
