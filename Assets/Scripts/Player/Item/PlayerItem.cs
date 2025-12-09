using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerItem : MonoBehaviourPunCallbacks, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    public TextMeshProUGUI useItemText;

    public virtual void Start()
    {
        ShowUseItem();
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }

    public virtual void UseItem(int _player, Vector3 _usePos)
    {
        if (iteminfo[1] > 1)
        {
            iteminfo[1]--;
            ShowUseItem();
            ItemEffect(_player, _usePos);
        }
        else
        {
            ItemEffect(_player, _usePos);
            Destroy(gameObject);
        }
    }

    public void ShowUseItem()
    {
        useItemText.text = iteminfo[1].ToString();
    }

    public virtual void ItemEffect(int _player, Vector3 _usePos) { }
}