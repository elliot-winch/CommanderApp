using System;
using System.Collections.Generic;
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
    [Header("Lerp")]
    [SerializeField]
    private List<RectTransform> mLerpTransforms;
    [SerializeField]
    private float mLerpFactor = 0.2f;
    [SerializeField]
    private float mLerpEpsilon = 0.001f;

    private Dictionary<RectTransform, LerpToVector2> mCurrentLerps;

    public bool Expanded { get; private set; }

    private bool mIsCollapsing = true;

    private void Awake()
    {
        mCurrentLerps = new Dictionary<RectTransform, LerpToVector2>();

        foreach(RectTransform transform in mLerpTransforms)
        {
            mCurrentLerps[transform] = new LerpToVector2(this, mLerpFactor)
            {
                Epsilon = mLerpEpsilon,
            };
        }
    }

    private void Start()
    {
        SetCollasped();
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
                SetCollasped();
            }
        }
    }

    public void SetCollasped()
    {
        Expanded = false;

        Debug.Log("COllapsed");

        mCollapsedParent.SetActive(true);

        MultiWait multiWait = new MultiWait(mLerpTransforms.Count)
        {
            OnComplete = () => mExpandedParent.SetActive(false)
        };

        LerpTransforms(Vector2.zero, multiWait.CouldComplete);
    }

    public void SetExpanded()
    {
        Expanded = true;

        Debug.Log("Expanded");

        mCollapsedParent.SetActive(false);
        mExpandedParent.SetActive(true);

        LerpTransforms(Vector2.one);
    }

    private void LerpTransforms(Vector2 target, Action onComplete = null)
    {
        foreach (RectTransform transform in mLerpTransforms)
        {
            mCurrentLerps[transform].StopLerp();

            mCurrentLerps[transform].Lerp(transform.localScale, target, (newValue) =>
            {
                transform.localScale = newValue;
            }, 
            onComplete);
        }
    }
}
