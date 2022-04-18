using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Freezer : MonoBehaviour
{
    public event EventHandler OnFreezingDone;
    [SerializeField] GameObject freezerDoor;



    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(StartFreezingSequence());      
    }

    

    IEnumerator StartFreezingSequence()
    {
        freezerDoor.transform.DOLocalRotate(new Vector3(-90f, 0f, 0f), 1f, RotateMode.Fast);
        yield return new WaitForSeconds(1.5f);
        freezerDoor.transform.DOLocalRotate(new Vector3(-90f, 0f, -110f), 1f, RotateMode.Fast);
        OnFreezingDone?.Invoke(this, EventArgs.Empty);
    }
}
