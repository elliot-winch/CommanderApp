using System;
using System.Collections;
using UnityEngine;

public interface IClickable
{
    void OnMouseOverBegin();
    void OnMouseOverEnd();
    void OnLeftClickStart();
    void OnLeftClickMoved(Vector2 pixelPosition);
    void OnLeftClickEnd();
}

public class MouseSelect : MonoBehaviour
{
    public LayerMask layer;
    public float maxDist = Mathf.Infinity;
    public float doubleClickTime = 0.2f;
    public new Camera camera;

    private IClickable current;
    private Coroutine clickCo;

    void Update()
    {
        if(camera == null)
        {
            return;
        }

        Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, maxDist);

        IClickable hitThisFrame = hitInfo.transform?.GetComponent<IClickable>();

        //MouseOver
        if(hitThisFrame != current)
        {
            current?.OnMouseOverEnd();

            hitThisFrame?.OnMouseOverBegin();
        }

        current = hitThisFrame;

        //Clicks
        if (hitThisFrame != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hitThisFrame.OnLeftClickStart();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                hitThisFrame.OnLeftClickEnd();
            }
            else
            {
                hitThisFrame.OnLeftClickMoved(Input.mousePosition);
            }
        }
    }
}
