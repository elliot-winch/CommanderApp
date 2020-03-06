using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDisplayFull : MonoBehaviour
{
    [SerializeField]
    private Text mName;
    [SerializeField]
    private SubscriptionIntController lifeTotal;
    [SerializeField]
    public SubscriptionIntController commanderCasts;

    public void SetPlayer(PlayerInfo player)
    {
        player.Name.Subscribe((name) =>
        {
            mName.text = name.Value;
        });

        lifeTotal.SetSubscription(player.LifeTotal);
        commanderCasts.SetSubscription(player.CommandCasts);

    }
}
