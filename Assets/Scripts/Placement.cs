using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placement : MonoBehaviour
{
    public Camera playerCamera;
    public Material material;
    public Material errorMaterial;
    public LayerMask terrainLayer;

    private bool _active = false;
    private bool _canPlace = false;
    private Blueprint _blueprint;

    public void Activate(GameObject prefab)
    {
        _active = true;

        _blueprint = Instantiate(prefab).GetComponent<Blueprint>();
        _blueprint.GetComponent<MeshRenderer>().material = material;
    }

    public void Build(InputAction.CallbackContext context)
    {
        if (_active && _canPlace)
        {
            _active = false;
            _canPlace = false;
            
            _blueprint.Spawn();
            
            Destroy(_blueprint.gameObject);
            _blueprint = null;
        }
    }

    private void Update()
    {
        if (!_active)
        {
            return;
        }

        MoveToMouse();
    }

    private void MoveToMouse()
    {
        var placefinder = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(placefinder, out hit, 3000, terrainLayer))
        {
            var blueprintTransform = _blueprint.transform;
            
            blueprintTransform.position = new Vector3(
                hit.point.x,
                _blueprint.offsetY,
                hit.point.z
            );

            var size = _blueprint.GetComponent<BoxCollider>().bounds.extents;
            _canPlace = !Physics.CheckBox(new Vector3(blueprintTransform.transform.position.x, .6f, blueprintTransform.transform.position.z), new Vector3(size.x, .5f, size.z));
            _blueprint.GetComponent<MeshRenderer>().material = _canPlace ? material : errorMaterial;
        }
    }
}