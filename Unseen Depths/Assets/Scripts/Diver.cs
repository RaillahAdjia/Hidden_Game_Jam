using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diver : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] float speed = 5.0f;
    [Header("Tools")]
    [SerializeField] ProjectileLauncher projectileLauncher;
    [Header("Effects")]
    [SerializeField] GameObject bloodEffectPrefab;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector3 movement){
        rb.velocity = movement * speed;
    }

    public void MoveTowards(Vector3 goalPos){
        goalPos.z = 0;
        Vector3 direction = goalPos - transform.position;
        Move(direction.normalized);
    }

    public void LauchProjectile(){
        float direction = transform.localScale.x > 0 ? 1 : -1;
        projectileLauncher.Launch(direction);
    }

    private IEnumerator KillEffect(){
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null){
            levelManager.PlayerDied();
        }

        // Existing kill effect logic
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        if (playerRb != null){
            playerRb.gravityScale = 0;
        }

        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject creature in creatures){
            Rigidbody2D creatureRb = creature.GetComponent<Rigidbody2D>();
            if (creatureRb != null){
                creatureRb.gravityScale = 0;
            }
        }

        // Instantiate blood effect at the player's position
        if (bloodEffectPrefab != null){
            Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1.0f); // Wait for 1 second to allow the effect to play

        Destroy(gameObject); // Destroy the player after the effect
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Creature")){
            StartCoroutine(KillEffect());
        }
    }
}
