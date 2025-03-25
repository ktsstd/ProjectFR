using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical : MonoBehaviour
{
    public float power = 0f;
    public float clampDist = 0.03f;

    Transform parentTransform;
    Rigidbody boneRigidbody;
    Vector3 prevFrameParentPosition = Vector3.zero;

    void Start()
    {
        parentTransform = transform.parent;
        prevFrameParentPosition = parentTransform.position;

        boneRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 delta = (prevFrameParentPosition - parentTransform.position);
        boneRigidbody.AddForce(Vector3.ClampMagnitude(delta, clampDist) * power);

        prevFrameParentPosition = parentTransform.position;
    }
}

