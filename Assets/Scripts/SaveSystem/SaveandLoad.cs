using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveAndLoad : MonoBehaviour {

    private void Start()
    {
        //Save an empty Player
    }
    public void StoreGameState(PlayerStats state)
    {
        BinaryWriter writer = new BinaryWriter(File.OpenWrite("./gamestate.bin"));
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(writer.BaseStream, state);
    }

    public PlayerStats RestoreGameState()
    {
        BinaryWriter reader = new BinaryWriter(File.OpenWrite("./gamestate.bin"));
        BinaryFormatter formatter = new BinaryFormatter();
        return (PlayerStats)formatter.Deserialize(reader.BaseStream);
    }


}