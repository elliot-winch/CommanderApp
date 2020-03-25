using System;
using System.Collections;
using UnityEngine;

public abstract class LerpTo<T>
{
    public bool Continuous { get; set; }
    public T Target { get; set; }

    private MonoBehaviour mMono;
    private float mLerpFactor;

    private Coroutine mCoroutine;

    public LerpTo(MonoBehaviour mono, float lerpFactor)
    {
        mLerpFactor = lerpFactor;
        mMono = mono;
    }

    public void InitLerp(T startPosition, Action<T> onNewPosition)
    {
        Lerp(startPosition, startPosition, onNewPosition);
    }

    public void Lerp(T startPosition, T endPosition, Action<T> onNewPosition, Action onComplete = null)
    {
        StopLerp();

        Target = endPosition;

        mCoroutine = mMono.StartCoroutine(LerpToCoroutine(startPosition, onNewPosition, onComplete));
    }

    public void StopLerp()
    {
        if(mCoroutine != null)
        {
            mMono.StopCoroutine(mCoroutine);
        }

        mCoroutine = null;
    }

    private IEnumerator LerpToCoroutine(T startPosition, Action<T> onNewPosition, Action onComplete)
    {
        T currentPosition = startPosition;

        while (Continuous || ContinueCondition(currentPosition, Target))
        {
            currentPosition = LerpFunction(currentPosition, Target, mLerpFactor);

            onNewPosition?.Invoke(currentPosition);

            yield return null;
        }

        onComplete?.Invoke();
        mCoroutine = null;
    }

    protected abstract T LerpFunction(T current, T end, float factor);
    protected abstract bool ContinueCondition(T current, T end);
}

public class LerpToVector2 : LerpTo<Vector2>
{
    public float Epsilon { get; set; } = Mathf.Epsilon;

    public LerpToVector2(MonoBehaviour mono, float lerpFactor) : base(mono, lerpFactor) { }

    protected override bool ContinueCondition(Vector2 current, Vector2 end)
    {
        return Vector2.Distance(current, end) > Epsilon;
    }

    protected override Vector2 LerpFunction(Vector2 current, Vector2 end, float factor)
    {
        return Vector2.Lerp(current, end, factor);
    }
}


public class LerpToFloat : LerpTo<float>
{
    public float Epsilon { get; set; } = Mathf.Epsilon;

    public LerpToFloat(MonoBehaviour mono, float lerpFactor) : base(mono, lerpFactor) { }

    protected override bool ContinueCondition(float current, float end)
    {
        return Mathf.Abs(current - end) > Epsilon;
    }

    protected override float LerpFunction(float current, float end, float factor)
    {
        return Mathf.Lerp(current, end, factor);
    }
}

