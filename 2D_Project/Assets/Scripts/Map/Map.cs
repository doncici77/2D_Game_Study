using System.Collections;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] monster;
    public Transform[] spawnPos;
    private int mapMonCount;

    private int currentKillCount;
    private bool isWallOpened = false;

    private void OnEnable()
    {
        mapMonCount = spawnPos.Length;
        Debug.Log("mapMonCount : " + mapMonCount);
        currentKillCount = PlayerStats.Instance.killcount;
        Debug.Log("currentKillCount : " + currentKillCount);

        StartCoroutine(spawnMon());
    }

    private void Update()
    {
        if (!isWallOpened && PlayerStats.Instance.killcount - currentKillCount == mapMonCount)
        {
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
            isWallOpened = true;
        }
    }

    public (int, int) GetMonCountData()
    {
        return (PlayerStats.Instance.killcount - currentKillCount, mapMonCount);
    }

    IEnumerator spawnMon()
    {
        // 챕터 씬에서 테스트할때 꺼놓아야 아래 코루틴이 작동함
        /*Debug.Log("몬스터 생성 파티클 시작");
        foreach (Transform spawn in spawnPos)
        {
            ParticleManager.Instance.ParticlePlay(ParticleType.MonsterSpwan, spawn.position, new Vector3(4, 4, 4));
        }*/

        Debug.Log("1초 대기 시작");
        yield return new WaitForSeconds(1);

        Debug.Log("몬스터 생성 시작");
        int i = 0;
        foreach (Transform spawn in spawnPos)
        {
            Instantiate(monster[i % monster.Length], spawn.position, Quaternion.identity);
            i++;
        }
    }
}
