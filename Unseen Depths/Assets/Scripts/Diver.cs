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

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    //Controls
    public void Move(Vector3 movement){
        rb.velocity = movement * speed;
    }

    public void LauchProjectile(){
        float direction = transform.localScale.x > 0 ? 1 : -1;
        projectileLauncher.Launch(direction);
    }
}
