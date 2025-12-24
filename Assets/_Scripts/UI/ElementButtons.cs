using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementButtons : MonoBehaviour
{
    public GameElements Element;

    public void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelect);
        GetComponentInChildren<TextMeshProUGUI>().text = $"{Element}";
    }

    public void OnSelect()
    {
        GameEventHandler.OnPlayerSelectElement?.Invoke(Element);
    }
}
