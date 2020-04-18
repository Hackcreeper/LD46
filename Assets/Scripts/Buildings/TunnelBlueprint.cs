using UnityEngine;

namespace Buildings
{
    public class TunnelBlueprint : Blueprint
    {
        public override Building Spawn()
        {
            var spawned = base.Spawn();

            spawned.GetComponent<MeshRenderer>().material.mainTextureScale =
                GetComponent<MeshRenderer>().material.mainTextureScale;
            
            return spawned;
        }
    }
}