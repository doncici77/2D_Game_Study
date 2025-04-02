using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        movement.HandleMovement();
    }
}
