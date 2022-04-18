using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private C_GameManager gameManager;
    private Animator customerAnimator;

    private void Start()
    {
        customerAnimator = GetComponent<Animator>();
        gameManager = C_GameManager.instance;
        gameManager.OnGameFinish += GameManager_OnGameFinish;
    }

    private void GameManager_OnGameFinish(object sender, System.EventArgs e)
    {
        customerAnimator.SetTrigger("Cheer");
    }
}
