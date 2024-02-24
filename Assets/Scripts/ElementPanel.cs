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
        var data = Scriptable.LevelElementData();

        if (!data.craftedElements.Contains(element))
        {
            var el = Instantiate(elementPrefab, content).GetComponent<ElementController>();
            el.SetElement(element);
        

            data.craftedElements.Add(element);
            ES3.Save("data",data);
            EventManager.ElementDiscovered(element);
        }
    }

    private void Start()
    {
        CreateStartElement();
    }

    private void CreateStartElement()
    {
        var data = Scriptable.LevelElementData();
        if (!ES3.KeyExists("data"))
        {
            ES3.Save("data",data);
        }
        data = ES3.Load<LevelElementData>("data");
        foreach (var element in data.startElements)
        {
            var el = Instantiate(elementPrefab, content).GetComponent<ElementController>();
            el.SetElement(element);

        }
        foreach (var element in data.craftedElements)
        {
            var el = Instantiate(elementPrefab, content).GetComponent<ElementController>();
            el.SetElement(element);

        }
    }
}
