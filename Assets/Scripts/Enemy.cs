using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RandomItem))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    [SerializeField]
    private float _health = 10f;
    [SerializeField]
    private float _attackSpeed = 1f;
    [SerializeField]
    private float _attackCooldownStart = 1f;

    [Header("Definiowane dynamicznie")]
    public GameObject player;
    private Vector3 _target;
    private NavMeshAgent _agent;
    private bool _attack = false;
    private float _attackCooldown = 0f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        GoToPoint();
    }

    public void GoToPoint ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _target = player.transform.position;
            _agent.SetDestination(_target);
        }
        else Debug.Log("Nie znaleziono gracza!");
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.name);
        if (collider.tag.Equals("Player"))
        {
            _attack = true;
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            _attack = false;
        }
    }

    private IEnumerator Attack()
    {
        _agent.isStopped = true;
        _attackCooldown = _attackCooldownStart;

        while (_attack)
        {
            _attackCooldown -= Time.deltaTime;

            if (_attackCooldown <= 0f)
            {
                int _takeDamage = Random.Range(2, 5);
                player.gameObject.transform.GetChild(0).GetComponent<Player>().TakeDamage(_takeDamage);
                Debug.Log("*AAŁA* Kurwa gryzie! -" + _takeDamage);
                _attackCooldown = 1f / _attackSpeed;
            }
            yield return null;
        }
    }

    public void TakeDamage(float dmg)
    {
        _health -= dmg;

        if (_health <= 0)
        {
            Death();
            GetComponent<RandomItem>().randomItemDrop();
        }
    }

    public void Death()
    {
        Main.S.countEnemy--;
        Destroy(gameObject);
    }
}