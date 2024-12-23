using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] public Diver player;
    
    // Update is called once per frame
    void Update(){
        if(Input.GetMouseButtonDown(0)){
            player.LauchProjectile();
        }
    }
    
    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        if(Input.GetKey(KeyCode.A)){
            movement += new Vector3(-1,0,0);
            player.transform.localScale = new Vector3(-1, 1, 1);
        }

        if(Input.GetKey(KeyCode.D)){
            movement += new Vector3(1,0,0);
            player.transform.localScale = new Vector3(1, 1, 1);
        }

        if(Input.GetKey(KeyCode.W)){
            movement += new Vector3(0,1,0);
        }

        if(Input.GetKey(KeyCode.S)){
            movement += new Vector3(0,-1,0);
        }
        player.Move(movement);
    }
}
