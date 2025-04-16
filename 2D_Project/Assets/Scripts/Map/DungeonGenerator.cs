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
    public GameObject[] roomPrefabs; // �� ������ ����
    public int roomCount = 5;       // ���� �� ����
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

        // �÷��̾� ��ġ ����
        Transform spawnPos = newMap.transform.Find("SpawnPos");

        // Cinemachine ���� �ʱ�ȭ
        camera.GetComponent<CinemachineBrain>().enabled = false;

        playerPos.position = spawnPos.position;

        // Confiner ���� �ʱ�ȭ
        confiner.BoundingShape2D = null;
        confiner.InvalidateBoundingShapeCache();
        confiner.gameObject.GetComponent<CinemachineCamera>().Target.TrackingTarget = null;

        // 1 ������ ���
        yield return null;

        // ���ο� ��ġ�� �°� �缳��
        confiner.gameObject.GetComponent<CinemachineCamera>().Target.TrackingTarget = playerPos;
        confiner.BoundingShape2D = newMap.transform.Find("CinemachinePoly").GetComponent<Collider2D>();
        confiner.InvalidateBoundingShapeCache();

        // ī�޶� ��Ȱ��ȭ
        camera.GetComponent<CinemachineBrain>().enabled = true;

        confiner.gameObject.SetActive(false);
        yield return null;
        confiner.gameObject.SetActive(true);
    }
}
