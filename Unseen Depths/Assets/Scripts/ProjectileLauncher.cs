using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnTransform;
    [SerializeField] AudioSource audioSource;
    
    public void Launch(){
        GameObject newProjectile = Instantiate(projectilePrefab, spawnTransform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector3(projectileSpeed, 0, 0);
        audioSource.Play();
        Destroy(newProjectile, 4);
    }
}
