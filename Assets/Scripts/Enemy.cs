using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    [SerializeField]
    private float _health = 10f;

    [Header("Definiowane dynamicznie")]
    public GameObject player;
    private Vector3 _point;
    private NavMeshAgent _agent;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        GoToPoint();
    }

    public void GoToPoint ()
    {
        player = GameObject.Find("Player");
        _point = player.transform.position;

        _agent.SetDestination(_point);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    public void TakeDamage(float dmg)
    {
        _health -= dmg;

        if (_health <= 0)
        {
            Death();
            //SpawnItem();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}