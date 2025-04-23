using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private bool canUsePortal = false;
    public Transform clearPortal;
    public CinemachineConfiner2D confiner;
    public Transform playerPos;
    public GameObject camera;
    public Collider2D cinePoly;
    public GameObject thankText;

    private void Update()
    {
        if(canUsePortal && Input.GetKeyDown(KeyCode.W))
        {
            // 포탈 로직 
            StartCoroutine(FadingPortal());
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

    IEnumerator FadingPortal()
    {
        yield return StartCoroutine(FadeManager.instance.FadeIn());

        yield return StartCoroutine(TeleportMap());

        yield return StartCoroutine(FadeManager.instance.FadeOut());

        thankText.SetActive(true);
    }

    IEnumerator TeleportMap()
    {
        // Cinemachine 설정 초기화
        camera.GetComponent<CinemachineBrain>().enabled = false;

        playerPos.position = clearPortal.position;

        // Confiner 설정 초기화
        confiner.BoundingShape2D = null;
        confiner.InvalidateBoundingShapeCache();
        confiner.gameObject.GetComponent<CinemachineCamera>().Target.TrackingTarget = null;

        // 1 프레임 대기
        yield return null;

        // 새로운 위치에 맞게 재설정
        confiner.gameObject.GetComponent<CinemachineCamera>().Target.TrackingTarget = playerPos;
        confiner.BoundingShape2D = cinePoly;
        confiner.InvalidateBoundingShapeCache();

        // 카메라 재활성화
        camera.GetComponent<CinemachineBrain>().enabled = true;

        confiner.gameObject.SetActive(false);
        yield return null;
        confiner.gameObject.SetActive(true);
    }
}
