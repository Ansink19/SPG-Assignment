using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevealToken : MonoBehaviour
{
    [SerializeField] private Image ElementIcon;

    public void Init(GameElements element, List<ElementIconMap> elementIcons)
    {
        if (element == GameElements.None)
        {
            return;
        }
        ElementIconMap map = elementIcons.Find(x => x.Element == element);
        ElementIcon.sprite = map.Icon;
    }

    public void ResetSprite()
    {
        ElementIcon.sprite = null;
    }
}
