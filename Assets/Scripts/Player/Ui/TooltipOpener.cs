using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipOpener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject skillTooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillTooltip.SetActive(false);
    }
}
