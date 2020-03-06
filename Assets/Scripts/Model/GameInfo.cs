using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo 
{
    public List<PlayerInfo> Players;

    public GameInfo()
    {
        Players = new List<PlayerInfo>();
    }

    public void AddPlayer(PlayerInfo player)
    {
        foreach(PlayerInfo otherPlayer in Players)
        {
            otherPlayer.CommanderDamage[player] = new SubscriptionValue<int>(0);
            player.CommanderDamage[otherPlayer] = new SubscriptionValue<int>(0);
        }

        Players.Add(player);     
    }
}
