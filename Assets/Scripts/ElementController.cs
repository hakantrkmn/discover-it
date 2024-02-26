using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class ElementController : MonoBehaviour ,IPointerDownHandler
{
    public ElementState state;
    public TextMeshProUGUI elementName;
    public Image elementImage;
    public Element elementData;
    public Image image;

    public void ErrorFeedback()
    {
        image.DOColor(Color.red, .1f).SetEase(Ease.OutBounce).SetLoops(2, LoopType.Yoyo);
    }

    public void SetElement(Element data)
    {
        elementData = data;
        elementName.text = data.name;
        elementImage.sprite = data.image;
        transform.localScale = Vector3.zero;
        transform.DOScale(1, .3f).SetEase(Ease.OutBounce);
    }

    public Element CheckElementCanMerge(Element mergeElement)
    {
        foreach (var merge in elementData.mergeTable)
        {
            if (merge.mergeWith.name == mergeElement.name)
            {
                return merge.outcome;
            }
        }
        foreach (var merge in mergeElement.mergeTable)
        {
            if (merge.mergeWith.name == elementData.name)
            {
                return merge.outcome;
            }
        }

        return null;
    }

    Vector3 direction;
    float timer;
    private void Update()
    {
        if (state==ElementState.WaitingForCraft)
        {
            timer += Time.deltaTime;
            if (timer>2f)
            {
                direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),0);
                timer = 0;
            }

            transform.position += direction * (Time.deltaTime * 10);
        }
    }

    private void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),0);
    }

    public void BounceUp()
    {
        DOTween.Kill("Bounce");
        transform.DOScale(1.1f, .3f).SetEase(Ease.OutBounce).SetId("Bounce");
    }
    public void BounceDown()
    {

        DOTween.Kill("Bounce");
        transform.DOScale(1f, .3f).SetEase(Ease.OutBounce).SetId("Bounce");
    }
    
  
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (state)
        {
            case ElementState.OnElementTable:
                EventManager.ElementClicked(this);
                break;
            case ElementState.WaitingForCraft:
                transform.SetAsLastSibling();
                EventManager.ElementClicked(this);
                GetComponent<Image>().raycastTarget = false;
                break;
        }
        AudioManager.instance.PlaySelectClip();
    }
}
