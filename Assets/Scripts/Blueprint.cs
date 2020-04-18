using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public float offsetY = 0f;
    public GameObject building;

    public void Spawn()
    {
        var spawned = Instantiate(building);
        spawned.transform.position = transform.position;
    }
}