using System;
using System.Collections.Generic;

public class PlayerInfo
{
    public SubscriptionValue<string> Name { get; private set; }
    public SubscriptionValue<int> LifeTotal { get; private set; }
    public SubscriptionValue<int> CommandCasts { get; private set; }
    public Dictionary<PlayerInfo, SubscriptionValue<int>> CommanderDamage { get; private set; }

    public PlayerInfo(string name, int startingLifeTotal) 
    {
        Name = new SubscriptionValue<string>(name);
        LifeTotal = new SubscriptionValue<int>(startingLifeTotal);
        CommandCasts = new SubscriptionValue<int>(0);
        CommanderDamage = new Dictionary<PlayerInfo, SubscriptionValue<int>>();
    }

    public void AddCommanderDamage(PlayerInfo player, int delta)
    {
        if(CommanderDamage.ContainsKey(player) == false)
        {
            throw new ArgumentException("Cannot add commander damage as player is not initialised");
        }

        CommanderDamage[player].Value += delta;
    }
}
