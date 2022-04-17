using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class PopsicleMold : MonoBehaviour
{
    public LiquidVolume liquidVolume;
    public event EventHandler OnJuiceFilled;
    BoxCollider moldCollider;

    private void Start()
    {
        moldCollider = GetComponent<BoxCollider>();
    }


    private void OnParticleCollision(GameObject other)
    {
        CheckLiquidLevel(); // If liquid is full fire liquid full event and disable collider.
        GetJuiceColor(other);
        Fill(0.18f);
        UpdateMoldCollider(0.3f);

    }

    private void CheckLiquidLevel()
    {
        if (liquidVolume.level >= 1)
        {
            OnJuiceFilled?.Invoke(this, EventArgs.Empty);
            moldCollider.enabled = false;
        }

    }

    private void Fill(float fillSpeed)
    {
        liquidVolume.level += Time.deltaTime * fillSpeed;
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

    private void UpdateMoldCollider(float colliderMovementSpeed)
    {
        Vector3 colliderPos = moldCollider.center;
        colliderPos.z += Time.deltaTime * colliderMovementSpeed;
        moldCollider.center = colliderPos;
    }
}
