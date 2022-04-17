using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    public GameObject selectedJuice;
    private Touch touch;

    private void Start()
    {
        C_GameManager.instance.OnJuiceSelected += Instance_OnJuiceSelected;
    }

    private void Instance_OnJuiceSelected(object sender, EventArgs e)
    {
        selectedJuice = C_GameManager.instance.selectedJuice; // Assign selected juice to move by player. 
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (touch.phase == TouchPhase.Ended)
            {
                return;
            }
            else
            {
                selectedJuice.transform.DORotate(new Vector3(0, -90f, -90f), 0.5f, RotateMode.Fast);
            }
        }
    }
}
