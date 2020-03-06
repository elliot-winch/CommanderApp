using System;
using UnityEngine;

public class CommanderDamageArc : UIArc
{
    [Header("Commander Damage Display")]
    [SerializeField]
    private SubscriptionIntController mAToBDamage;
    [SerializeField]
    private SubscriptionIntController mBToADamage;
    [SerializeField]
    private float mAToBDamageDistance;
    [SerializeField]
    private float mBToADamageDistance;
    [SerializeField]
    private float mMinDamageDisplayRenderDistance;

    protected override void Update()
    {
        base.Update();

        if(mAToBDamage != null)
        {
            mAToBDamage.transform.position = PositionAlongLine(mAToBDamageDistance);
            mAToBDamage.gameObject.SetActive(CurrentDistance > mMinDamageDisplayRenderDistance);
        }

        if (mBToADamage != null)
        {
            mBToADamage.transform.position = PositionAlongLine(CurrentDistance - mBToADamageDistance);
            mBToADamage.gameObject.SetActive(CurrentDistance > mMinDamageDisplayRenderDistance);
        }
    }

    public void SetPlayers(PlayerInfo playerA, PlayerInfo playerB)
    {
        if(playerA.CommanderDamage.ContainsKey(playerB) == false)
        {
            throw new ArgumentException("Player A Commander Damager for Player B not set up");
        }

        if (playerB.CommanderDamage.ContainsKey(playerA) == false)
        {
            throw new ArgumentException("Player B Commander Damager for Player A not set up");
        }

        mAToBDamage.SetSubscription(playerA.CommanderDamage[playerB]);
        mBToADamage.SetSubscription(playerB.CommanderDamage[playerA]);
    }
}
