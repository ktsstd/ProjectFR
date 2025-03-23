using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapCamera : MonoBehaviour
{
    GameObject playerObj;
    public Vector3 center = Vector3.zero; 
    public float radius = 5f;

    private void Start()
    {
        FindLocalPlayer();
    }

    private void Update()
    {
        if (playerObj == null)
        {
            FindLocalPlayer();
        }
    }

    private void LateUpdate()
    {
        if (playerObj != null)
        {
            Vector3 newPosition = playerObj.transform.position;

            newPosition.y = transform.position.y;

            Vector3 offset = new Vector3(newPosition.x - center.x, 0, newPosition.z - center.z);
            if (offset.magnitude > radius)
            {
                offset = offset.normalized * radius;
                newPosition = center + offset;
                newPosition.y = transform.position.y;
            }

            transform.position = newPosition;
        }
    }
    void FindLocalPlayer()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = obj.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                playerObj = obj;
                break;
            }
        }
    }
}
