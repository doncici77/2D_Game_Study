using System.Collections.Generic;
using Unity.AppUI.Core;
using Unity.Cinemachine;
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
    public int roomCount = 5;       // 만들 방 개수
    public Transform playerPos;
    public CinemachineConfiner2D confiner;
    public Transform camera;
    private Vector3 mapPos = Vector3.zero;

    public void GenerateDungeon(PortalDir dir)
    {
        if (roomCount > 0)
        {
            int randMapType = Random.Range(0, roomPrefabs.Length);
            if (dir == PortalDir.right)
            {
                mapPos = new Vector3(mapPos.x + 60, mapPos.y, mapPos.z);
                CreateMap(randMapType);
            }
            else if (dir == PortalDir.left)
            {
                mapPos = new Vector3(mapPos.x - 60, mapPos.y, mapPos.z);
                CreateMap(randMapType);
            }
        }
    }

    void CreateMap(int randMapType)
    {
        GameObject newMap = Instantiate(roomPrefabs[randMapType], mapPos, Quaternion.identity);
        Transform spawnPos = newMap.transform.Find("SpawnPos");
        playerPos.position = spawnPos.position;
        confiner.BoundingShape2D = newMap.transform.Find("CinemachinePoly").gameObject.GetComponent<BoxCollider2D>();
        Debug.Log("mapPos : " + mapPos);
        camera.position = mapPos;
        confiner.InvalidateBoundingShapeCache();
    }
}
