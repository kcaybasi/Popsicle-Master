using System;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;


public class C_GameManager : MonoBehaviour
{


    [Header("Game States")]
    public State gameState=State.gettingOrder;
    public enum State { gettingOrder,moldFilling, stickPlacing, freezing}
   

    [Header("Game Events")]

    [SerializeField] UnityEvent OnGameStarted;
    [SerializeField] PopsicleMold popsicleMold;
    public GameObject popsicleStick;
    public event EventHandler OnJuiceSelected;
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
        popsicleMold.OnJuiceFilled += PopsicleMold_OnJuiceFilled;
        gameManagerAnimator = GetComponent<Animator>();
    }

    private void PopsicleMold_OnJuiceFilled(object sender, EventArgs e)
    {
        for(int i = 0; i < juiceList.Count; i++)
        {
            juiceList[i].SetActive(false);
        }
        popsicleStick.SetActive(true);
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



    // Button functions 

    public void StartMakingPopsticle()
    {
        OnGameStarted?.Invoke();
        gameManagerAnimator.SetBool("MakingPopsicle", true); //Trigger state camera turning 
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
        
    }
}
