using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] float speed = 1.0f;

    [Header("Appearence")]
    [SerializeField] public Diver targetDiver;
    [SerializeField] Transform faceFront;
    [SerializeField] SpriteRenderer creatureSprite;
    [SerializeField] Collider2D creatureCollider;


    [Header("Visibility")]
    [SerializeField] float minDisappearTime = 2.0f;
    [SerializeField] float maxDisappearTime = 4.0f;
    [SerializeField] float minReappearTime = 1.0f;
    [SerializeField] float maxReappearTime = 3.0f;

    public delegate void CreatureKilled();
    public event CreatureKilled OnCreatureKilled;


    // Update is called once per frame
    void Update(){
        MoveTowards(targetDiver.transform.position);
        AimAtTarget(targetDiver.transform);
    }

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Visibility());
    }

    public void MoveTowards(Vector3 goalPos){
        goalPos.z = 0;
        Vector3 direction = (goalPos - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void AimAtTarget(Transform targetTransform){
        // Check the direction relative to the target diver
        if (targetTransform.position.x > faceFront.position.x){
            // Diver is to the left, face left
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else{
            // Diver is to the right, face right
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private IEnumerator KillEffect(){
        creatureSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Projectile")){
            Destroy(collider.gameObject);
            OnCreatureKilled?.Invoke();
            StartCoroutine(KillEffect());
        }
    }

    IEnumerator Visibility(){
        while(true){
            creatureSprite.enabled = false;
            creatureCollider.enabled = false;
            float disappearTime = Random.Range(minDisappearTime, maxDisappearTime);
            yield return new WaitForSeconds(disappearTime);

            creatureSprite.enabled = true;
            creatureCollider.enabled = true;
            float reappearTime = Random.Range(minReappearTime, maxReappearTime);
            yield return new WaitForSeconds(reappearTime);
        }
    }
}
