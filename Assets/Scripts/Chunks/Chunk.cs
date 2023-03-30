using UnityEngine;

public class Chunk{
    private TileType[] row;
    public GameObject[] visuals;
    public Chunk(int rowLength, int y) {
        row = new TileType[rowLength];
        GenerateRow();


        ChunkManager.Instance.GenerateChunkRow(row, y, out visuals);
    }
    void GenerateRow() {
        if (Random.Range(0,10) < 3) {
            GenerateRoad();
        } else {
            GenerateGrass();
        }
    }

    void GenerateRoad() {
        for (int i = 0; i < row.Length; i++) {
            row[i] = TileType.road;
        }
    }
    void GenerateGrass() {
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

    public bool CheckChunk(int x) {
        if (x < 0 || x >= row.Length) return false;

        if (ChunkManager.Instance.prefabs[row[x]].walkable) return true;

        return false;
    }
}
