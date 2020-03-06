using UnityEngine;
using UnityEngine.UI;

public class SubscriptionIntController : MonoBehaviour
{
    [SerializeField]
    private Text mDisplayText;

    public SubscriptionValue<int> SubscriptionValue;

    public virtual void SetSubscription(SubscriptionValue<int> subscriptionValue)
    {
        SubscriptionValue?.Unsubscribe(OnValueChanged);

        SubscriptionValue = subscriptionValue;

        SubscriptionValue?.Subscribe(OnValueChanged);
    }

    public void AdjustValue(int amount)
    {
        SubscriptionValue.Value += amount;
    }

    private void OnValueChanged(SubscriptionValue<int> newValue)
    {
        mDisplayText.text = newValue.Value.ToString();
    }
}
