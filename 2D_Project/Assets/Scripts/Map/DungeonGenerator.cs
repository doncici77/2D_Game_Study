using System.Collections.Generic;
using Unity.AppUI.Core;
using UnityEngine;

public enum PortalDir
{
    left,
    right, 
    top, 
    bottom
}

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // 방 프리팹 모음
    public int roomCount = 10;       // 만들 방 개수

    public void GenerateDungeon(PortalDir dir)
    {
        int randMapType = Random.Range(0, roomPrefabs.Length);
    }
}
