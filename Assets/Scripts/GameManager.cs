using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas mMainCanvas;

    public static GameManager Get { get; private set; }

    public Canvas MainCanvas => mMainCanvas;

    private void Awake()
    {
        Get = this;
    }
}
