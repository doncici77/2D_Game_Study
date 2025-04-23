using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChapterUI : MonoBehaviour
{
    public Text mapNumText;
    public Text monsterCoundText;
    public Text mapCountText;
    public DungeonGenerator dungeonGenerator;
    public GameObject ClearText;
    private int currentRoomNum;
    private int monMax;
    private int monCurrent;
    private GameObject currentRoom;
    private bool isTextOn = true;

    private void Start()
    {
        currentRoomNum = dungeonGenerator.roomNumber;
    }

    private void Update()
    {
        mapNumText.text = " 맵 넘버 : " + dungeonGenerator.roomNumber;
        mapCountText.text = "남은 맵 개수 : " + dungeonGenerator.roomCount;

        if (currentRoomNum != dungeonGenerator.roomNumber)
        {
            currentRoomNum = dungeonGenerator.roomNumber;
            currentRoom = dungeonGenerator.GetRoomData(currentRoomNum);
            (monCurrent, monMax) =  currentRoom.GetComponent<Map>().GetMonCountData();
            monsterCoundText.text = $"잡은 몬스터 : {monCurrent} / {monMax}";
        }
        else if(currentRoomNum != 50)
        {
            (monCurrent, monMax) = currentRoom.GetComponent<Map>().GetMonCountData();
            monsterCoundText.text = $"잡은 몬스터 : {monCurrent} / {monMax}";
        }

        if(dungeonGenerator.roomCount == 0 && isTextOn)
        {
            isTextOn = false;
            ClearText.SetActive(true);
        }
    }
}
