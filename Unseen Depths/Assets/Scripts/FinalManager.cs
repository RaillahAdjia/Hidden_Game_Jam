using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    public void GoHome(){
        GameManager.instance.LoadScene("MainMenu");
    }
}
