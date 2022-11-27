using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeManager : MonoBehaviour
{
    [SerializeField] float cooldownTime;
    [SerializeField] Image cooldownBarImage;
    [SerializeField] GameObject prefabToSpawn;

    public float _currentCooldownTime;
    private bool _coolDown;
    private GameObject _tileToSpawnOn;

    public List<GameObject> listOfAvailableTiles = new List<GameObject>();

    void Start()
    {
        listOfAvailableTiles.AddRange(GameObject.FindGameObjectsWithTag("Platform"));
    }
    void Update()
    {
        if (_coolDown && listOfAvailableTiles.Count != 0)
        {
            _currentCooldownTime += Time.deltaTime;
            cooldownBarImage.fillAmount = _currentCooldownTime / cooldownTime;

            if (_currentCooldownTime >= cooldownTime)
            {
                // Reset system after cooldown has been completed
                _coolDown = false;
                _currentCooldownTime = 0;
                cooldownBarImage.fillAmount = _currentCooldownTime;
            }
        }
        else
        {
            if (listOfAvailableTiles.Count != 0)
            {
                _tileToSpawnOn = listOfAvailableTiles[Random.Range(0, listOfAvailableTiles.Count - 1)];
                GameObject currentPrefab = Instantiate(prefabToSpawn, new Vector3(_tileToSpawnOn.transform.position.x, _tileToSpawnOn.transform.position.y+0.9f, -0.1f), _tileToSpawnOn.transform.rotation);
                listOfAvailableTiles.Remove(_tileToSpawnOn);
                _tileToSpawnOn.layer = 0;
                _coolDown = true;
                currentPrefab.GetComponent<MergePrefabController>().parentTile = _tileToSpawnOn;
            }

        }
    }
}
