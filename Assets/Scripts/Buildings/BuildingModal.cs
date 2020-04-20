using Colonists;
using Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingModal : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public GameObject colonistPreviewPrefab;
        public RectTransform colonistParent;
        public GameObject producesPreviewPrefab;
        public RectTransform producesParent;
        public TextMeshProUGUI error;
        public Button confirmButton;

        private Building _building;
        private int _assignedColonists;
        private bool _isOpen;

        public void Open(Building building)
        {
            _building = building;
            _isOpen = true;

            title.text = building.title;
            description.text = building.description;

            _assignedColonists = building.GetAssignedColonists();

            Rerender();

            gameObject.SetActive(true);
        }

        public void Close()
        {
            if (_isOpen)
            {
                PauseModal.Handled = true;
            }
            
            gameObject.SetActive(false);
            _isOpen = false;
        }

        private void Rerender()
        {
            // Spawn colonist prefabs
            for (var i = 0; i < colonistParent.childCount; i++)
            {
                Destroy(colonistParent.GetChild(i).gameObject);
            }

            for (var i = 0; i < _building.maxColonists; i++)
            {
                var icon = Instantiate(colonistPreviewPrefab, colonistParent);

                if (i < _assignedColonists)
                {
                    icon.GetComponent<Image>().color = Color.white;
                }
            }

            for (var i = 0; i < producesParent.childCount; i++)
            {
                Destroy(producesParent.GetChild(i).gameObject);
            }

            foreach (var producer in _building.GetProducer())
            {
                var produces = Instantiate(producesPreviewPrefab, producesParent);

                produces.transform.Find("Icon").GetComponent<Image>().sprite =
                    ResourceManager.Instance.GetSpriteForType(producer.resourceType);

                var amount = producer.GetAmountFor(_assignedColonists);
                var perSecond = 0f;

                if (amount.amount != 0)
                {
                    perSecond = amount.amount / (float) amount.interval;
                }

                produces.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text =
                    $"{perSecond.ToString("F2")} / second";
            }

            // Check if we have enough colonists
            var difference = _assignedColonists - _building.GetAssignedColonists();
            var hasError = difference > 0 && ColonistManager.Instance.GetUnemployedAmount() < difference;
            error.gameObject.SetActive(hasError);
            confirmButton.interactable = !hasError;
        }

        public void Confirm()
        {
            _building.ChangeColonistAmount(_assignedColonists);
            Close();
        }
        
        public void Increase()
        {
            _assignedColonists = Mathf.Clamp(_assignedColonists + 1, 0, _building.maxColonists);
            Rerender();
        }

        public void Decrease()
        {
            _assignedColonists = Mathf.Clamp(_assignedColonists - 1, 0, _building.maxColonists);
            Rerender();
        }

        public bool IsOpen() => _isOpen;
    }
}