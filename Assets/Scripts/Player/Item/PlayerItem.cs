using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Icon Drag
    public static GameObject beingDraggedIcon;

    Vector3 startPos;

    [SerializeField] Transform onDragParent;

    [HideInInspector] public Transform startParent;

    public void SetOnDragParent()
    {
        onDragParent = transform.parent.parent.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beingDraggedIcon = gameObject;

        startPos = transform.position;
        startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        transform.SetParent(onDragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        beingDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == onDragParent)
        {
            transform.position = startPos;
            transform.SetParent(startParent);
        }
    }
    #endregion

    // 0 = itemcode, 1 = itemcount
    public int[] iteminfo;

    void Start()
    {
        iteminfo = new int[2] {1, 1};
    }

    public void UseItem()
    {
        if (iteminfo[1] > 0)
        {
            iteminfo[1]--;
            ItemEffect();
        }
        else
        {
            ItemEffect();
            Destroy(gameObject);
        }
    }

    public virtual void ItemEffect()
    {

    }
}