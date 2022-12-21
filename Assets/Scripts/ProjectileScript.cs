using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float dissapearTimer = 2f;
    [SerializeField] private int damage = 2;

    [Header("Rocket Launcher")]
    public bool rocketLauncherShot;
    [SerializeField] private GameObject explosion;
    public void Initialize()
    {
        rb.AddForce(transform.forward * 1300f); //Character controller radius 1.02
        StartCoroutine(DespawnTimer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(rocketLauncherShot)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            if(collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();

                player.TakeDamage(damage);
            }
        }
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(dissapearTimer);
        Destroy(gameObject);
    }
}
