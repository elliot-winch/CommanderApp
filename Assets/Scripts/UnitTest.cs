using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour
{
    public PlayerNode nodePrefab;
    public CommanderDamageArc commanderDamageArcPrefab;

    private void Start()
    {
        GameInfo game = Test_Game();

        GameManager.Get.SetGame(game);
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
}
