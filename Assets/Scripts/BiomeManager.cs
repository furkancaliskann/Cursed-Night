using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    private Terrain terrain;
    public Biomes currentBiome {  get; private set; }

    void Awake()
    {
        terrain = Terrain.activeTerrain;
    }
    void Update()
    {
        CheckCurrentBiome();
    }
    private void CheckCurrentBiome()
    {
        // layer 0 and 3 -> forest
        // layer 1 -> snow
        // layer 2 desert

        TerrainData terrainData = terrain.terrainData;
        int mapX = Mathf.FloorToInt(transform.position.x / terrainData.size.x * terrainData.alphamapWidth);
        int mapY = Mathf.FloorToInt(transform.position.z / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapY, 1, 1);

        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            if (splatmapData[0, 0, i] > 0.5f)
            {
                if (i == 0 || i == 3) currentBiome = Biomes.Forest;
                else if (i == 1) currentBiome = Biomes.Snow;
                else if (i == 2) currentBiome = Biomes.Desert;
                return;
            }
        }
    }
}

public enum Biomes
{
    Forest,
    Desert,
    Snow
}