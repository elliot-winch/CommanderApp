using System.Linq;
using UnityEngine;

public class UIArc : MonoBehaviour
{
    [SerializeField]
    private LineRenderer mLineRenderer;

    public Transform BeginTarget;
    public Transform EndTarget;
    [SerializeField]
    private float mBeginDistance;
    [SerializeField]
    private float mEndDistance;
    [SerializeField]
    private float mMinRenderDistance;

    protected float CurrentDistance => Vector3.Distance(BeginTarget.position, EndTarget.position);

    protected virtual void Update()
    {
        if(BeginTarget == null || EndTarget == null)
        {
            return;
        }

        Vector2[] positions = new Vector2[]
        {
            PositionAlongLine(mBeginDistance),
            PositionAlongLine(CurrentDistance - mEndDistance)
        };

        mLineRenderer.SetPositions(positions.Select(Helper.CanvasToWorldSpace).ToArray());
        mLineRenderer.enabled = CurrentDistance >= mMinRenderDistance;
    }

    protected Vector2 PositionAlongLine(float distance)
    {
        return (EndTarget.position - BeginTarget.position).normalized * distance + BeginTarget.position;        
    }
}
