using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placement : MonoBehaviour
{
    public Camera playerCamera;

    public Material blueprintMaterial;
    public Material errorMaterial;

    public LayerMask terrainLayer;
    public LayerMask ignoreBlueprintLayer;
    
    public GameObject tunnelBlueprintPrefab;

    private bool _active = false;
    private bool _canPlace = false;
    private Blueprint _blueprint;
    private Dictionary<Building, Transform> _tunnels = new Dictionary<Building, Transform>();

    public void Activate(GameObject prefab)
    {
        _active = true;

        _blueprint = Instantiate(prefab).GetComponent<Blueprint>();
        _blueprint.GetComponent<MeshRenderer>().material = blueprintMaterial;
    }

    public void Build(InputAction.CallbackContext context)
    {
        if (!_active || !_canPlace)
        {
            return;
        }

        _active = _canPlace = false;

        BuildingRegistry.Instance.Register(_blueprint.Spawn());

        foreach (var tunnel in _tunnels)
        {
            if (tunnel.Value.GetComponent<MeshRenderer>().enabled)
            {
                tunnel.Value.GetComponent<Blueprint>().Spawn();
            }

            Destroy(tunnel.Value.gameObject);
        }

        Destroy(_blueprint.gameObject);

        _blueprint = null;
        _tunnels.Clear();
    }

    private void Update()
    {
        if (!_active)
        {
            return;
        }

        MoveToMouse();
        BuildTunnels();
        
        if (!_tunnels.Any(tunnel => tunnel.Value.GetComponent<MeshRenderer>().enabled))
        {
            _canPlace = false;
        }
        
        _blueprint.GetComponent<MeshRenderer>().material = _canPlace ? blueprintMaterial : errorMaterial;
    }

    private void BuildTunnels()
    {
        foreach (var building in BuildingRegistry.Instance.GetAll())
        {
            if (!_tunnels.ContainsKey(building))
            {
                _tunnels.Add(building, Instantiate(tunnelBlueprintPrefab).transform);
            }

            if (Vector3.Distance(building.transform.position, _blueprint.transform.position) > 25)
            {
                _tunnels[building].GetComponent<MeshRenderer>().enabled = false;
                continue;
            }

            PrepareTunnel(building);
        }
    }

    private void PrepareTunnel(Building building)
    {
        var tunnel = _tunnels[building];

        var buildingTransform = building.transform;
        var buildingPosition = buildingTransform.position;
        var blueprintTransform = _blueprint.transform;
        var blueprintPosition = blueprintTransform.position;

        // Place the tunnel on the middle point between the building and blueprint
        tunnel.position = new Vector3(
            (buildingPosition.x + blueprintPosition.x) / 2,
            .5f,
            (buildingPosition.z + blueprintPosition.z) / 2
        );


        // Rotate the tunnel between the blueprint and building
        tunnel.LookAt(buildingTransform);
        tunnel.localRotation = Quaternion.Euler(0, tunnel.localRotation.eulerAngles.y, 0);

        // Scale it up
        var buildingSize = building.GetComponent<BoxCollider>().bounds.size;
        
        var distance = Vector3.Distance(building.transform.position, _blueprint.transform.position) - buildingSize.magnitude / 2;
        tunnel.localScale = new Vector3(1, 1, distance);
        tunnel.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(distance * 2, 1);

        var tunnelExtents = tunnel.GetComponent<BoxCollider>().bounds.extents;
        var checkBox = Physics.OverlapBox(
            new Vector3(tunnel.position.x, .6f, tunnel.position.z),
            new Vector3(tunnelExtents.x, .5f, tunnelExtents.z),
            Quaternion.identity,
            ignoreBlueprintLayer
        );

        var active = true;
        foreach (var box in checkBox)
        {
            if (box.gameObject == building.gameObject)
            {
                continue;
            }

            active = false;
        }

        // Check for collisions
        tunnel.GetComponent<MeshRenderer>().enabled = active;
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
            _canPlace = !Physics.CheckBox(
                new Vector3(blueprintTransform.transform.position.x, .6f, blueprintTransform.transform.position.z),
                new Vector3(size.x, .5f, size.z)
            );
        }
    }
}