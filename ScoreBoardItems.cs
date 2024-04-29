using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ScoreBoardItems : MonoBehaviourPunCallbacks
{
    public TMP_Text UseName;
    public TMP_Text Deaths;
    public TMP_Text Kills;
    Player Player;
    public void Instantiate(Player player)
    {
        UseName.text = player.NickName;
        this.Player = player;
    }
    void UpdateState()
    {
        if(Player.CustomProperties.TryGetValue("kills", out var kills))
        {
            Kills.text = kills.ToString();
        }
        if (Player.CustomProperties.TryGetValue("deaths", out var deaths))
        {
            Deaths.text = deaths.ToString();
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer == Player)
        {
            if(changedProps.ContainsKey("kills")|| changedProps.ContainsKey("deaths"))
            {
                UpdateState();
            }
        }
    }
}
