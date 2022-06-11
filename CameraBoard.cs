using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoard : MonoBehaviour
{
    public Camera cameraBoard;
    public bool status = false;
    public Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        cameraBoard = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCamera == null)
        {
            try
            {
                playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
            catch
            {
                
            }
            return;
        }
        bool isJumpPressed = Input.GetKeyDown(KeyCode.Space);
        if (isJumpPressed)
        {
            this.playerCamera.enabled = !status;
            this.cameraBoard.enabled = status;
            status = !status;
            if (status)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
