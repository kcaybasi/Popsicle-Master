using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    private GameObject selectedJuice;


    private void GetSelectedJuice()
    {
        selectedJuice = C_GameManager.instance.selectedJuice;
    }

    private void Update()
    {
        
    }
}
