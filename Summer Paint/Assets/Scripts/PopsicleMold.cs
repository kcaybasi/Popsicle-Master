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
        GetJuiceColor(other);
        Fill();
        UpdateMoldCollider();

    }

    private void Fill()
    {
        liquidVolume.level += Time.deltaTime * 0.18f;
    }

    private void GetJuiceColor(GameObject other)
    {
        switch (other.tag)
        {
            case "Strawberry":
                liquidVolume.liquidColor1 = Color.red;
                break;
            case "Kiwi":
                liquidVolume.liquidColor1 = Color.green;
                break;
            case "Orange":
                liquidVolume.liquidColor1 = Color.yellow;
                break;
        }
    }

    private void UpdateMoldCollider()
    {
        Vector3 colliderPos = moldCollider.center;
        colliderPos.z += Time.deltaTime * 0.3f;
        moldCollider.center = colliderPos;
    }
}
