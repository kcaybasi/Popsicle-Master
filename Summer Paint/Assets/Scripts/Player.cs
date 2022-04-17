using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    private GameObject selectedJuice;
    private Touch touch;

    private void GetSelectedJuice()
    {
        selectedJuice = C_GameManager.instance.selectedJuice;
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
