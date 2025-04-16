using UnityEngine;

public class GunPivotPosSetting : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        transform.position = player.position;
    }
}
