using UnityEngine;

public class Chunk{
    private TileType[] row;
    public GameObject visual;
    public GameObject carGen;
    public GameObject carDestroy;

    private int yVal;

    public Chunk(int rowLength, int y) {
        yVal = y;
        row = new TileType[rowLength];
        GenerateRow();

        ChunkManager.Instance.GenerateChunkRow(row, y, out visual);
    }
    void GenerateRow() {
        if (yVal < 10) {
            GenerateGrass();
        }else if (Random.Range(0,10) < 3) {
            GenerateRoad();
        } else {
            GenerateForest();
        }
    }

    void GenerateRoad() {
        for (int i = 0; i < row.Length; i++) {
            row[i] = TileType.road;
        }
        ChunkManager.Instance.GenerateCarRoad(out carGen,out carDestroy, yVal);
    }
    void GenerateForest() {
        for (int i = 0; i < row.Length; i++) {
            if (i == (int)(row.Length * 0.5f)) { 
                row[i] = TileType.grass;
                continue;
            }

            if (Random.Range(0, 10) < 3) {
                row[i] = TileType.grassTree;
            } else {
                row[i] = TileType.grass;
            }
        }
    }
    void GenerateGrass() {
        for (int i = 0; i < row.Length; i++) {
            row[i] = TileType.grass;
        }
    }
    public bool CheckChunk(int x) {
        if (x < 0 || x >= row.Length) return false;

        if (ChunkManager.Instance.prefabs[row[x]].walkable) return true;

        return false;
    }
}
