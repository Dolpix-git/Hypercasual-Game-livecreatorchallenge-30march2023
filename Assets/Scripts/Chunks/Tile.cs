using System;
using UnityEngine;

[Serializable]
public class Tile{
    public GameObject prefab;
    public bool walkable;
}
public enum TileType {
    grass,
    road,
    grassTree,
    carGen,
    carDestroy
}