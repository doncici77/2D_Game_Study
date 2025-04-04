using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public GameObject ArrowKeyObj;
    public GameObject SpaceBarKeyObj;
    public GameObject AttackKeyObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "TutorialEvent1")
        {
            ArrowKeyObj.SetActive(true);
        }
        else if (collision.name == "TutorialEvent2")
        {
            SpaceBarKeyObj.SetActive(true);
        }
        else if (collision.name == "TutorialEvent3")
        {
            AttackKeyObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return; // 삭제된 오브젝트 예외 처리

        if (collision.name == "TutorialEvent1" && ArrowKeyObj != null)
        {
            ArrowKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent2" && SpaceBarKeyObj != null)
        {
            SpaceBarKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent3" && AttackKeyObj != null)
        {
            AttackKeyObj.SetActive(false);
        }
    }
}
