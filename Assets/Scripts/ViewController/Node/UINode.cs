using UnityEngine;

public class UINode : MonoBehaviour
{
    [SerializeField]
    private float mPositionLerpFactor;
    [SerializeField]
    private float mRotationLerpFactor;
    [SerializeField]
    private RectTransform mRect;

    private LerpToVector2 mPositionLerpTo;
    private LerpToVector2 mRotationLerpTo;
    private Vector2 mOffset;

    void Awake()
    {
        mPositionLerpTo = new LerpToVector2(this, mPositionLerpFactor);
        mRotationLerpTo = new LerpToVector2(this, mRotationLerpFactor)
        {
            Continuous = true
        };

        mRotationLerpTo.InitLerp(-mRect.up, SetFacingDirection);
    }

    public void OnDragStart()
    {
        mPositionLerpTo.StopLerp();
        mOffset = GetAdjustedMousePosition() - mRect.anchoredPosition;
    }

    public void OnDrag()
    {
        Vector2 newPosition = GetAdjustedMousePosition() - mOffset;

        //Modifies mRect.anchoredPosition
        SetPosition(newPosition);

        //Lerp Rotation to face nearest edge
        Vector2 newFacingDirection = Helper.NearestEdgePosition(newPosition) - newPosition;

        //When the mouse is outside of the screen, we want to ensure that the node remains pointed
        //in the right direction. We can do this by comparing the newFacingDirection and the vector
        //from the canvas origin to the new position. If they are pointed towards each other (dot > 0), then
        //the new position is inside the canvas. If not, then we need to reverse the facing direction.
        if(Vector2.Dot(newFacingDirection, newPosition) < 0f)
        {
            newFacingDirection *= -1f;
        }

        mRotationLerpTo.Target = newFacingDirection;
    }

    public void OnDragEnd()
    {
        //Lerp to nearest edge
        mPositionLerpTo.Lerp(mRect.anchoredPosition, Helper.NearestEdgePosition(mRect.anchoredPosition), SetPosition);
    }

    private Vector2 GetAdjustedMousePosition()
    {
        return new Vector2()
        {
            x = Input.mousePosition.x - Screen.width / 2f,
            y = Input.mousePosition.y - Screen.height / 2f
        };
    }

    private void SetPosition(Vector2 dragPosition)
    {
        mRect.anchoredPosition = new Vector2()
        {
            x = Mathf.Clamp(dragPosition.x, -(Screen.width / 2f), Screen.width / 2f),
            y = Mathf.Clamp(dragPosition.y, -(Screen.height / 2f), Screen.height / 2f),
        };
        
    }

    /// <summary>
    /// Sets which way the tip (at the bottom) of the node faces
    /// </summary>
    /// <param name="facingDirection"></param>
    private void SetFacingDirection(Vector2 facingDirection)
    {
        mRect.up = -facingDirection;
        //There's an issue where setting 'up' flips the element such that
        //forward = -forward. This ensures the rotation is only in the z component
        mRect.rotation = Quaternion.Euler(new Vector3()
        {
            z = mRect.eulerAngles.z
        });
    }
}
