using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;  

    public RoomInfo Info;
    public void setUp(RoomInfo _Info)
    {
        Info = _Info;
        text.text = _Info.Name;
    }
    public void OnClick()
    {
        launch.Instance.JoinRoom(Info);
    }
}
