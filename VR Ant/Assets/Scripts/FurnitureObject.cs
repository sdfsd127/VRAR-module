using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureObject")]
public class FurnitureObject : ScriptableObject
{
    public GameObject prefab;
    public ROOM room;

    public new string name;
    public float price;
}