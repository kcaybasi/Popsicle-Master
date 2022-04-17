using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class C_GameManager : MonoBehaviour
{

    private Animator gameManagerAnimator;

    [Header("UI Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject gameOverMenu;
    List<GameObject> menuList = new List<GameObject>();

    [Header("Juice Holders")]
    [SerializeField] List<GameObject> juiceList = new List<GameObject>();
    public GameObject selectedJuice;

    public static C_GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DOTween.Init();
        AddMenuObjectsToList();
        gameManagerAnimator = GetComponent<Animator>();
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
        gameManagerAnimator.SetBool("MakingPopsicle", true); //Trigger state camera turning 
        OpenMenu(gameplayMenu);
    }

    public void SelectStrawberryJuice()
    {
        ActivateSelectedJuice(juiceList[0]);
    }

    public void SelectKiwiJuice()
    {
        ActivateSelectedJuice(juiceList[1]);
    }

    public void SelectOrangeJuice()
    {
        ActivateSelectedJuice(juiceList[2]);
    }
}
