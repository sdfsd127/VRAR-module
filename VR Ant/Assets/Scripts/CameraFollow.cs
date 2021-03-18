using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject targetObject;
    private Vector3 initialOffset;

    private void Start()
    {
        targetObject = GameObject.Find("Player Ant");
        initialOffset = transform.position - targetObject.transform.position;
    }

    private void Update()
    {
        transform.position = targetObject.transform.position + initialOffset;
    }
}
