using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalPosition : MonoBehaviour
{
    public Vector3 originalPosition {  get; private set; }
    public Quaternion originalRotation { get; private set; }
    void Awake()
    {
        originalPosition = transform.localPosition;
    }
}
