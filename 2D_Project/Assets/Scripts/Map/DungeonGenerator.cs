using System.Collections;
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
    public GameObject camera;
    private Vector3 mapPos = Vector3.zero;

    public void GenerateDungeon(PortalDir dir)
    {
        if (roomCount > 0)
        {
            int randMapType = Random.Range(0, roomPrefabs.Length);
            if (dir == PortalDir.right)
            {
                mapPos = new Vector3(mapPos.x + 60, mapPos.y, mapPos.z);
                StartCoroutine(CreateMap(randMapType));
            }
            else if (dir == PortalDir.left)
            {
                mapPos = new Vector3(mapPos.x - 60, mapPos.y, mapPos.z);
                StartCoroutine(CreateMap(randMapType));
            }
        }
    }

    IEnumerator CreateMap(int randMapType)
    {
        GameObject newMap = Instantiate(roomPrefabs[randMapType], mapPos, Quaternion.identity);

        // 플레이어 위치 설정
        Transform spawnPos = newMap.transform.Find("SpawnPos");

        // Cinemachine 설정 초기화
        camera.GetComponent<CinemachineBrain>().enabled = false;

        playerPos.position = spawnPos.position;

        // Confiner 설정 초기화
        confiner.BoundingShape2D = null;
        confiner.InvalidateBoundingShapeCache();
        confiner.gameObject.GetComponent<CinemachineCamera>().Target.TrackingTarget = null;

        // 1 프레임 대기
        yield return null;

        // 새로운 위치에 맞게 재설정
        confiner.gameObject.GetComponent<CinemachineCamera>().Target.TrackingTarget = playerPos;
        confiner.BoundingShape2D = newMap.transform.Find("CinemachinePoly").GetComponent<Collider2D>();
        confiner.InvalidateBoundingShapeCache();

        // 카메라 재활성화
        camera.GetComponent<CinemachineBrain>().enabled = true;

        confiner.gameObject.SetActive(false);
        yield return null;
        confiner.gameObject.SetActive(true);
    }
}
