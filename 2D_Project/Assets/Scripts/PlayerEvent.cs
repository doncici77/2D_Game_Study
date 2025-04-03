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
        if (collision == null || collision.gameObject == null) return; // 오브젝트가 삭제되었는지 확인

        if (collision.name == "TutorialEvent1")
        {
            ArrowKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent2")
        {
            SpaceBarKeyObj.SetActive(false);
        }
        else if (collision.name == "TutorialEvent3")
        {
            AttackKeyObj.SetActive(false);
        }
    }
}
