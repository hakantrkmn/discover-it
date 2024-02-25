using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementMovement : MonoBehaviour
{
    public GameObject elementPrefab;
    public Transform currentElement;
    public Transform craftPanel;

    private void OnEnable()
    {
        EventManager.GetCurrentElement += () => currentElement;
        EventManager.ElementClicked += ElementClicked;
    }


    private void OnDisable()
    {
        EventManager.GetCurrentElement -= () => currentElement;
        EventManager.ElementClicked -= ElementClicked;
    }

    private void ElementClicked(ElementController obj)
    {
        if (obj.state == ElementState.OnElementTable)
        {
            var element = Instantiate(elementPrefab, obj.transform.position, quaternion.identity, craftPanel)
                .GetComponent<ElementController>();
            element.SetElement(obj.elementData);
            element.state = ElementState.Dragging;
            currentElement = element.transform;
            element.BounceUp();
            element.GetComponent<Image>().raycastTarget = false;
        }
        else if (obj.state == ElementState.WaitingForCraft)
        {
            obj.state = ElementState.Dragging;
            currentElement = obj.transform;
            obj.BounceUp();
        }
    }

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;


    public Vector3 moveOffset;

    // Update is called once per frame
    void Update()
    {
        if (currentElement != null)
        {
            if (Input.GetMouseButton(0))
            {
                currentElement.position = Vector3.Lerp(currentElement.position, Input.mousePosition + moveOffset,
                    15 * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentElement.GetComponent<ElementController>().BounceDown();
                currentElement.GetComponent<Image>().raycastTarget = true;
                m_PointerEventData = new PointerEventData(EventSystem.current);
                m_PointerEventData.position = currentElement.position;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);

                if (results.Any(x =>
                        x.gameObject.GetComponent<ElementController>() && x.gameObject != currentElement.gameObject))
                {
                    if (results.Any(x => x.gameObject.GetComponent<ElementPanel>()))
                    {
                        var elementPanel = results.First(x => x.gameObject.GetComponent<ElementPanel>()).gameObject
                            .GetComponent<ElementPanel>();
                        Destroy(currentElement.gameObject);
                        currentElement = null;
                    }
                    else
                    {
                        var element = results
                            .First(x => x.gameObject.GetComponent<ElementController>() &&
                                        x.gameObject != currentElement.gameObject).gameObject
                            .GetComponent<ElementController>();
                        var craftedElement = currentElement.GetComponent<ElementController>()
                            .CheckElementCanMerge(element.elementData);


                        if (craftedElement != null)
                        {
                            EventManager.ElementCrafted(craftedElement);
                            Destroy(element.gameObject);
                            Destroy(currentElement.gameObject);
                            currentElement = null;
                        }
                        else
                        {
                            currentElement.GetComponent<ElementController>().state = ElementState.WaitingForCraft;
                            currentElement = null;
                        }
                    }
                }
                else if (results.Any(x => x.gameObject.GetComponent<CraftPanel>()))
                {
                    var craftPanel = results.First(x => x.gameObject.GetComponent<CraftPanel>()).gameObject
                        .GetComponent<CraftPanel>();

                    currentElement.GetComponent<ElementController>().state = ElementState.WaitingForCraft;
                    currentElement = null;
                }
                else if (results.Any(x => x.gameObject.GetComponent<ElementPanel>()))
                {
                    var elementPanel = results.First(x => x.gameObject.GetComponent<ElementPanel>()).gameObject
                        .GetComponent<ElementPanel>();
                    Destroy(currentElement.gameObject);
                    currentElement = null;
                }
                EventManager.MouseUp();

            }
        }
    }
}