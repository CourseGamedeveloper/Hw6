using UnityEngine;

public class MyCaveGenerator
{
    private float randomFillPercent;
    private int gridWidth;
    private int gridHeight;
    private int[,] map;

    public MyCaveGenerator(float randomFillPercent, int gridWidth, int gridHeight)
    {
        this.randomFillPercent = randomFillPercent;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        map = new int[gridWidth, gridHeight];
    }

    public void RandomizeMap()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                map[x, y] = Random.value < randomFillPercent ? 1 : 0;
            }
        }
    }

    public void SmoothMap()
    {
        int[,] newMap = new int[gridWidth, gridHeight];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int neighborWallTiles = GetSurroundingWallCount(x, y);

                if (neighborWallTiles > 4)
                    newMap[x, y] = 1;
                else if (neighborWallTiles < 4)
                    newMap[x, y] = 0;
                else
                    newMap[x, y] = map[x, y];
            }
        }

        map = newMap;
    }

    public int[,] GetMap()
    {
        return map;
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighborY = -1; neighborY <= 1; neighborY++)
        {
            for (int neighborX = -1; neighborX <= 1; neighborX++)
            {
                int neighborPosX = gridX + neighborX;
                int neighborPosY = gridY + neighborY;

                if (neighborPosX >= 0 && neighborPosX < gridWidth && neighborPosY >= 0 && neighborPosY < gridHeight)
                {
                    if (neighborX != 0 || neighborY != 0)
                    {
                        wallCount += map[neighborPosX, neighborPosY];
                    }
                }
                else
                {
                    wallCount++; // Consider out of bounds as walls
                }
            }
        }
        return wallCount;
    }
}
