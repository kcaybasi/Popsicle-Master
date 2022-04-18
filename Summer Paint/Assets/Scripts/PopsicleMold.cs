using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class PopsicleMold : MonoBehaviour
{
    public LiquidVolume liquidVolume;
    public event EventHandler OnJuiceFilled;
    public BoxCollider moldCollider;
    [SerializeField] ParticleSystem fillupParticle;
    enum JuiceTypes {Null,Strawberry, Kiwi,Orange }
    private JuiceTypes currentJuiceType = JuiceTypes.Null;
    private int fruitLayerlevel = -1;
    C_GameManager gameManager;
    

    private void Start()
    {
        gameManager = C_GameManager.instance;
        gameManager.OnJuiceSelected += GameManager_OnJuiceSelected;
        moldCollider = GetComponent<BoxCollider>();
    }

    private void GameManager_OnJuiceSelected(object sender, EventArgs e)
    {
        fruitLayerlevel++;
    }

    private void OnParticleCollision(GameObject other)
    {
        CheckLiquidLevel(); // If liquid is full fire liquid full event and disable collider.
        GetJuiceType(other);
        Fill(currentJuiceType,0.18f);
        UpdateMoldCollider(0.3f);

    }

    private void CheckLiquidLevel()
    {
        if (liquidVolume.level >= 1)
        {
            OnJuiceFilled?.Invoke(this, EventArgs.Empty);
            moldCollider.enabled = false;
            fillupParticle.Play();
        }

    }


    private void Fill(JuiceTypes juiceTypes, float fillSpeed)
    {
        switch (juiceTypes)
        {
            case JuiceTypes.Strawberry: liquidVolume.liquidLayers[fruitLayerlevel].color = Color.red;
                break;
            case JuiceTypes.Kiwi: liquidVolume.liquidLayers[fruitLayerlevel].color = Color.green;
                break;
            case JuiceTypes.Orange: liquidVolume.liquidLayers[fruitLayerlevel].color = Color.yellow;
                break;
        }
        liquidVolume.liquidLayers[fruitLayerlevel].amount += Time.deltaTime * fillSpeed;
        liquidVolume.UpdateLayers(true);
    }

    private void GetJuiceType(GameObject other)
    {
        switch (other.tag)
        {
            case "Strawberry":
                currentJuiceType = JuiceTypes.Strawberry;
                break;
            case "Kiwi":
                currentJuiceType = JuiceTypes.Kiwi;
                break;
            case "Orange":
                currentJuiceType = JuiceTypes.Orange;
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
