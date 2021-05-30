using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapPrefabLoader : MonoBehaviour
{
    public GameObject toplevelTilemap;
    public int toplevelTilemaplayerHeight = -6;

    public List<GameObject> tilemaps;

    public int layerHeight = -6;

    public float nextY = 0f;
    public Vector3 nextPos;

    public GameObject startLayer;
    public List<GameObject> pooledLayers = new List<GameObject>();
    public List<GameObject> inUseLayers = new List<GameObject>();

    public int amountToLoadAtStart;

    public int layersUntilDeleteDefault = 3;
    public int layersUntilDelete;
    void Start()
    {
        foreach (GameObject go in tilemaps) {
            GameObject obj = Instantiate(go);
            pooledLayers.Add(obj);
        }
        foreach (GameObject go in tilemaps) {
            GameObject obj = Instantiate(go);
            pooledLayers.Add(obj);
        }
        ResetLevel();
    }

    public void ResetLevel() {
        layersUntilDelete = layersUntilDeleteDefault;
        nextPos = Vector3.zero;
        nextY = 0f;
        if (startLayer == null) {
            startLayer = Instantiate(toplevelTilemap);
        }
        startLayer.SetActive(false);
        startLayer.transform.position = nextPos;
        updateTransform(toplevelTilemaplayerHeight);
        startLayer.SetActive(true);

        //Move inUse to Pooled
        foreach(GameObject go in inUseLayers) {
            go.SetActive(false);
            pooledLayers.Add(go);
        }
        inUseLayers.Clear();

        for (int i = 0; i <= amountToLoadAtStart; i++) {
            GameObject obj = pickRandomPooledTile();
            obj.transform.position = nextPos;
            obj.SetActive(true);
            updateTransform(layerHeight);
        }
    }
   
    public void logLayerPassed() {
        Debug.Log("logLayerPassed");
        if(layersUntilDelete <= 0) {
            if(startLayer.activeSelf == true) {
                startLayer.SetActive(false);
            } else if (inUseLayers.Count >= 1) {
                GameObject obj = inUseLayers[0];
                inUseLayers.Remove(obj);
                obj.SetActive(false);
                pooledLayers.Add(obj);
            }
        } else {
            layersUntilDelete--;
        }
        loadNext();
    }

    public void loadNext()
    {
        GameObject obj = pickRandomPooledTile();
        obj.transform.position = nextPos;
        obj.SetActive(true);
        updateTransform(layerHeight);
        Debug.Log("loadNext - " + obj.name);
    }

    void updateTransform(float layerH) {
        nextY += layerH;
        nextPos.y = nextY;
    }

    public GameObject pickRandomPooledTile() {
        GameObject result = pooledLayers[Random.Range(0, pooledLayers.Count)];
        pooledLayers.Remove(result);
        inUseLayers.Add(result);
        return result;
    }
}
