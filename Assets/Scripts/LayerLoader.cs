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

    string jsonPath;

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

    void convertToJson()
    {   

        TilePrefab[] tiles = GetComponentsInChildren<TilePrefab>();
        TileLocations dataTileLocations = new TileLocations();
        dataTileLocations.numOfTiles = tiles.Length;
        List<Vector3> indivTileTransforms = new List<Vector3>();
        List<TilePrefab.TileTypes> indivTilePrefabNames = new List<TilePrefab.TileTypes>();
        foreach (TilePrefab tilePrefab in tiles) {
            if (tilePrefab.tileType == TilePrefab.TileTypes.SINGLE ||
                tilePrefab.tileType == TilePrefab.TileTypes.TWOXONE ||
                tilePrefab.tileType == TilePrefab.TileTypes.TWOXTWO) {
                indivTileTransforms.Add(tilePrefab.transform.localPosition);
                indivTilePrefabNames.Add(tilePrefab.tileType);
            }
        }
        dataTileLocations.positions = indivTileTransforms;
        dataTileLocations.prefabNames = indivTilePrefabNames;
        string dataString = JsonUtility.ToJson(dataTileLocations);
        Debug.Log("convertToJson - ");
        Debug.Log(dataString);
        System.IO.File.WriteAllText(jsonPath, dataString);
    }

    void convertFromJson() {
        string datareconstructed = System.IO.File.ReadAllText(jsonPath);
        TileLocations dataReco = JsonUtility.FromJson<TileLocations>(datareconstructed);
        for(int i = 0; i < dataReco.positions.Count; i++) {
            GameObject touse = singleTile;
            if (dataReco.prefabNames[i] == TilePrefab.TileTypes.TWOXONE) {
                touse = twobyOneTile;
            } else if (dataReco.prefabNames[i] == TilePrefab.TileTypes.TWOXTWO) {
                touse = doubleTile;
            }
            Instantiate(touse, dataReco.positions[i], Quaternion.identity, this.transform);
        }
        Debug.Log("convertFromJson - ");
        Debug.Log(datareconstructed);
    }


}
