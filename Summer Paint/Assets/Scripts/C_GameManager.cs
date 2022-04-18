using System;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class C_GameManager : MonoBehaviour
{


    [Header("Game States")]
    public State gameState=State.gettingOrder;
    public enum State { gettingOrder,moldFilling, stickPlacing, freezing}
    private Animator gameManagerAnimator;

    [Header("Game Events")]

    [SerializeField] UnityEvent OnGameStarted;
    public event EventHandler OnJuiceSelected;
    public static C_GameManager instance;
   
    [Header("GameObjects")]

    [SerializeField] GameObject cameraObj;
    public PopsicleMold popsicleMold;
    public PopsicleStick popsicleStick;

    [Header("UI Menu")]

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] List<GameObject> menuList = new List<GameObject>();

    [Header("Buttons")]

    [SerializeField] GameObject checkButton;
    [SerializeField] GameObject putButton;

    [Header("Juice Holders")]

    [SerializeField] List<GameObject> juiceList = new List<GameObject>();
    public GameObject selectedJuice;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DOTween.Init();
        popsicleStick.OnStickPlaced += PopsicleStick_OnStickPlaced;
        popsicleMold.OnJuiceFilled += PopsicleMold_OnJuiceFilled;
        gameManagerAnimator = GetComponent<Animator>();
    }

    private void PopsicleStick_OnStickPlaced(object sender, EventArgs e)
    {
        gameState = State.freezing;
        ActivateButton(true, checkButton);
    }

    private void PopsicleMold_OnJuiceFilled(object sender, EventArgs e)
    {
        gameState = State.stickPlacing;
        for(int i = 0; i < juiceList.Count; i++)
        {
            juiceList[i].SetActive(false);
        }
        popsicleStick.gameObject.SetActive(true);
        popsicleStick.transform.DOMoveZ(-18f, 0.5f, false);
      
        
    }

    private void OpenMenu(GameObject menuGameObj)
    {
        int selectedMenuIndex= menuList.IndexOf(menuGameObj);
        for (int i = 0; i < menuList.Count; i++)
        {
            if (i != selectedMenuIndex)
            {
                menuList[i].SetActive(false);
            }
        }
        menuGameObj.SetActive(true);
        

    }

    private void ActivateSelectedJuice(GameObject juiceObj)
    {
        int selectedJuiceIndex = juiceList.IndexOf(juiceObj);
        for(int i = 0; i < juiceList.Count; i++)
        {
            if (i != selectedJuiceIndex)
            {
                juiceList[i].SetActive(false);
            }
        }
        juiceObj.SetActive(true);
        selectedJuice = juiceObj;
        selectedJuice.transform.DOShakePosition(0.8f, 0.4f, 6, 10);
        
        
    }

    private void ActivateButton(bool isActive,GameObject button) 
    {
        if (isActive)
        {        
            button.GetComponent<RectTransform>().DOAnchorPos3DX(420f, 1.25f, false);
        }
        else
        {
            button.GetComponent<RectTransform>().DOAnchorPos3DX(1614f, 1.25f, false);
        }
    }

    
    #region Button Functions

    public void StartMakingPopsicle()
    {
        OnGameStarted?.Invoke(); 
        gameManagerAnimator.SetTrigger("StartMakingPopsicle");//Trigger state camera turning 
        OpenMenu(gameplayMenu);
    }

    public void SelectJuice()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        switch (clickedButton.tag)
        {
            case "Strawberry": ActivateSelectedJuice(juiceList[0]);
                break;
            case "Kiwi": ActivateSelectedJuice(juiceList[1]);
                break;
            case "Orange": ActivateSelectedJuice(juiceList[2]);
                break;
        }
        OnJuiceSelected?.Invoke(this, EventArgs.Empty);
        gameState = State.moldFilling;
        
    }

    public void StartFreezingPopsicle()
    {
        popsicleMold.transform.DOScale(0.2f, 1f);
        popsicleMold.moldCollider.enabled = true;
        popsicleMold.transform.parent = cameraObj.transform; 
        gameManagerAnimator.SetTrigger("StartFreezingPopsicle"); // Trigger state camera turning
        
        gameplayMenu.SetActive(false);
        ActivateButton(false,checkButton);
        ActivateButton(true, putButton);
       
    }


    #endregion

}
