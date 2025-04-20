using System.Collections;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] monster;
    public Transform[] spawnPos;
    private int mapMonCount;

    private int currentKillCount;

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
        if(PlayerStats.Instance.killcount - currentKillCount == mapMonCount)
        {
            foreach(GameObject wall in walls)
            {
                wall.SetActive(false);
            }
        }
    }

    IEnumerator spawnMon()
    {
        foreach (Transform spawn in spawnPos)
        {
            ParticleManager.Instance.ParticlePlay(ParticleType.MonsterSpwan, spawn.position, new Vector3(1, 1, 1));
        }

        yield return new WaitForSeconds(1);

        foreach (Transform spawn in spawnPos)
        {
            int i = 0;
            Instantiate(monster[i % monster.Length], spawn);
            i++;
        }
    }
}
