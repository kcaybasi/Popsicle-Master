using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C_GameManager : MonoBehaviour
{

    [Header("UI Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject gameOverMenu;
    List<GameObject> menuList = new List<GameObject>();


    private Animator gameManagerAnimator;

    private void Start()
    {
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

    // Button functions 

    public void StartMakingPopsticle()
    {
        gameManagerAnimator.SetBool("MakingPopsicle", true); //Trigger state camera turning 
        OpenMenu(gameplayMenu);
    }
}
