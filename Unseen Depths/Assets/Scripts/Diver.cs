using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diver : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] float speed = 3.0f;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    //Controls
    public void Move(Vector3 movement){
        rb.velocity = movement * speed;
    }
}
