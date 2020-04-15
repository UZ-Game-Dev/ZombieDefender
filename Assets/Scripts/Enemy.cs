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
    public Transform zombieFragments;

    [SerializeField]
    private float _health = 10f;
    [SerializeField]
    private float _attackSpeed = 1f;
    [SerializeField]
    private float _attackCooldownStart = 1f;
    public int minDamage = 2;
    public int maxDamage = 5;

    [Header("Definiowane dynamicznie")]
    public GameObject player;
    private Vector3 _target;
    private NavMeshAgent _agent;
    private bool _attack = false;
    private float _attackCooldown = 0f;

    private HealthUI _healthUI;
    private float _maxHP;

    [Header("Sounds")]
    //public GameObject audioSourceObject;
    public AudioSource audioSource;

    private GameObject _hitObject;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _healthUI = GetComponent<HealthUI>();
        _maxHP = _health;

        player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();

        //audioSource = audioSourceObject.GetComponent<AudioSource>();
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
        float speedPercent = _agent.velocity.magnitude / _agent.speed;
        _animator.SetFloat("speedPercent", speedPercent, 0.1f, Time.deltaTime);

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
            _animator.SetBool("isAttacking",true);
            _attack = true;
            _agent.isStopped = true;
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player") || other.tag.Equals("DefensiveObject"))
        {
            _animator.SetBool("isAttacking", false);
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
                int _takeDamage = Random.Range(minDamage, maxDamage);
                audioSource.Play();
                switch (_hitObject.tag)
                {
                    case "DefensiveObject":
                        _hitObject.transform.root.GetComponent<DefensiveObject>().TakeDamage(_takeDamage);
                        break;
                    case "Player":
                        player.gameObject.transform.GetChild(0).GetComponent<Player>().TakeDamage(_takeDamage);
                        break;
                }

                Debug.Log("*GRRR!* " + _hitObject.tag + " -" + _takeDamage);
                _attackCooldown = 2f / _attackSpeed;
            }
            yield return null;
        }
        _animator.SetBool("isAttacking", false);
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
        SoundsMenager.S.PlayZombieDeathSound();
        Instantiate(zombieFragments,new Vector3(transform.position.x, transform.position.y - 0.9702432f,transform.position.z),transform.rotation);
        Destroy(gameObject);
    }

    public void SetMaxHP(int hp)
    {
        _maxHP = hp;
        _health = hp;
    }

    public void SetSpeed(float speed)
    {
        NavMeshAgent navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        _attackSpeed = attackSpeed;
    }

    public void SetDamageOnHit(int minDamage, int maxDamage)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }
}