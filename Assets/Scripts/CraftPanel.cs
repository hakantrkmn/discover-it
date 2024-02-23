using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftPanel : MonoBehaviour
{
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
        var el = Instantiate(elementPrefab, EventManager.GetCurrentElement().position,quaternion.identity,transform).GetComponent<ElementController>();
        el.SetElement(element);
        el.state = ElementState.WaitingForCraft;
    }

  

   
    
}
