using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform contain;
    [SerializeField] GameObject PausePreFabs;
    [SerializeField] GameObject QuitPreFabs;
    [SerializeField] CanvasGroup canvasGroup;

    private void Start()
    {
        /*Instantiate(PausePreFabs, contain);
        Instantiate(QuitPreFabs, contain);*/
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            inputing();
        }
    }
    public void inputing()
    {
        
            switch (canvasGroup.alpha)
            {
                case 1:
                    canvasGroup.alpha = 0;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
                    break;
                case 0:
                    canvasGroup.alpha = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            }
        
    }
    public void backgame()
    {
        canvasGroup.alpha = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void quiteroom()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel(0);
    }

}
