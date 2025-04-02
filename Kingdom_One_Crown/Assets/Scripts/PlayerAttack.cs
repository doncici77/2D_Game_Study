using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void PerformAttack()
    {
        if(playerAnimation != null)
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                playerAnimation.TriggerAttack();
            }
        }
    }
}
