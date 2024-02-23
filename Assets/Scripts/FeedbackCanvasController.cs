using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FeedbackCanvasController : MonoBehaviour
{
    public TextMeshProUGUI discoverText;
    public TextMeshProUGUI missionText;

    private Element currentMissionElement;

    private void Start()
    {
        UpdateMission();
    }

    private void OnEnable()
    {
        EventManager.ElementDiscovered += ElementDiscovered;
        EventManager.ElementCrafted += ElementCrafted;
    }

    private void ElementCrafted(Element obj)
    {
        
    }

    private void OnDisable()
    {
        EventManager.ElementCrafted -= ElementCrafted;
        EventManager.ElementDiscovered -= ElementDiscovered;
    }

    private void ElementDiscovered(Element obj)
    {
        AudioManager.instance.PlayDiscoverClip();
        discoverText.text = obj.name + " is discovered";
        discoverText.DOFade(0, 1f).SetDelay(.3f);
        if (obj == currentMissionElement)
        {
            UpdateMission();
        }
    }

    void UpdateMission()
    {
        var data = Scriptable.LevelElementData();
        currentMissionElement = data.GetUndiscoveredElement();
        missionText.text = "Discover " + currentMissionElement.name;
    }

   
}
