using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas mMainCanvas;

    [Header("Prefabs")]
    [SerializeField]
    private PlayerNode mNodePrefab;
    [SerializeField]
    private CommanderDamageArc mCommanderDamagePrefab;
    [SerializeField]
    private int mBackgroundChildrenCount;

    [Header("Scaling")]
    [SerializeField]
    private float mNodeScaleFactor;

    public static GameManager Get { get; private set; }

    public Canvas MainCanvas => mMainCanvas;
    public GameInfo CurrentGame { get; private set; }

    private Dictionary<PlayerInfo, PlayerNode> mNodes;
    private List<CommanderDamageArc> mArcs;

    private void Awake()
    {
        Get = this;
    }

    public void SetGame(GameInfo game)
    {
        CurrentGame = game;
        List<PlayerInfo> players = game.Players;

        mNodes = new Dictionary<PlayerInfo, PlayerNode>();

        foreach (PlayerInfo player in players)
        {
            PlayerNode bubble = Instantiate(mNodePrefab);
            bubble.transform.SetParent(MainCanvas.transform);
            bubble.RectTransform.anchoredPosition = Vector2.zero;

            bubble.SetPlayer(player);

            mNodes[player] = bubble;
        }

        mArcs = new List<CommanderDamageArc>();

        for (int i = 0; i < players.Count - 1; i++)
        {
            for (int j = i + 1; j < players.Count; j++)
            {
                PlayerNode playerBubble = mNodes[players[i]];
                PlayerNode otherPlayerBubble = mNodes[players[j]];

                CommanderDamageArc arc = Instantiate(mCommanderDamagePrefab);
                arc.transform.SetParent(MainCanvas.transform);
                //Ensure the arcs render behind the nodes but in front of the background
                arc.transform.SetSiblingIndex(mBackgroundChildrenCount);

                arc.BeginTarget = playerBubble.VisualRectTransform;
                arc.EndTarget = otherPlayerBubble.VisualRectTransform;

                arc.SetPlayers(players[i], players[j]);

                mArcs.Add(arc);
            }
        }

        Scale();
    }

    public void Scale()
    {
        int playerCount = CurrentGame.Players.Count;

        //We want to scale the nodes such that they maximise the space available.

        //We adjust the size based on the number of players -> for 4 or less,
        //a bubble can take up an entire side of the screen. For more than 4, 
        //we have to let multiple bubbles fit on one screen side.
        float playerNumberFactor = 1f / (1f + (playerCount % 4));

        //We use the shorter of width / height to guarentee the bubble will fit
        //Some scalar < 1 is applied to make the screen less cluttered
        float nodeScale = Mathf.Min(Screen.width, Screen.height) * playerNumberFactor * mNodeScaleFactor;

        foreach(PlayerNode node in mNodes.Values)
        {
            node.RectTransform.sizeDelta = Vector2.one * nodeScale;
        }

        mArcs.ForEach(arc => arc.Scale(nodeScale));   
    }
}
