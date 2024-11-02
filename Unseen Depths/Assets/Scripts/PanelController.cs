using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] public GameObject panel;

    void Start(){
        panel.SetActive(false);
    }

    public void ShowPanel(){
        if(panel !=null){
            panel.SetActive(true);
        }
    }

    public void HidePanel(){
        if(panel !=null){
            panel.SetActive(false);
        }
    }
}
