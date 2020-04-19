using UnityEngine;

namespace Buildings
{
    public class TunnelBlueprint : Blueprint
    {
        public override Building Spawn()
        {
            var spawned = base.Spawn();

            spawned.GetComponentInChildren<MeshRenderer>().material.mainTextureScale =
                GetComponentInChildren<MeshRenderer>().material.mainTextureScale;
            
            return spawned;
        }
    }
}