using System.Linq;
using UnityEngine;

public class UIArc : MonoBehaviour
{
    [SerializeField]
    protected LineRenderer mLineRenderer;

    public Transform BeginTarget;
    public Transform EndTarget;
    public float BeginDistance;
    public float EndDistance;
    public float MinRenderDistance;

    protected float CurrentDistance => Vector3.Distance(BeginTarget.position, EndTarget.position);

    protected virtual void Update()
    {
        if(BeginTarget == null || EndTarget == null)
        {
            return;
        }

        Vector2[] positions = new Vector2[]
        {
            PositionAlongLine(BeginDistance),
            PositionAlongLine(CurrentDistance - EndDistance)
        };

        mLineRenderer.SetPositions(positions.Select(Helper.CanvasToWorldSpace).ToArray());
        mLineRenderer.enabled = CurrentDistance >= MinRenderDistance;
    }

    protected Vector2 PositionAlongLine(float distance)
    {
        return (EndTarget.position - BeginTarget.position).normalized * distance + BeginTarget.position;        
    }
}
