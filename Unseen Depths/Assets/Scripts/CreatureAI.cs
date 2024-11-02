using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] float speed = 5.0f;

    [SerializeField] Diver targetDiver;
    [SerializeField] Transform faceFront;

    // Update is called once per frame
    void Update()
    {
        MoveTowards(targetDiver.transform.position);
        AimAtTarget(targetDiver.transform);
    }

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector3 goalPos)
    {
        goalPos.z = 0;
        Vector3 direction = (goalPos - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void AimAtTarget(Transform targetTransform)
    {
        // Check the direction relative to the target diver
        if (targetTransform.position.x > faceFront.position.x)
        {
            // Diver is to the left, face left
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Diver is to the right, face right
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
