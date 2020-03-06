using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandableController : MonoBehaviour
{
    [SerializeField]
    private GameObject mExpandedParent;
    [SerializeField]
    private GameObject mCollapsedParent;
    [SerializeField]
    private GameObject[] mDoNotCloseOnPress;

    public bool Expanded { get; private set; }

    private void Start()
    {
        SetExpanded(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && Expanded)
        {
            GameObject currentMouse = EventSystem.current.currentSelectedGameObject;

            //We want to close this UI on a click that doesn't target one of its buttons 
            //(ie anywhere else on the screen)
            if(mDoNotCloseOnPress.Any(gameObject => gameObject == currentMouse) == false)
            {
                SetExpanded(false);
            }
        }
    }

    public void SetExpanded(bool expand)
    {
        Expanded = expand;

        mExpandedParent.SetActive(expand);
        mCollapsedParent.SetActive(expand == false);

        //TODO: lerp
    }


}
