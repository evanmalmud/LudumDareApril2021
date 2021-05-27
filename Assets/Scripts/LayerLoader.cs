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

    bool finishedLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        jsonPath = Application.dataPath + "/JsonLevels/" + JsonFileName + ".json";
        if (toJson) {
            convertToJson();
        } else {
            convertFromJson();
        }
    }

    private void Update()
    {
        if(!toJson && !finishedLoading) {
            int maxVal = currentIndex + tilePerFrame;
            if(maxVal > dataReco.positions.Count) {
                maxVal = dataReco.positions.Count;
                finishedLoading = true;
            }
            Debug.Log("Update " + gameObject.name + "- currentIndex:" + currentIndex + " maxVal:" + maxVal);
            for (int i = currentIndex; i < maxVal; i++) {
                GameObject touse = singleTile;
                if (dataReco.prefabNames[i] == TilePrefab.TileTypes.TWOXONE) {
                    touse = twobyOneTile;
                } else if (dataReco.prefabNames[i] == TilePrefab.TileTypes.TWOXTWO) {
                    touse = doubleTile;
                }
                GameObject obj = Instantiate(touse, this.transform, false);
                obj.transform.localPosition = dataReco.positions[i];
            }
            currentIndex += tilePerFrame;
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
        Debug.Log("convertToJson - ");
        Debug.Log(dataString);
        System.IO.File.WriteAllText(jsonPath, dataString);
    }

    void convertFromJson() {
        string datareconstructed = jsonFile.text;
        dataReco = JsonUtility.FromJson<TileLocations>(datareconstructed);
        Debug.Log("convertFromJson - ");

    }


}
