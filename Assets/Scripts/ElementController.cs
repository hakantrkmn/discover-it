using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementController : MonoBehaviour ,IBeginDragHandler , IDragHandler , IPointerDownHandler , IPointerUpHandler , IPointerEnterHandler,IPointerExitHandler
{
    public ElementState state;
    public TextMeshProUGUI elementName;
    public Image elementImage;
    public Element elementData;


    private void OnEnable()
    {
        EventManager.MouseUp += MouseUp;
    }

    private void OnDisable()
    {
        EventManager.MouseUp -= MouseUp;
    }

    private void MouseUp()
    {
        transform.DOScale(1, .3f).SetEase(Ease.OutBounce);
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
   

    public void OnBeginDrag(PointerEventData eventData)
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

    public void OnDrag(PointerEventData eventData)
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == ElementState.WaitingForCraft)
        {
            transform.DOScale(1.2f, .3f).SetEase(Ease.OutBounce);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == ElementState.WaitingForCraft)
        {
            transform.DOScale(1f, .3f).SetEase(Ease.OutBounce);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state == ElementState.WaitingForCraft)
        {
            transform.DOScale(1.2f, .3f).SetEase(Ease.OutBounce);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (state == ElementState.WaitingForCraft)
        {
            transform.DOScale(1f, .3f).SetEase(Ease.OutBounce);
        }
    }
}
