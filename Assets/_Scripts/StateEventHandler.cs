using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventHandler 
{
    public static Action<World> OnWorldInit;
    public static Action<GameState> OnGameStateChange;
    public static Action<GameElements> OnPlayerSelectElement;
}
