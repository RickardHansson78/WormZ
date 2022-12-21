using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damageRadius;
    [SerializeField] private int damage;

    void Start()
    {
        StartCoroutine(WaitForDamage());
    }

    private IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(0.5f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        foreach(Collider collider in colliders)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                if(collider.TryGetComponent<Player>(out var player))
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}
