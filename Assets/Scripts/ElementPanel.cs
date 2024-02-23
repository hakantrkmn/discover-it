using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ElementPanel : MonoBehaviour
{
    public Transform content;
    public GameObject elementPrefab;


    private void OnEnable()
    {
        EventManager.ElementCrafted += ElementCrafted;
    }


    private void OnDisable()
    {
        EventManager.ElementCrafted -= ElementCrafted;
    }

    private void ElementCrafted(Element element)
    {
        var el = Instantiate(elementPrefab, content).GetComponent<ElementController>();
        el.SetElement(element);
        el.GetComponent<Image>().raycastTarget = false;

        var data = Scriptable.LevelElementData();
        if (!data.craftedElements.Contains(element))
        {
            data.craftedElements.Add(element);
            EventManager.ElementDiscovered(element);
        }
    }

    private void Start()
    {
        CreateStartElement();
    }

    void CreateStartElement()
    {
        var data = Scriptable.LevelElementData();
        foreach (var element in data.startElements)
        {
            var el = Instantiate(elementPrefab, content).GetComponent<ElementController>();
            el.SetElement(element);

        }
    }
}
