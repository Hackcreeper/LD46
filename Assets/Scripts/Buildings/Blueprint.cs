using UnityEngine;

namespace Buildings
{
    public class Blueprint : MonoBehaviour
    {
        public float offsetY = 0f;
        public GameObject building;
        public int costs;

        public virtual Building Spawn()
        {
            var spawned = Instantiate(building);
            var transform1 = transform;
            
            spawned.transform.position = transform1.position;
            spawned.transform.rotation = transform1.rotation;
            spawned.transform.localScale = transform1.localScale;

            return spawned.GetComponent<Building>();
        }
    }
}