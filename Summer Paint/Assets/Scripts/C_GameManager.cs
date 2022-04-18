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


    [Header("Game Events")]

    [SerializeField] GameObject cameraObj;
    [SerializeField] UnityEvent OnGameStarted;
    [SerializeField] PopsicleMold popsicleMold;
    public GameObject popsicleStick;
    public event EventHandler OnJuiceSelected;
    [SerializeField] GameObject checkButton;
    private Animator gameManagerAnimator;
    public static C_GameManager instance;
   
    [Header("UI Menus")]

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject gameOverMenu;
    List<GameObject> menuList = new List<GameObject>();

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
        AddMenuObjectsToList();
        popsicleStick.GetComponent<PopsicleStick>().OnStickPlaced += C_GameManager_OnStickPlaced;
        popsicleMold.OnJuiceFilled += PopsicleMold_OnJuiceFilled;
        gameManagerAnimator = GetComponent<Animator>();
    }

    private void C_GameManager_OnStickPlaced(object sender, EventArgs e)
    {
        gameState=State.freezing;
        ActivateCheckButton(true);

    }

    private void PopsicleMold_OnJuiceFilled(object sender, EventArgs e)
    {
        gameState = State.stickPlacing;
        for(int i = 0; i < juiceList.Count; i++)
        {
            juiceList[i].SetActive(false);
        }
        popsicleStick.SetActive(true);
        popsicleStick.transform.DOMoveZ(-18f, 0.5f, false);
      
        
    }

    private void AddMenuObjectsToList()
    {
        menuList.Add(mainMenu);
        menuList.Add(gameplayMenu);
        menuList.Add(gameOverMenu);
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

    private void ActivateCheckButton(bool isActive)
    {
        if (isActive)
        {
           
            checkButton.GetComponent<RectTransform>().DOAnchorPos3DX(420f, 1.25f, false);
        }
        else
        {
            checkButton.GetComponent<RectTransform>().DOAnchorPos3DX(1614f, 1.25f, false);
        }
    }

    // Button functions 

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
        popsicleMold.transform.parent = cameraObj.transform; // Assign as child of camera to pickup mold
        gameManagerAnimator.SetTrigger("StartFreezingPopsicle"); // Trigger state camera turning
        
        gameplayMenu.SetActive(false);
        ActivateCheckButton(false);
       
    }


}
