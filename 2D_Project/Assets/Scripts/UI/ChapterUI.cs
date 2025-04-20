using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChapterUI : MonoBehaviour
{
    public Text mapNumText;
    public Text monsterCoundText;
    public DungeonGenerator dungeonGenerator;
    private int currentRoomNum;
    private int monMax;
    private int monCurrent;
    private GameObject currentRoom;

    private void Start()
    {
        currentRoomNum = dungeonGenerator.roomNumber;
    }

    private void Update()
    {
        mapNumText.text = " 맵 넘버 : " + dungeonGenerator.roomNumber;

        if(currentRoomNum != dungeonGenerator.roomNumber)
        {
            currentRoomNum = dungeonGenerator.roomNumber;
            currentRoom = dungeonGenerator.GetRoomData(currentRoomNum);
            (monCurrent, monMax) =  currentRoom.GetComponent<Map>().GetMonCountData();
            monsterCoundText.text = $"잡은 몬스터 : {monCurrent} / {monMax}";
        }
        else if(currentRoomNum != 50)
        {
            (monCurrent, monMax) = currentRoom.GetComponent<Map>().GetMonCountData();
            monsterCoundText.text = $"남은 몬스터 : {monCurrent} / {monMax}";
        }
    }
}
