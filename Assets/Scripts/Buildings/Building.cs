using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        private int _activePopups = 0;
        private readonly List<Connection> _connections = new List<Connection>();

        public void RegisterConnection(Building other, Tunnel tunnel)
        {    
            _connections.Add(new Connection() {building = other, tunnel = tunnel});
        }

        public Connection[] GetConnections() => _connections.ToArray();

        public float AddPopup()
        {
            return .2f * _activePopups++;
        }

        public void RemovePopup()
        {
            --_activePopups;
        }
    }
}