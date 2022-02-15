using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerLoader : MonoBehaviour
{

    public struct TileLocations { public int numOfTiles; public List<Vector3> positions; public List<TilePrefab.TileTypes> prefabNames; }

    public bool toJson = false;

    public GameObject singleTile;
    public GameObject doubleTile;
    public GameObject twobyOneTile;

    public string JsonFileName;
    int currentIndex = 0;
    int tilePerFrame = 20;
    TileLocations dataReco;
    string jsonPath;
    public TextAsset jsonFile;

    public int blocksHigh = 8;

    public bool finishedLoading = false;

    public List<GameObject> allTileObjects = new List<GameObject>();

    public List<GameObject> allCreated = new List<GameObject>();

    private void Awake()
    {
        if (jsonFile != null) {
            jsonPath = Application.dataPath + "/JsonLevels/" + JsonFileName + ".json";
            allTileObjects = new List<GameObject>();
            allCreated = new List<GameObject>();
            if (toJson) {
                convertToJson();
            } else {
                convertFromJson();
            }
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if(finishedLoading) {
            foreach (GameObject go in allTileObjects) {
                go.SetActive(true);
            }
        }
    }

    void OnDisable()
    {
        foreach (GameObject go in allTileObjects) {
            go.SetActive(false);
        }
        foreach (GameObject go in allCreated) {
            Destroy(go);
        }
        allCreated.Clear();
    }

    private void Update()
    {
        if(jsonFile != null && !toJson && !finishedLoading) {
            int maxVal = currentIndex + tilePerFrame;
            if(maxVal > dataReco.positions.Count) {
                maxVal = dataReco.positions.Count;
                finishedLoading = true;
            }
            //Debug.Log("Update " + gameObject.name + "- currentIndex:" + currentIndex + " maxVal:" + maxVal);
            for (int i = currentIndex; i < maxVal; i++) {
                GameObject touse = singleTile;
                if (dataReco.prefabNames[i] == TilePrefab.TileTypes.TWOXONE) {
                    touse = twobyOneTile;
                } else if (dataReco.prefabNames[i] == TilePrefab.TileTypes.TWOXTWO) {
                    touse = doubleTile;
                }
                GameObject obj = Instantiate(touse, this.transform, false);
                obj.transform.localPosition = dataReco.positions[i];
                obj.SetActive(true);
                allTileObjects.Add(obj);
            }
            currentIndex += tilePerFrame;
        } else if(jsonFile == null) {
            finishedLoading = true;
        }
    }

    void convertToJson()
    {   

        TilePrefab[] tiles = GetComponentsInChildren<TilePrefab>();
        TileLocations dataTileLocations = new TileLocations();
        List<Vector3> indivTileTransforms = new List<Vector3>();
        List<TilePrefab.TileTypes> indivTilePrefabNames = new List<TilePrefab.TileTypes>();
        int count = 0;
        foreach (TilePrefab tilePrefab in tiles) {
            if (tilePrefab.tileType == TilePrefab.TileTypes.SINGLE ||
                tilePrefab.tileType == TilePrefab.TileTypes.TWOXONE ||
                tilePrefab.tileType == TilePrefab.TileTypes.TWOXTWO) {
                count++;
                indivTileTransforms.Add(tilePrefab.transform.localPosition);
                indivTilePrefabNames.Add(tilePrefab.tileType);
            }
        }
        dataTileLocations.positions = indivTileTransforms;
        dataTileLocations.prefabNames = indivTilePrefabNames;
        dataTileLocations.numOfTiles = count; //Removes bombs and artifacts
        string dataString = JsonUtility.ToJson(dataTileLocations);
        //Debug.Log("convertToJson - ");
        //Debug.Log(dataString);
        System.IO.File.WriteAllText(jsonPath, dataString);
    }

    void convertFromJson() {
        string datareconstructed = jsonFile.text;
        dataReco = JsonUtility.FromJson<TileLocations>(datareconstructed);
        //Debug.Log("convertFromJson - ");

    }


}
