using System;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class Colonist : MonoBehaviour
{
    public Action<Colonist> taskCompleted;

    private List<Transform> _path = new List<Transform>();
    private List<Building> _checked = new List<Building>();
    private Building _currentBuilding;

    public void SetCurrentBuilding(Building current)
    {
        _currentBuilding = current;
    }

    public void SetTarget(Building building)
    {
        Debug.Log("Going to " + building.name);

        transform.position = new Vector3(building.transform.position.x, 0, building.transform.position.z);
    }
}