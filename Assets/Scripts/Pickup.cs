using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int healAmount;
    [SerializeField] private int ammoAmount;

    private enum PickupType
    {
        Health,
        Ammo
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.TryGetComponent<Player>(out var player)) return;

            switch (type)
            {
                case PickupType.Health:
                    player.GainHealth(healAmount);
                    break;
                case PickupType.Ammo:
                    player.GainAmmo(ammoAmount);
                    break;
            }

            Destroy(gameObject);
        }
    }
}