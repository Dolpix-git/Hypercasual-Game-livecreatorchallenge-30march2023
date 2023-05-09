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
            if (chunks[chunksToDelete[i]].visual is not null) Destroy(chunks[chunksToDelete[i]].visual);
            if (chunks[chunksToDelete[i]].carGen is not null) Destroy(chunks[chunksToDelete[i]].carGen);
            if (chunks[chunksToDelete[i]].carDestroy is not null) Destroy(chunks[chunksToDelete[i]].carDestroy);
            chunks.Remove(chunksToDelete[i]);
        }

        for (int i = highestChunk + 1; i <= GameManager.Instance.CurrentYLevel + chunkMaxDistance; i++) {
            GenerateChunks(chunkLength, i);
        }
    }

    public void ClearAllChunks() {
        foreach (Chunk values in chunks.Values) {
            if (values.visual is not null) Destroy(values.visual);
            if (values.carGen is not null) Destroy(values.carGen);
            if (values.carDestroy is not null) Destroy(values.carDestroy);
        }
        chunks = new Dictionary<int, Chunk>();
    }

    void GenerateChunks(int rowLength, int yVal) {
        chunks.Add(yVal, new Chunk(rowLength,yVal));
    }

    public bool CheckPosition(int x, int y) {
        if (!chunks.ContainsKey(y)) return false;

        return chunks[y].CheckChunk(x);
    }

    public void GenerateChunkRow(TileType[] rows, int y, out GameObject visual) {
        visual = Instantiate(new GameObject(), new Vector3(0, 0, y), Quaternion.identity, transform);

        //for (int x = 0; x < rows.Length; x++) {
        //    if (!prefabs.ContainsKey(rows[x])) continue;
        //    visuals[x] = Instantiate(prefabs[rows[x]].prefab, new Vector3(x, 0, y), Quaternion.identity, transform);
        //}



        MeshFilter[] meshFilters = new MeshFilter[rows.Length];
        for (int i = 0; i < rows.Length; i++) {
            meshFilters[i] = prefabs[rows[i]].prefab.GetComponent<MeshFilter>();
        }

        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
        for (int x = 0; x < meshFilters.Length; x++) {
            combineInstances[x].mesh = meshFilters[x].sharedMesh;
            var matrix = visual.transform.localToWorldMatrix;
            matrix[0, 3] = x;
            matrix[1, 3] = 0;
            matrix[2, 3] = 0;
            combineInstances[x].transform = matrix;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances);

        MeshFilter combinedMeshFilter = visual.AddComponent<MeshFilter>();
        combinedMeshFilter.mesh = combinedMesh;

        MeshRenderer combinedMeshRenderer = visual.AddComponent<MeshRenderer>();
        combinedMeshRenderer.material = prefabs[rows[0]].prefab.GetComponent<MeshRenderer>().sharedMaterial;
    }
    public void GenerateCarRoad(out GameObject gen, out GameObject destroy, int y) {
        float direction = Mathf.Sign(Random.Range(-1, 2));
        if (direction > 0) {
            gen = Instantiate(prefabs[TileType.carGen].prefab, new Vector3(0,1,y),Quaternion.identity);
            gen.GetComponent<CarGenerator>().direction = direction;
            destroy = Instantiate(prefabs[TileType.carDestroy].prefab, new Vector3(chunkLength, 1,y),Quaternion.identity);
        } else {
            gen = Instantiate(prefabs[TileType.carGen].prefab, new Vector3(chunkLength, 1, y), Quaternion.identity);
            gen.GetComponent<CarGenerator>().direction = direction;
            destroy = Instantiate(prefabs[TileType.carDestroy].prefab, new Vector3(0, 1, y), Quaternion.identity);
        }
    }
}
