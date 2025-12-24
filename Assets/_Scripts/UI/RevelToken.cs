using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevelToken : MonoBehaviour
{
    [SerializeField] private Image ElementIcon;

    public void Init(GameElements element, List<ElementIconMap> elementIcons)
    {
        if (element == GameElements.None)
        {
            Debug.LogError("Invalid Element Passed");

        }
        ElementIconMap map = elementIcons.Find(x => x.Element == element);
        ElementIcon.sprite = map.Icon;
    }
}
