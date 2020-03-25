using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PlayerNode : MonoBehaviour
{
    [SerializeField]
    private Text mName;
    [SerializeField]
    private SubscriptionIntController mLifeTotal;
    //[SerializeField]
    //private SubscriptionIntController mCommanderCasts;
    [SerializeField]
    private RectTransform mVisualsTransform;

    public RectTransform RectTransform { get; private set; }
    public RectTransform VisualRectTransform => mVisualsTransform;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();   
    }

    public void SetPlayer(PlayerInfo player)
    {
        player.Name.Subscribe((name) =>
        {
            mName.text = name.Value;
        });

        mLifeTotal.SetSubscription(player.LifeTotal);
        //mCommanderCasts.SetSubscription(player.CommandCasts);      
    }
}
