using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SPG/ElementMap", fileName = "ElementMap")]
public class ElementMapSO : ScriptableObject
{
    public List<ElementWinMap> ElementWins;
    public List<ElementIconMap> ElementIcons;

}

[Serializable]
public class ElementWinMap
{
    public GameElements Element;
    public List<GameElements> WinsAgainst;
}

[Serializable]
public class ElementIconMap
{
    public GameElements Element;
    public Sprite Icon;
}
