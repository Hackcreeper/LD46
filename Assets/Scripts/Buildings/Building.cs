using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        private int _activePopups = 0;

        public float AddPopup()
        {
            return .2f * _activePopups++;
        }

        public void RemovePopup()
        {
            Debug.Log("Remove");
            --_activePopups;
        }
    }
}