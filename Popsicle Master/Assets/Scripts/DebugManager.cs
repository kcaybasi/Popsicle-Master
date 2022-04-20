using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] List<GameObject> cameraList = new List<GameObject>();
    [SerializeField] GameObject activeCamera;
    [SerializeField] GameObject debugCameraMenu;
    Quaternion cam1pos,cam2pos,cam3pos;


    private void Start()
    {
        GetOriginalCamRotations();
    }

    private void GetOriginalCamRotations()
    {
        cam1pos = cameraList[0].transform.rotation;
        cam2pos = cameraList[1].transform.rotation;
        cam3pos = cameraList[2].transform.rotation;
    }


    public void ActivateCameraMenu()
    {
        debugCameraMenu.SetActive(true);
        
    }

    public void GetCamera()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        switch (clickedButton.tag)
        {
            case "cam1": activeCamera = cameraList[0];             
                break;
            case "cam2":activeCamera = cameraList[1];
                break;
            case "cam3": activeCamera = cameraList[2];
                break;
        }
        CinemachineBrain.SoloCamera = activeCamera.GetComponent<CinemachineVirtualCamera>();
    }

    public void CloseCameraMenu()
    {
        debugCameraMenu.SetActive(false);
        CinemachineBrain.SoloCamera = null;     
      
    }

    public void RotateCamera()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        switch (clickedButton.tag)
        {
            case "Left": activeCamera.transform.Rotate(Vector3.down, 2.5f);
                break;
            case "Right":activeCamera.transform.Rotate(Vector3.up, 2.5f);
                break;
            case "Up":activeCamera.transform.Rotate(Vector3.left, 2.5f);
                break;
            case "Down":activeCamera.transform.Rotate(Vector3.right, 2.5f);
                break;
        }
    }

    public void ResetCameras()
    {
        cameraList[0].transform.SetPositionAndRotation(cameraList[0].transform.position, cam1pos);
        cameraList[1].transform.SetPositionAndRotation(cameraList[1].transform.position, cam2pos);
        cameraList[2].transform.SetPositionAndRotation(cameraList[2].transform.position, cam3pos);
    }
}
