using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapPrefabLoader : MonoBehaviour
{
    public GameObject toplevelTilemap;

    public List<GameObject> tilemaps;

    public float nextY = 0f;
    public Vector3 nextPos;

    public GameObject startLayer;

    public List<LayerLoader> pooledLayerLoader = new List<LayerLoader>();
    public List<LayerLoader> inUseLayers = new List<LayerLoader>();

    public int amountToLoadAtStart;

    public int layersUntilDeleteDefault = 3;
    public int layersUntilDelete;
    void Start()
    {
        foreach (GameObject go in tilemaps) {
            GameObject obj = Instantiate(go);
            pooledLayerLoader.Add(obj.GetComponent<LayerLoader>());
        }
        foreach (GameObject go in tilemaps) {
            GameObject obj = Instantiate(go);
            pooledLayerLoader.Add(obj.GetComponent<LayerLoader>());
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
        updateTransform(startLayer.GetComponent<LayerLoader>().blocksHigh);
        startLayer.SetActive(true);

        //Move inUse to Pooled
        foreach(LayerLoader go in inUseLayers) {
            go.gameObject.SetActive(false);
            pooledLayerLoader.Add(go);
        }
        inUseLayers.Clear();

        for (int i = 0; i <= amountToLoadAtStart; i++) {
            LayerLoader obj = pickRandomPooledTile();
            obj.gameObject.transform.position = nextPos;
            obj.gameObject.SetActive(true);
            updateTransform(obj.blocksHigh);
        }
    }
   
    public void logLayerPassed() {
        Debug.Log("logLayerPassed");
        if(layersUntilDelete <= 0) {
            if(startLayer.activeSelf == true) {
                startLayer.SetActive(false);
            } else if (inUseLayers.Count >= 1) {
                LayerLoader obj = inUseLayers[0];
                inUseLayers.Remove(obj);
                obj.gameObject.SetActive(false);
                pooledLayerLoader.Add(obj);
            }
        } else {
            layersUntilDelete--;
        }
        loadNext();
    }

    public void loadNext()
    {
        LayerLoader obj = pickRandomPooledTile();
        obj.gameObject.transform.position = nextPos;
        obj.gameObject.SetActive(true);
        updateTransform(obj.blocksHigh);
        //Debug.Log("loadNext - " + obj.name);
    }

    void updateTransform(float layerH) {
        nextY -= layerH;
        nextPos.y = nextY;
    }

    public LayerLoader pickRandomPooledTile() {
        LayerLoader result = pooledLayerLoader[Random.Range(0, pooledLayerLoader.Count)];
        pooledLayerLoader.Remove(result);
        inUseLayers.Add(result);
        return result;
    }
}
