using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour
{
    public PlayerInfoNode nodePrefab;
    public CommanderDamageArc commanderDamageArcPrefab;

    [SerializeField]
    private float mBubbleScalar;

    private float mBubbleSize;

    private void Start()
    {
        GameInfo game = Test_Game();

        mBubbleSize = CalculateBubbleSize(game.Players.Count);
        SetUpBubbles(game.Players);
    }

    private GameInfo Test_Game()
    {
        GameInfo game = new GameInfo();

        //game.AddPlayer(new PlayerInfo("Steve", 20));
        game.AddPlayer(new PlayerInfo("Dave", 20));
        game.AddPlayer(new PlayerInfo("Craig", 20));
        game.AddPlayer(new PlayerInfo("Nyugen", 20));
        game.AddPlayer(new PlayerInfo("Yesk", 20));

        return game;
    }

    //TODO: move me
    private void SetUpBubbles(List<PlayerInfo> players)
    {
        Dictionary<PlayerInfo, PlayerInfoNode> playerBubbles = new Dictionary<PlayerInfo, PlayerInfoNode>();

        foreach (PlayerInfo player in players)
        {
            PlayerInfoNode bubble = Instantiate(nodePrefab);
            bubble.transform.SetParent(GameManager.Get.MainCanvas.transform);
            bubble.RectTransform.anchoredPosition = Vector2.zero;
            bubble.RectTransform.sizeDelta = Vector2.one * mBubbleSize;

            bubble.SetPlayer(player);

            playerBubbles[player] = bubble;
        }

        for(int i = 0; i < players.Count - 1; i++)
        {
            for(int j = i + 1; j < players.Count; j++)
            {         
                PlayerInfoNode playerBubble = playerBubbles[players[i]];
                PlayerInfoNode otherPlayerBubble = playerBubbles[players[j]];

                CommanderDamageArc arc = Instantiate(commanderDamageArcPrefab);
                arc.transform.SetParent(GameManager.Get.MainCanvas.transform);
                arc.transform.SetAsFirstSibling();

                arc.BeginTarget = playerBubble.VisualRectTransform;
                arc.EndTarget = otherPlayerBubble.VisualRectTransform;

                arc.SetPlayers(players[i], players[j]);
            }
        }
    }

    private float CalculateBubbleSize(int playerCount)
    {
        //We want to scale the bubbles such that they maximise the space available.

        //We adjust the size based on the number of players -> for 4 or less,
        //a bubble can take up an entire side of the screen. For more than 4, 
        //we have to let multiple bubbles fit on one screen side.
        float playerNumberFactor = 1f / (1f + (playerCount % 4));

        //We use the shorter of width / height to guarentee the bubble will fit
        //Some scalar < 1 is applied to make the screen less cluttered
        return Mathf.Min(Screen.width, Screen.height) * playerNumberFactor * mBubbleScalar;
    }
}
