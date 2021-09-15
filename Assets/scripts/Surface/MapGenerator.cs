using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWitdth;
    public int mapHeight;
    public float noiseScale;

    public void generateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWitdth, mapHeight, noiseScale);



    }

}
