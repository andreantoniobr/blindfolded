using System;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Utility for getting the dominant texture from a splat blend at a given point on a terrain.
    /// </summary>

    public class TerrainTextureDetector : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;

        private TerrainData terrainData => terrain.terrainData;
        private float[,,] CachedTerrainAlphamapData;

        private void Start()
        {
            if(terrain && terrain.terrainData)
            {
                CachedTerrainAlphamapData = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
            }            
        }

        /// <summary>
        /// Gets the index of the most visible texture on the terrain at the specified point in world space.
        /// These texture indexes are assigned in the "paint textures" tab of the terrain inspector.
        /// If the supplied position is outside the bounds of the terrain, this function will return -1.
        /// </summary>
        public int GetDominantTextureIndexAt(Vector3 worldPosition)
        {
            Vector3Int alphamapCoordinates = ConvertToAlphamapCoordinates(worldPosition);

            if (!ContainsIndex(CachedTerrainAlphamapData, alphamapCoordinates.x, dimension: 1))
                return -1;

            if (!ContainsIndex(CachedTerrainAlphamapData, alphamapCoordinates.z, dimension: 0))
                return -1;


            int mostDominantTextureIndex = 0;
            float greatestTextureWeight = float.MinValue;

            int textureCount = CachedTerrainAlphamapData.GetLength(2);
            for (int textureIndex = 0; textureIndex < textureCount; textureIndex++)
            {
                // I am really not sure why the x and z coordinates are out of order here, I think it's just Unity being lame and weird
                float textureWeight = CachedTerrainAlphamapData[alphamapCoordinates.z, alphamapCoordinates.x, textureIndex];

                if (textureWeight > greatestTextureWeight)
                {
                    greatestTextureWeight = textureWeight;
                    mostDominantTextureIndex = textureIndex;
                }
            }

            return mostDominantTextureIndex;


            Vector3Int ConvertToAlphamapCoordinates(Vector3 _worldPosition)
            {
                Vector3 relativePosition = _worldPosition - transform.position;
                // Important note: terrains cannot be rotated, so we don't have to worry about rotation

                return new Vector3Int
                (
                    x: Mathf.RoundToInt((relativePosition.x / terrainData.size.x) * terrainData.alphamapWidth),
                    y: 0,
                    z: Mathf.RoundToInt((relativePosition.z / terrainData.size.z) * terrainData.alphamapHeight)
                );
            }
        }

        private bool ContainsIndex(Array array, int index, int dimension)
        {
            if (index < 0)
                return false;

            if (array == null)
                return false;

            return index < array.GetLength(dimension);
        }

        private Terrain GetClosestCurrentTerrain(Vector3 playerPos)
        {
            //Get all terrain
            Terrain[] terrains = Terrain.activeTerrains;

            //Make sure that terrains length is ok
            if (terrains.Length == 0)
                return null;

            //If just one, return that one terrain
            if (terrains.Length == 1)
                return terrains[0];

            //Get the closest one to the player
            float lowDist = (terrains[0].GetPosition() - playerPos).sqrMagnitude;
            var terrainIndex = 0;

            for (int i = 1; i < terrains.Length; i++)
            {
                Terrain terrain = terrains[i];
                Vector3 terrainPos = terrain.GetPosition();

                //Find the distance and check if it is lower than the last one then store it
                var dist = (terrainPos - playerPos).sqrMagnitude;
                if (dist < lowDist)
                {
                    lowDist = dist;
                    terrainIndex = i;
                }
            }
            return terrains[terrainIndex];
        }
    }
}