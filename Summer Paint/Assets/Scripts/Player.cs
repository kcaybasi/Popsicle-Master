using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Player : MonoBehaviour
{

    [Header("Movement")]

    private Vector3 slideVector;
    private Touch touch;
    [SerializeField] float slideSpeed;
    [SerializeField] float pcSlideSpeed;

    [SerializeField] PopsicleMold popsicleMold;
    [SerializeField] GameObject playerObject;
    private C_GameManager gameManager;

    [Header("Juice Pouring")]

    [SerializeField] float holdtimer;
    GameObject spillFillImage;
    ParticleSystem juiceParticle;
 

    private void Start()
    {
       
        gameManager = C_GameManager.instance;
        gameManager.OnJuiceSelected += Instance_OnJuiceSelected;
        popsicleMold.OnJuiceFilled += PopsicleMold_OnJuiceFilled;
    }

    private void PopsicleMold_OnJuiceFilled(object sender, EventArgs e)
    {
        playerObject = gameManager.popsicleStick; //Assing popsicle stick as player object;
        gameManager.gameState = C_GameManager.State.stickPlacing;
    }

    private void Instance_OnJuiceSelected(object sender, EventArgs e)
    {
        playerObject = gameManager.selectedJuice; // Assign selected juice to move by player. 
        spillFillImage = playerObject.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        juiceParticle = playerObject.transform.GetChild(2).GetComponent<ParticleSystem>();
        gameManager.gameState = C_GameManager.State.moldFilling;

    }

    private void Update()
    {
        MovePlayerObject();
    }


    private void MovePlayerObject()
    {
        switch (gameManager.gameState)
        {
            case C_GameManager.State.moldFilling: MoveJuiceBottles();
                break;
            case C_GameManager.State.stickPlacing:MovePopsicleStick();
                break;
        }
    }

    private void MoveJuiceBottles()
    {
        TouchControl();
        KeyboardControl();
       
    }

    private void MovePopsicleStick()
    {
        TouchControl();
        KeyboardControl();
    }


    private void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                slideVector = new Vector3(
                            playerObject.transform.position.x,
                            playerObject.transform.position.y + touch.deltaPosition.y * slideSpeed * Time.deltaTime,
                            playerObject.transform.position.z - touch.deltaPosition.x * slideSpeed * Time.deltaTime);
                playerObject.transform.position = slideVector;
             
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                PourJuice();
            }

        }
        ClampPosition(slideVector);      

    }

    private void KeyboardControl()
    {
        Vector3 pos = playerObject.transform.position;


        if (Input.GetKey(KeyCode.D))
        {
            pos.z -= pcSlideSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.z += pcSlideSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos.y += pcSlideSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= pcSlideSpeed * Time.deltaTime;
        }

        playerObject.transform.position = pos;
    }

    private void ClampPosition(Vector3 clampedPos)
    {
        clampedPos = playerObject.transform.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, 0.9f, 4.3f);
        clampedPos.z = Mathf.Clamp(clampedPos.z, -18f, -15.5f);
        
        playerObject.transform.position = clampedPos;
    }

    private void PourJuice()
    {
        float spillAmount = spillFillImage.GetComponent<Image>().fillAmount;
        spillAmount += Time.deltaTime;
        spillFillImage.GetComponent<Image>().fillAmount = spillAmount;
        if (spillAmount > 0.95f)
        {
            juiceParticle.Play();
        }

        
    }


}
