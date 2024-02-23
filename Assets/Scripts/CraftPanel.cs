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
    public Transform currentElement;
    public GameObject elementPrefab;
    private void OnEnable()
    {
        EventManager.ElementCrafted += ElementCrafted;
        EventManager.ElementClicked += ElementClicked;
    }

    private void ElementCrafted(Element element)
    {
        var el = Instantiate(elementPrefab, currentElement.position,quaternion.identity,transform).GetComponent<ElementController>();
        el.SetElement(element);

        el.state = ElementState.WaitingForCraft;
    }

    private void OnDisable()
    {
        EventManager.ElementCrafted -= ElementCrafted;
        EventManager.ElementClicked -= ElementClicked;
    }

    private void ElementClicked(ElementController obj)
    {
        if (obj.state == ElementState.OnElementTable)
        {
            var element = Instantiate(elementPrefab, transform).GetComponent<ElementController>();
            element.SetElement(obj.elementData);
            element.state = ElementState.Dragging;
            currentElement = element.transform;
        }
        else if (obj.state == ElementState.WaitingForCraft)
        {
            obj.state = ElementState.Dragging;
            currentElement = obj.transform;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField]  GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
 
    

    // Update is called once per frame
    void Update()
    {
        

        if (currentElement != null)
        {
            if (Input.GetMouseButton(0))
            {
                currentElement.position = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentElement.GetComponent<Image>().raycastTarget = true;
                m_PointerEventData = new PointerEventData(EventSystem.current);
                m_PointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);

                if (results.Any(x => x.gameObject.GetComponent<ElementController>() && x.gameObject!=currentElement.gameObject))
                {
                    var element = results.First(x => x.gameObject.GetComponent<ElementController>() && x.gameObject!=currentElement.gameObject).gameObject.GetComponent<ElementController>();
                    var craftedElement = currentElement.GetComponent<ElementController>()
                        .CheckElementCanMerge(element.elementData);
                   

                    if (craftedElement != null)
                    {
                        EventManager.ElementCrafted(craftedElement);
                        Destroy(element.gameObject);
                        Destroy(currentElement.gameObject);
                        currentElement = null;
                    }
                }
                else if (results.Any(x => x.gameObject.GetComponent<CraftPanel>()))
                {
                    var craftPanel = results.First(x => x.gameObject.GetComponent<CraftPanel>()).gameObject.GetComponent<CraftPanel>();

                    currentElement.GetComponent<ElementController>().state = ElementState.WaitingForCraft;
                    currentElement = null;
                }
                else if (results.Any(x => x.gameObject.GetComponent<ElementPanel>()))
                {
                    var elementPanel = results.First(x => x.gameObject.GetComponent<ElementPanel>()).gameObject.GetComponent<ElementPanel>();
                    Destroy(currentElement.gameObject);
                    currentElement = null;
                }
                

            }
        }
    }
}
