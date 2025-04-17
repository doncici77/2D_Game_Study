using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] walls;
    public int mapCount = 3;

    private int currentKillCount;

    private void OnEnable()
    {
        currentKillCount = PlayerStats.Instance.killcount;
        Debug.Log("currentKillCount : " + currentKillCount);
    }

    private void Update()
    {
        if(PlayerStats.Instance.killcount - currentKillCount == mapCount)
        {
            foreach(GameObject wall in walls)
            {
                wall.SetActive(false);
            }
        }
    }
}
