using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor;
    [SerializeField] private Color lowHealthColor;

    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthImage;
    [SerializeField] private TMP_Text ammoTextField;

    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float invincibilitySeconds = 1;
    [SerializeField] private int maxAmmo = 10;

    [Header("Weapons")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject rocketLauncher;

    [Header("Other")]
    [SerializeField] private PlayerTurn playerTurn;
    [SerializeField] private ProjectileScript projectilePrefab;
    [SerializeField] private Transform shootingStartPosition;

    private int _playerHealth;
    private bool _invincibilityFrames;
    private int _ammo;

    private enum Weapon
    {
        Gun,
        RocketLauncher
    }
    [SerializeField] private Weapon _currentWeapon;

    private void Awake()
    {
        _ammo = maxAmmo;
        _playerHealth = maxHealth;
        healthImage.color = fullHealthColor;
        healthBar.value = 1;
        rocketLauncher.SetActive(_currentWeapon == Weapon.RocketLauncher);
        gun.SetActive(_currentWeapon == Weapon.Gun);
    }

    private void Update()
    {
        bool isPlayerTurn = playerTurn.IsPlayerTurn();
        if (isPlayerTurn && Input.GetKeyDown(KeyCode.Mouse0) && _ammo > 0)
        {
            _ammo--;
            UpdateAmmoCanvas();
            ProjectileScript newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = shootingStartPosition.position;
            newProjectile.transform.localRotation = shootingStartPosition.rotation;
            newProjectile.GetComponent<ProjectileScript>().Initialize();
            TurnBasedManager.GetInstance().TriggerChangeTurn();

            if (_currentWeapon == Weapon.RocketLauncher)
            {
                newProjectile.rocketLauncherShot = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && isPlayerTurn)
        {
            SwapWeapon();
        }
    }

    private void SwapWeapon()
    {
        _currentWeapon = _currentWeapon == Weapon.Gun ? Weapon.RocketLauncher : Weapon.Gun;

        rocketLauncher.SetActive(_currentWeapon == Weapon.RocketLauncher);
        gun.SetActive(_currentWeapon == Weapon.Gun);
    }

    public void TakeDamage(int damage)
    {
        if (_invincibilityFrames) return;

        _playerHealth -= damage;
        UpdateHealthbar();

        if (_playerHealth <= 0)
        {
            SceneManager.LoadScene("EndScreen");
        }

        _invincibilityFrames = true;

        StartCoroutine(InvincibilityFrames());
    }

    private void UpdateHealthbar()
    {
        float healthbarValue = Mathf.InverseLerp(0, maxHealth, _playerHealth);

        healthBar.value = healthbarValue;

        float colorValue = Mathf.InverseLerp(0.2f, 0.8f, healthbarValue);
        healthImage.color = Color.Lerp(lowHealthColor, fullHealthColor, colorValue);
    }

    private IEnumerator InvincibilityFrames()
    {
        yield return new WaitForSeconds(invincibilitySeconds);
        _invincibilityFrames = false;
    }

    public void GainHealth(int health)
    {
        if(_playerHealth + health >= maxHealth)
        {
            _playerHealth = maxHealth;
        }
        else
        {
            _playerHealth += health;
        }

        UpdateHealthbar();
    }

    public void GainAmmo(int amount)
    {
        if (_ammo + amount >= maxAmmo)
        {
            _ammo = maxAmmo;
        }
        else
        {
            _ammo += amount;
        }
        UpdateAmmoCanvas();
    }

    public void UpdateAmmoCanvas() => ammoTextField.text = $"Ammo: {_ammo}";
}

