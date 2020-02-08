using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RandomItem))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HealthUI))]
public class Enemy : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    public LayerMask mask;

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

    private HealthUI _healthUI;
    private float _maxHP;

    private GameObject _hitObject;

    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();
        _maxHP = _health;

        player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    public void GoToPlayer()
    {
        if (player != null)
        {
            _target = player.transform.position;
            _agent.SetDestination(_target);
            _agent.isStopped = false;
        }
        else Debug.Log("Nie znaleziono gracza!");
    }

    void Update()
    {
        if (!_attack)
        {
            GoToPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_attack && (other.tag.Equals("Player") || other.tag.Equals("DefensiveObject")))
        {
            _hitObject = other.transform.gameObject;
            _attack = true;
            _agent.isStopped = true;
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player") || other.tag.Equals("DefensiveObject"))
        {
            _attack = false;
            _hitObject = null;
        }
    }

    private IEnumerator Attack()
    {
        _attackCooldown = _attackCooldownStart;

        while (_attack && _hitObject != null)
        {
            _attackCooldown -= Time.deltaTime;

            if (_hitObject != null && _attackCooldown <= 0f)
            {
                int _takeDamage = Random.Range(2, 5);

                switch (_hitObject.tag)
                {
                    case "DefensiveObject":
                        //_hitObject.transform.root.GetComponent<DefensiveObject>().TakeDamage(_takeDamage);
                        break;
                    case "Player":
                        player.gameObject.transform.GetChild(0).GetComponent<Player>().TakeDamage(_takeDamage);
                        break;
                }

                Debug.Log("*GRRR!* " + _hitObject.tag + " -" + _takeDamage);
                _attackCooldown = 1f / _attackSpeed;
            }
            yield return null;
        }
        _attack = false;
    }

    public void TakeDamage(float dmg)
    {
        _health -= dmg;
        _healthUI.updateHP(_health, _maxHP);

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