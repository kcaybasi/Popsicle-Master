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
    [SerializeField] PopsicleStick popsicleStick;
   
    private C_GameManager gameManager;
  

    [Header("Juice Pour Part")]

    [SerializeField] float holdtimer;
    GameObject spillFillImage;
    ParticleSystem juiceParticle;

    [Header("Freeze Part")]
    private string swapDirection;
    private bool isFreezingDone;



    private void Start()
    {     
        gameManager = C_GameManager.instance;
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
        gameManager.OnJuiceSelected += Instance_OnJuiceSelected;
        popsicleMold.OnJuiceFilled += PopsicleMold_OnJuiceFilled;
        popsicleStick.OnStickPlaced += PopsicleStick_OnStickPlaced;
    }

    private void SwipeDetector_OnSwipe(SwipeData obj)
    {
        swapDirection = obj.Direction.ToString();
    }

    private void PopsicleStick_OnStickPlaced(object sender, EventArgs e)
    {
       playerObject=popsicleMold.gameObject;
    }

    private void PopsicleMold_OnJuiceFilled(object sender, EventArgs e)
    {
        playerObject = gameManager.popsicleStick.gameObject; //Assing popsicle stick as player object;
   
    }

    private void Instance_OnJuiceSelected(object sender, EventArgs e)
    {
        playerObject = gameManager.selectedJuice; // Assign selected juice to move by player. 
        spillFillImage = playerObject.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        juiceParticle = playerObject.transform.GetChild(2).GetComponent<ParticleSystem>();

    }

    private void Update()
    {
        ControlPlayerObject();
    }


    private void ControlPlayerObject()
    {
        switch (gameManager.gameState)
        {
            case C_GameManager.State.moldFilling: ControlJuiceBottles();
                break;
            case C_GameManager.State.stickPlacing:MovePopsicleStick();
                break;
            case C_GameManager.State.freezing:
                if (swapDirection == "Up")
                {
                    if (!isFreezingDone)
                    {
                        StartCoroutine(FreezePopsicle(2.2f));
                        isFreezingDone = true;
                    }
                    
                }
                break;

        }
    }

    private void ControlJuiceBottles()
    {
        TouchControl();
        if (touch.phase == TouchPhase.Stationary )
        {
            PourJuice();
        }
        else if(touch.phase==TouchPhase.Moved)
        {
            return;
        }
        else
        {
            StopPouring();
        }
     
       
    }

    private void MovePopsicleStick()
    {
        TouchControl();

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
        }
        ClampPosition(slideVector);      

    }

    private void ClampPosition(Vector3 clampedPos)
    {
        clampedPos = playerObject.transform.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, 0.9f, 2.5f);
        clampedPos.z = Mathf.Clamp(clampedPos.z, -22f, -15.5f);
        
        playerObject.transform.position = clampedPos;
    }

    private void PourJuice()
    {
        spillFillImage.SetActive(true);
        float spillAmount = spillFillImage.GetComponent<Image>().fillAmount;
        spillAmount += Time.deltaTime;
        spillFillImage.GetComponent<Image>().fillAmount = spillAmount;
        if (spillAmount > 0.95f)
        {
            juiceParticle.Play();
        }  
    }

    private void StopPouring()
    {
        float spillAmount = spillFillImage.GetComponent<Image>().fillAmount;
        spillAmount -= Time.deltaTime;
        spillFillImage.GetComponent<Image>().fillAmount = spillAmount;
        juiceParticle.Stop();
    }



    IEnumerator FreezePopsicle(float freezeTime)
    {
        PutPopsicleInFreezer();
        yield return new WaitForSeconds(freezeTime);       
        PullPopsicleFromFreezer();    
    }


    public void PutPopsicleInFreezer()
    {
        playerObject.transform.DORotate(new Vector3(0f, 0, 0), 1f, RotateMode.Fast);
        playerObject.transform.DOMoveZ(-31f, 1f, false);

    }

    public void PullPopsicleFromFreezer()
    {
        playerObject.transform.DOMoveZ(-26f, 1f, false);
        playerObject.transform.DORotate(new Vector3(90, 0, 0), 0.65f, RotateMode.Fast);
        playerObject.transform.GetChild(4).gameObject.SetActive(true);
    }

}
