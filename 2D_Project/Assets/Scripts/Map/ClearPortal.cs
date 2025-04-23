using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private bool canUsePortal = false;

    private void Update()
    {
        if(canUsePortal && Input.GetKeyDown(KeyCode.W))
        {
            // Æ÷Å» ·ÎÁ÷ 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canUsePortal = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canUsePortal = false;
    }
}
