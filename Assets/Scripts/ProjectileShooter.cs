using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float bulletVelocity = 20f;
    public float fireRate = 0.2f;
    public float maxSpreadAngle = 5f;
    public float spreadIncreaseRate = 0.1f;
    public float maxSpreadIncrease = 10f;

    [Header("Aim Settings")]
    public PlayerController player;
    public bool isAimable = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fireSound;

    private float currentSpread = 0f;
    private float fireTimer = 0f;

    private void Update()
    {
        // Decrease current spread over time
        currentSpread = Mathf.Clamp(currentSpread - (spreadIncreaseRate * Time.deltaTime), 0f, maxSpreadIncrease);

        // Check if it's time to shoot
        if (fireTimer > 0f)
            fireTimer -= Time.deltaTime;

        // Check for user input to fire
        if (Input.GetButton("PrimaryFireButton") && fireTimer <= 0f)
        {
            ShootProjectile();
            fireTimer = fireRate;
        }
    }

    private void ShootProjectile()
    {
        // Calculate spread angle
        float spreadAngle = Random.Range(0f, currentSpread) - currentSpread / 2f;
        currentSpread = Mathf.Min(currentSpread + spreadIncreaseRate, maxSpreadIncrease);

        // Calculate bullet direction with spread angle
        Vector3 bulletDirection;
        if (isAimable)
        {
            bulletDirection = Quaternion.Euler(0f, 0f, spreadAngle + player._aimAngle) * transform.up;
        }
        else
        {
            bulletDirection = Quaternion.Euler(0f, 0f, spreadAngle) * transform.up;
        }

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the projectile's velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bulletDirection * bulletVelocity;
        }

        // Play the firing sound
        if (audioSource && fireSound)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // Optional: Set any other properties of the projectile (e.g., damage, effects, etc.)

        // Optional: Destroy the projectile after a certain time if it doesn't collide with anything
        Destroy(projectile, 5f);
    }
}




