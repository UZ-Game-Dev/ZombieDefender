using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveSpikes : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    [SerializeField]
    private float _health = 3f;
    [SerializeField]
    private float _demageEnemy = 10;
    [SerializeField]
    private float _TakeDamage = 1;

    private HealthUI _healthUI;
    private float _maxHP;

    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();
        _maxHP = _health;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.transform.root.GetComponent<Enemy>())
        {
            StartCoroutine(SpikesUp(_collider));
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }

    private IEnumerator SpikesUp(Collider _collider)
    {
        yield return new WaitForSeconds(0.2f);

        if (_collider != null)
        {
            GetComponent<Animator>().SetBool("Attack", false);
            _collider.transform.root.GetComponent<Enemy>().TakeDamage(_demageEnemy);
        }

        _health -= _TakeDamage;
        _healthUI.updateHP(_health, _maxHP);

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
