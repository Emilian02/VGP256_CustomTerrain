using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public Terrain terrain; // Assign this in the Unity Editor 
    public int width = 100; 
    public int height = 100; 
    public float scale = 10f;
    public float strength = 1f;

    void Start()
    {
        //Create the heightmap array
        float[,] heightMap = new float[width, height];

        // Generate the terrain 
        GenerateTerrain(heightMap, scale);

        // Apply the heightmap to the terrain
        ApplyHeightMap(heightMap); 
    }

    void GenerateTerrain(float[,] terrain, float scale)
    {
        // Center of the terrain
        float centerX = width / 2f;
        float centerY = height / 2f;
        float maxDistance = Mathf.Sqrt(centerX * centerX + centerY * centerY);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Perlin Noise to add surface detail
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);

                // Calculate the distance from the center of the terrain
                float distanceX = x - centerX;
                float distanceY = y - centerY;
                float distanceFromCenter = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY) / maxDistance;

                // Create a radial falloff to form a mountain peak
                float falloff = Mathf.Max(0, 1 - Mathf.Pow(distanceFromCenter, 2));

                // The falloff with Perlin noise to create mountain
                terrain[x, y] = perlinValue * falloff * strength;
            }
        }
    }

    void ApplyHeightMap(float[,] heightMap)
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, scale, height);

        // Convert 2D array to 1D array for SetHeights
        float[,] heights = new float[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heights[y, x] = heightMap[x, y] / scale; // Normalize height values 
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
