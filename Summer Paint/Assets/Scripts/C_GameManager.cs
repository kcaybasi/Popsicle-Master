using System;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class C_GameManager : MonoBehaviour
{


    [Header("Game States")]
    public State gameState=State.gettingOrder;
    public enum State { gettingOrder,moldFilling, stickPlacing, freezing  ,finishingOrder}
    private Animator gameManagerAnimator;

    [Header("Game Events")]

    [SerializeField] UnityEvent OnGameStarted;
    public event EventHandler OnJuiceSelected;
    public event EventHandler OnGameFinish;
    public static C_GameManager instance;
   
    [Header("GameObjects")]

    [SerializeField] GameObject cameraObj;
    public PopsicleMold popsicleMold;
    public PopsicleStick popsicleStick;
    public Freezer freezer;
    public GameObject swipeDetector;
    [SerializeField] ParticleSystem confetti;
 
    [Header("UI Menu")]

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] List<GameObject> menuList = new List<GameObject>();

    [Header("Buttons")]

    [SerializeField] GameObject checkButton;
    [SerializeField] GameObject finishButton;

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
        DOTween.SetTweensCapacity(50, 500);
        popsicleStick.OnStickPlaced += PopsicleStick_OnStickPlaced;
        popsicleMold.OnJuiceFilled += PopsicleMold_OnJuiceFilled;
        freezer.OnFreezingDone += Freezer_OnFreezingDone;
        gameManagerAnimator = GetComponent<Animator>();
    }

    private void Freezer_OnFreezingDone(object sender, EventArgs e)
    {
        gameState = State.finishingOrder;
        ActivateButton(finishButton, 1612f, 0f, true);
       
    }

    private void PopsicleStick_OnStickPlaced(object sender, EventArgs e)
    {
        gameState = State.freezing;
        swipeDetector.SetActive(true);
        ActivateButton(checkButton,1614f,420f,true);
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
                juiceList[i].transform.DOMoveZ(-23f, 1f, false);
            }
        }
        juiceObj.SetActive(true);
        selectedJuice = juiceObj;
        selectedJuice.transform.DOMoveZ(-18.2f, 1f, false);
        
        
    }

    private void ActivateButton(GameObject button,float startingPos, float endingPos , bool isActive) 
    {
        if (isActive)
        {        
            button.GetComponent<RectTransform>().DOAnchorPos3DX(endingPos, 1.25f, false);
        }
        else
        {
            button.GetComponent<RectTransform>().DOAnchorPos3DX(startingPos, 1.25f, false);
        }
    }

    private void PrepareMoldToFreeze()
    {
        popsicleMold.transform.DOScale(0.2f, 1f);
        popsicleMold.moldCollider.enabled = true;
        popsicleMold.transform.parent = cameraObj.transform;
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
        PrepareMoldToFreeze();
        gameManagerAnimator.SetTrigger("StartFreezingPopsicle"); // Trigger state camera turning

        gameplayMenu.SetActive(false);
        ActivateButton(checkButton, 1614f, 420f, false);


    }

    public void FinishOrder()
    {
        gameState = State.finishingOrder;
        gameManagerAnimator.SetTrigger("ReturnToStand");
        ActivateButton(finishButton, 1612f, 0f, false);
        StartCoroutine(FinishGame(2f));
    }


    IEnumerator FinishGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnGameFinish?.Invoke(this, EventArgs.Empty);
        confetti.Play();
        OpenMenu(gameOverMenu);
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

}
