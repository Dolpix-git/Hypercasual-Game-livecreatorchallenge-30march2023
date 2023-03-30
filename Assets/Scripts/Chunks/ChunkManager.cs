using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour{
    #region Singleton Pattern
    private static ChunkManager _instance;
    public static ChunkManager Instance {
        get {
            if (_instance is null) {
                _instance = FindObjectOfType<ChunkManager>();
                if (_instance is null) {
                    var obj = Instantiate(new GameObject("ChunkManager"));
                    _instance = obj.AddComponent<ChunkManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private int chunkLength, chunkMaxDistance, chunkMinDistance;

    [SerializedDictionary("ID", "GameObject")]
    public SerializedDictionary<TileType, Tile> prefabs;
    private Dictionary<int, Chunk> chunks = new Dictionary<int, Chunk>();

    

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this);
        } else {
            _instance = this;
        }
    }

    private void Update() {
        if ((int)GameManager.Instance.Player.transform.position.z >= GameManager.Instance.CurrentYLevel) {
            GameManager.Instance.CurrentYLevel = (int)GameManager.Instance.Player.transform.position.z;
            UpdateChunks();
        }
    }
    public void UpdateChunks() {
        int highestChunk = 0;
        List<int> chunksToDelete = new List<int>();
        foreach (int pos in chunks.Keys) {
            if (pos < GameManager.Instance.CurrentYLevel - chunkMinDistance) chunksToDelete.Add(pos);
            if (pos > highestChunk) highestChunk = pos;
        }

        for (int i = 0; i < chunksToDelete.Count; i++) {
            for (int j = 0; j < chunks[chunksToDelete[i]].visuals.Length; j++) {
                Destroy(chunks[chunksToDelete[i]].visuals[j]);
            }
            chunks.Remove(chunksToDelete[i]);
        }

        for (int i = highestChunk + 1; i <= GameManager.Instance.CurrentYLevel + chunkMaxDistance; i++) {
            GenerateChunks(chunkLength, i);
        }
    }

    void GenerateChunks(int rowLength, int yVal) {
        chunks.Add(yVal, new Chunk(rowLength,yVal));
    }

    public bool CheckPosition(int x, int y) {
        if (!chunks.ContainsKey(y)) return false;

        return chunks[y].CheckChunk(x);
    }

    public void GenerateChunkRow(TileType[] rows, int y, out GameObject[] visuals) {
        visuals = new GameObject[rows.Length];
        for (int x = 0; x < rows.Length; x++) {
            if (!prefabs.ContainsKey(rows[x])) continue;
            visuals[x] = Instantiate(prefabs[rows[x]].prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
        }
    }
}
