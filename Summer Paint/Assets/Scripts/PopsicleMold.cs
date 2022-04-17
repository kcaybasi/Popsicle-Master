using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class PopsicleMold : MonoBehaviour
{
    [SerializeField] LiquidVolume liquidVolume;
    BoxCollider moldCollider;

    private void Start()
    {
        moldCollider = GetComponent<BoxCollider>();
    }


    private void OnParticleCollision(GameObject other)
    {
        liquidVolume.level += Time.deltaTime * 0.18f;
        UpdateMoldCollider();

    }

    private void UpdateMoldCollider()
    {
        Vector3 colliderPos = moldCollider.center;
        colliderPos.z += Time.deltaTime * 0.3f;
        moldCollider.center = colliderPos;
    }
}
