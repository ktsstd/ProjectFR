using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    GameObject Icon()
    {
        if (transform.childCount > 0)
            return transform.GetChild(0).gameObject;
        else 
            return null;
    }

    
    public void OnDrop(PointerEventData eventData)
    {
        if (Icon() == null)
        {
            PlayerItem.beingDraggedIcon.transform.SetParent(transform);
            PlayerItem.beingDraggedIcon.transform.position = transform.position;
        }
        else
        {

        }
    }
}
