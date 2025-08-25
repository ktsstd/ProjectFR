using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code : MonoBehaviour
{
    [SerializeField] PlayerController playerS;
    // Start is called before the first frame update
    void Start()
    {
        playerS.elementalCode = 1;
    }
}
