using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUi : MonoBehaviour
{
    public GameObject[] ItemSlotObject;
    public GameObject[] Items;

    public GameObject[] HaveItem()
    {
        GameObject[] items = new GameObject[ItemSlotObject.Length];

        for (int i = 0; i < ItemSlotObject.Length; i++)
        {
            if (ItemSlotObject[i].transform.childCount > 0)
                items[i] = ItemSlotObject[i].transform.GetChild(0).gameObject;
            else
                items[i] = null;
        }
        return items;
    }

    public void AddItem(int _num)
    {
        // n원 소비, 없으면 리턴 추가하기
        if (_num == 1)
        {
            GameObject[] items = HaveItem();
            foreach (GameObject item in items)
            {
                Debug.Log(item);
                if (item == null)
                    continue;

                PlayerItem iteminfo = item.GetComponent<PlayerItem>();
                if (iteminfo.iteminfo[0] == 1)
                    if (iteminfo.iteminfo[1] < 3)
                    {
                        iteminfo.iteminfo[1]++;
                        return;
                    }
            }
        }

        for (int i = 0; i < ItemSlotObject.Length; i++)
        {
            if (ItemSlotObject[i].transform.childCount == 0)
            {
                GameObject itme = Instantiate(Items[_num]);
                itme.transform.parent = ItemSlotObject[i].transform;
                itme.transform.position = ItemSlotObject[i].transform.position;
                itme.GetComponent<PlayerItem>().SetOnDragParent();
                return;
            }
        }
    }
}
