using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.Core;
using Unity.Cinemachine;
using Unity.VisualScripting;
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
    public GameObject startMap;
    public int roomCount = 5;       // ���� �� ����
    public Transform playerPos;
    public CinemachineConfiner2D confiner;
    public GameObject camera;
    private Vector3 currentMapPos = Vector3.zero;
    private int[] roomVisit = new int[100];
    private GameObject[] rooms = new GameObject[100];
    private int roomNumber = 50;

    private void Start()
    {
        roomVisit[roomNumber] = 1;
        rooms[roomNumber] = startMap;
    }

    public void GenerateDungeon(PortalDir dir)
    {
        Debug.Log("roomCount : " + roomCount);

        int randMapType = Random.Range(0, roomPrefabs.Length);
        if (dir == PortalDir.right)
        {
            roomNumber++;
            currentMapPos = new Vector3(currentMapPos.x + 60, currentMapPos.y, currentMapPos.z);

            if (roomVisit[roomNumber] == 0)
            {
                if (roomCount > 0)
                {
                    roomVisit[roomNumber] = 1;
                    StartCoroutine(CreateMap(randMapType, "Left"));

                    roomCount--;
                }
                else
                {
                    foreach (Transform child in rooms[roomNumber - 1].transform.GetComponentsInChildren<Transform>())
                    {
                        if (child.CompareTag("RightPortal"))
                        {
                            child.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                            child.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                            playerPos.position = new Vector3(playerPos.position.x - 3, playerPos.position.y);
                            roomNumber--;
                            break;
                        }
                    }
                }
            }
            else
            {
                StartCoroutine(TeleportMap("Left"));
            }
            Debug.Log("RoomNumber : " + roomNumber);
        }
        else if (dir == PortalDir.left)
        {
            roomNumber--;
            currentMapPos = new Vector3(currentMapPos.x - 60, currentMapPos.y, currentMapPos.z);

            if (roomVisit[roomNumber] == 0)
            {
                if (roomCount > 0)
                {
                    roomVisit[roomNumber] = 1;
                    StartCoroutine(CreateMap(randMapType, "Right"));

                    roomCount--;
                }
                else
                {
                    foreach (Transform child in rooms[roomNumber + 1].transform.GetComponentsInChildren<Transform>())
                    {
                        if (child.CompareTag("LeftPortal"))
                        {
                            child.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                            child.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                            playerPos.position = new Vector3(playerPos.position.x + 3, playerPos.position.y);
                            roomNumber++;
                            break;
                        }
                    }
                }
            }
            else
            {
                StartCoroutine(TeleportMap("Right"));
            }
            Debug.Log("RoomNumber : " + roomNumber);
        }

    }

    IEnumerator TeleportMap(string direction)
    {
        // �÷��̾� ��ġ ����
        Transform spawnPos = rooms[roomNumber].transform.Find("SpawnPos" + direction);

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
        confiner.BoundingShape2D = rooms[roomNumber].transform.Find("CinemachinePoly").GetComponent<Collider2D>();
        confiner.InvalidateBoundingShapeCache();

        // ī�޶� ��Ȱ��ȭ
        camera.GetComponent<CinemachineBrain>().enabled = true;

        confiner.gameObject.SetActive(false);
        yield return null;
        confiner.gameObject.SetActive(true);
    }

    IEnumerator CreateMap(int randMapType, string direction)
    {
        GameObject newMap = Instantiate(roomPrefabs[randMapType], currentMapPos, Quaternion.identity);
        rooms[roomNumber] = newMap;

        // �÷��̾� ��ġ ����
        Transform spawnPos = newMap.transform.Find("SpawnPos" + direction);

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
