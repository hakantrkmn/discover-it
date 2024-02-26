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
        //UpdateMission();
        if (obj==currentMissionElement)
        {
            UpdateMission();
        }
        discoverText.DOFade(1, 0);
        discoverText.text = obj.name + " is discovered";
        discoverText.DOFade(0, 1f).SetDelay(.3f);
        
    }

    void UpdateMission()
    {
        var data = Scriptable.LevelElementData();
        currentMissionElement = data.GetUndiscoveredElement();
        //var remain = data.GetCraftedElementAmount();
        //missionText.text = "Discovered " + remain+"/1532";
        missionText.text = "Discover " + currentMissionElement.name;

    }

   
}
