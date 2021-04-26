using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapPrefabLoader : MonoBehaviour
{
    public GameObject toplevelTilemap;
    public int toplevelTilemaplayerHeight = -20;

    public List<GameObject> tilemaps;

    public int layerHeight = -20;

    public float nextY = 0f;
    public Vector3 nextPos;

    public Queue<GameObject> layers = new Queue<GameObject>();

    public int amountToLoadAtStart = 100;

    public int layersPassed = 0;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        reloadLevel();

    }
    public void deleteOldLevel() {
        foreach(GameObject go in layers) {
            Destroy(go);
        }
        layers.Clear();
    }

    public void reloadLevel() {
        nextPos = Vector3.zero;
        nextY = 0f;
        GameObject obj = Instantiate(toplevelTilemap);
        obj.transform.position = nextPos;
        layers.Enqueue(obj);
        updateTransform(toplevelTilemaplayerHeight);
        for (int i = 0; i < amountToLoadAtStart; i++) {
            loadNext();
        }
    }

    public void logLayerPassed() {
        layersPassed++;
        if(layersPassed/amountToLoadAtStart >= .9f) {
            //Make player idle or something
            for (int i = 0; i < amountToLoadAtStart; i++) {
                loadNext();
            }
        }
    }

    public void loadNext()
    {
        coroutine = SpawnGO(nextPos);
        StartCoroutine(coroutine);
        updateTransform(layerHeight);
    }

    IEnumerator SpawnGO(Vector3 pos)
    {
        GameObject obj = Instantiate(pickRandomTilemap());
        obj.transform.position = pos;
        obj.GetComponentInChildren<LayerPassed>().loader = this;
        layers.Enqueue(obj);

        /*if (layers.Count > 102) {
            obj = layers.Dequeue();
            Destroy(obj);
        }*/
        yield return null;
    }

    IEnumerator Reload(Vector3 pos)
    {
        GameObject obj = Instantiate(pickRandomTilemap());
        obj.transform.position = pos;
        obj.GetComponentInChildren<LayerPassed>().loader = this;
        layers.Enqueue(obj);

        /*if (layers.Count > 102) {
            obj = layers.Dequeue();
            Destroy(obj);
        }*/
        yield return null;
    }

    void updateTransform(float layerH) {
        nextY += layerH;
        nextPos.y = nextY;
    }

    public GameObject pickRandomTilemap() {
        return tilemaps[Random.Range(0, tilemaps.Count)];
    }
}
