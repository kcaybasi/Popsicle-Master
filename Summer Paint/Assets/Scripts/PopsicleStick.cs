using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PopsicleStick : MonoBehaviour
{
    public event EventHandler OnStickPlaced;
    [SerializeField] ParticleSystem placedParticle;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StickPlace"))
        {
            OnStickPlaced?.Invoke(this,EventArgs.Empty);
            AdjustStickPosition(other);
            StartCoroutine(ActivateParticle(1f));
        }
    }

    private void AdjustStickPosition(Collider collider)
    {
        transform.parent = collider.transform;
        transform.DOLocalMove(Vector3.zero+Vector3.forward, 1f, false);
    }


    IEnumerator ActivateParticle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        placedParticle.Play();
    }
}
