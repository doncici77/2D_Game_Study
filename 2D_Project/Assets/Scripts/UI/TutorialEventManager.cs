using UnityEngine;

public class TutorialEventManager : MonoBehaviour
{
    public GameObject myEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            myEvent.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            myEvent.SetActive(false);
        }
    }
}
