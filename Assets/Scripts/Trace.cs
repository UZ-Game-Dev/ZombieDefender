using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    private float _speed = 180;
    public Vector3 waypoint;
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();

        var no = _ps.noise;
        no.enabled = true;
    }

    private void Start()
    {
        StartCoroutine("RenderTracer");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint,Time.deltaTime*_speed);
        Destroy(gameObject, _ps.main.startLifetimeMultiplier + 0.15f);
    }

    IEnumerator RenderTracer()
    {
        yield return new WaitForSeconds(0.2f);
        var no = _ps.noise;
        no.enabled = true;
    }
}