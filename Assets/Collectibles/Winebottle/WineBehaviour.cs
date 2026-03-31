using Collectibles.Scripts;
using UnityEngine;
using Utils;

public class WineBehaviour : CollectibleBase
{
    protected override void TriggerBehaviour()
    {
            // 1. set magic variable in GameData TRUE 
            GameData.ReversedCommands.SetValue(true);
    }
}