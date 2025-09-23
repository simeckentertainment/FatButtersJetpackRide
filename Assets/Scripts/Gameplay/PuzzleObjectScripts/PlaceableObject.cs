using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Placeable Object", menuName = "Fatbutters/PlaceableObject", order = 0)]
[Serializable]
public class PlaceableObject : ScriptableObject {
    public string Name;
    public string ID;
    //public DataType type;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public GameObject model;

    public enum DataType {statics,staticsScifi,dynamics,terrains,enemies}


}

