using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Movement();
        Spill();
    }

    private void MovePopsicleStick()
    {
        Movement();
    }


    private void Movement()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                slideVector = new Vector3(
                            playerObject.transform.position.x ,
                            playerObject.transform.position.y + touch.deltaPosition.y * slideSpeed * Time.deltaTime,
                            playerObject.transform.position.z + touch.deltaPosition.x * slideSpeed * Time.deltaTime);
                playerObject.transform.position = slideVector;
            }

        }
       // ClampPosition(slideVector);
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
        clampedPos.x = Mathf.Clamp(clampedPos.x, -8.25f, 8.25f);
        playerObject.transform.position = clampedPos;
    }

    private void Spill()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Vector3 spillRotation = playerObject.transform.eulerAngles;
            spillRotation.x = -165f;
            playerObject.transform.DORotate(spillRotation, 1f, RotateMode.Fast);
        }
        
    }
}
