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
    private RectTransform mAToBDamageRect;
    [SerializeField]
    private RectTransform mBToADamageRect;

    [Header("Scaling")]
    [SerializeField]
    [Tooltip("The width of the arc as a factor of the node scale")]
    private float mArcWidthFactor;
    [SerializeField]
    private float mArcIntDisplayFactor;
    [Tooltip("The size of the commander damage int displays as a factor of the node size")]
    [SerializeField]
    private float mArcIntFontSizeFactor;

    private float mAToBDamageDistance;
    private float mBToADamageDistance;
    private float mMinDamageDisplayRenderDistance;

    protected override void Update()
    {
        base.Update();

        if(mAToBDamage != null)
        {
            mAToBDamage.transform.position = PositionAlongLine(mAToBDamageDistance);
            mAToBDamage.transform.up = EndTarget.position - BeginTarget.position;

            mAToBDamage.gameObject.SetActive(CurrentDistance > mMinDamageDisplayRenderDistance);
        }

        if (mBToADamage != null)
        {
            mBToADamage.transform.position = PositionAlongLine(CurrentDistance - mBToADamageDistance);
            mBToADamage.transform.up = BeginTarget.position - EndTarget.position;

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

    public void Scale(float nodeCanvasSize)
    {
        //The arcs are in world space, but the nodes are in canvas space (pixels)
        //Thus we need to convert
        float arcScale = Helper.CanvasToWorldScale(nodeCanvasSize);

        mAToBDamageDistance = nodeCanvasSize * mArcIntDisplayFactor;
        mBToADamageDistance = nodeCanvasSize * mArcIntDisplayFactor;
        mMinDamageDisplayRenderDistance = nodeCanvasSize * 2f * mArcIntDisplayFactor;

        mLineRenderer.startWidth = arcScale * mArcWidthFactor;
        mLineRenderer.endWidth = arcScale * mArcWidthFactor;

        mAToBDamageRect.sizeDelta = Vector2.one * nodeCanvasSize * mArcIntFontSizeFactor;
        mBToADamageRect.sizeDelta = Vector2.one * nodeCanvasSize * mArcIntFontSizeFactor;
    }
}
