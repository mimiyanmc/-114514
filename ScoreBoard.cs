using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform contianer;
    [SerializeField] GameObject scoreBoardprefabs;
    [SerializeField] CanvasGroup canvasGroup;
    Dictionary<Player, ScoreBoardItems> scores = new Dictionary<Player, ScoreBoardItems>();

    public void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList) {
            AddScoreBoardItem(player);
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreBoardItem(otherPlayer);
    }

    private void RemoveScoreBoardItem(Player player)
    {
        Destroy(scores[player].gameObject);
        scores.Remove(player);
    }

    private void AddScoreBoardItem(Player player)
    {
        ScoreBoardItems scoreBoardItems = Instantiate(scoreBoardprefabs, contianer).GetComponent<ScoreBoardItems>();
        scoreBoardItems.Instantiate(player);
        scores[player] = scoreBoardItems;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            canvasGroup.alpha = 1;
        }
        else if(Input.GetKeyUp(KeyCode.Tab)) {
            canvasGroup.alpha = 0;
        }
    }
}
