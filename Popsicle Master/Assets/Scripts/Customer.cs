using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private C_GameManager gameManager;
    private Animator customerAnimator;
    [SerializeField] List<Texture> skinTextures = new List<Texture>();

    private void Start()
    {
        ChooseCustomerSkin();
        customerAnimator = GetComponent<Animator>();
        gameManager = C_GameManager.instance;
        gameManager.OnGameFinish += GameManager_OnGameFinish;
    }

    private void GameManager_OnGameFinish(object sender, System.EventArgs e)
    {
        customerAnimator.SetTrigger("Cheer");
    }


    private void ChooseCustomerSkin()
    {
        int random = Random.Range(0, 3);
        transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture = skinTextures[random];
    }
}
