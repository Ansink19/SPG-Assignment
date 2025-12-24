using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementButtons : MonoBehaviour
{
    public GameElements Element;

    public void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelect);
    }

    public void OnSelect()
    {
        GameEventHandler.OnPlayerSelectElement?.Invoke(Element);
    }
}
