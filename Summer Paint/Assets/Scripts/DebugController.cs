
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DebugController : MonoBehaviour
{
    [SerializeField] List<GameObject> cameraList = new List<GameObject>();
    [SerializeField] GameObject activeCamera;
    [SerializeField] bool isDebugging;
   
    C_GameManager gameManager;

    private void Start()
    {
        gameManager = C_GameManager.instance;
       
    }


    private void Update()
    {

    }


    private void GetActiveCamera()
    {

        switch (gameManager.gameState)
        {
            case C_GameManager.State.gettingOrder: activeCamera = cameraList[0];
                break;
            case C_GameManager.State.moldFilling:activeCamera = cameraList[1];
                break;
            case C_GameManager.State.freezing:activeCamera = cameraList[2];
                break;
            default: activeCamera = cameraList[0];
                break;
        }

    }

    private void RotateCamera()
    {
       
    }

    public void ActivateDebugMode()
    {
        isDebugging = true;
        GetActiveCamera();
    }
}
