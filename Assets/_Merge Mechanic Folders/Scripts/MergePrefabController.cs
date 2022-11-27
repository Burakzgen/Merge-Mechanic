using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePrefabController : MonoBehaviour
{
    [SerializeField] List<Sprite> listSpriteImages = new List<Sprite>(); // List of images that represent the prefab for each level of merge
    [SerializeField] LayerMask _layer;
    public GameObject parentTile;
    public int prefabLevel = 0;

    private GameObject _currentRaycastObject;
    private Vector3 _currentPrefabPosition;
    private Vector3 _newPrefabPosition;
    private Camera _cam;
    private RaycastHit2D _hit2D;
    private MergeManager mergeManager;

    private void Awake()
    {
        _cam = Camera.main;
        mergeManager = GameObject.FindWithTag("MergeManager").GetComponent<MergeManager>();
    }

    private void OnMouseDown()
    {
        _currentPrefabPosition = transform.position; // get the current position of the cube and set the variable to that value
        gameObject.layer = 0; // Set the cube layer default
    }
    private void OnMouseDrag()
    {
        transform.position = new Vector3(_cam.ScreenToWorldPoint(Input.mousePosition).x, _cam.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
    }
    private void OnMouseUp()
    {
        _hit2D = Physics2D.Raycast(_cam.ScreenToWorldPoint(Input.mousePosition), _cam.transform.forward, 10f, _layer);
        if (_hit2D)
        {
            _newPrefabPosition = _hit2D.collider.transform.position+new Vector3(0, 0.9f,0);
            _currentRaycastObject = _hit2D.collider.gameObject;

            if (_currentRaycastObject.layer == 3) // 3:Tile
            {
                transform.position = _newPrefabPosition;
                gameObject.layer = 6;
                parentTile.layer = 3;
                mergeManager.listOfAvailableTiles.Add(parentTile);
                mergeManager.listOfAvailableTiles.Remove(_currentRaycastObject);
                parentTile = _currentRaycastObject;
                _currentRaycastObject.layer = 0;
            }
            else if (_currentRaycastObject.layer == 6 && _currentRaycastObject.GetComponent<MergePrefabController>().prefabLevel == prefabLevel)
            {
                _currentRaycastObject.GetComponent<MergePrefabController>().prefabLevel++;
                _currentRaycastObject.GetComponent<SpriteRenderer>().sprite = listSpriteImages[_currentRaycastObject.GetComponent<MergePrefabController>().prefabLevel];
                mergeManager.listOfAvailableTiles.Add(parentTile);
                parentTile.layer = 3;
                Destroy(gameObject);
            }
            else
            {
                OnNullResult();
            }
        }
        else
        {
            OnNullResult();
        }
    }

    private void OnNullResult() // If we have no location that is valid to move to
    {
        transform.position = _currentPrefabPosition;
        gameObject.layer = 6; // 6:Cube
    }
}
