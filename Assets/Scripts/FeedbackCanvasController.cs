using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FeedbackCanvasController : MonoBehaviour
{
    public TextMeshProUGUI discoverText;

    private void OnEnable()
    {
        EventManager.ElementDiscovered += ElementDiscovered;
    }

    private void OnDisable()
    {
        EventManager.ElementDiscovered -= ElementDiscovered;
    }

    private void ElementDiscovered(Element obj)
    {
        AudioManager.instance.PlayDiscoverClip();
        discoverText.text = obj.name + " is discovered";
        discoverText.DOFade(0, 1f).SetDelay(.3f);
    }

   
}
