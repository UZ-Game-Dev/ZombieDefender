using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFragments : MonoBehaviour
{
    public bool objectFragmentsBox;

    private Rigidbody _rigidbody;
    private float _randomTime;
    private float _time = 0;

    private void Start()
    {
        _randomTime = Random.Range(4, 14);
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!objectFragmentsBox)
        {
            if (_time < _randomTime)
            {
                if (_rigidbody.velocity.magnitude < 0.05f)
                {
                    _rigidbody.isKinematic = true;
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.detectCollisions = false;
                    this.GetComponent<MeshCollider>().enabled = false;
                }

                _time += Time.deltaTime * 1;
            }
            else
            {
                _rigidbody.isKinematic = false;
                Destroy(gameObject, 0.4f);
            }
        }
        else
        {
            Destroy(gameObject, 14.5f);
        }
    }
}