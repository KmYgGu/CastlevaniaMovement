using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageMove : MonoBehaviour
{
    public GameObject textUIPrefab;  // UI 프리팹을 할당합니다.
    public Transform targetPosition; // 이동할 목표 위치를 할당합니다.

    void Update()
    {
        // 키보드 입력 감지
        if (Input.anyKeyDown)
        {
            // "Player" 태그를 가진 오브젝트의 위치에서 UI 생성
            CreateUITextAtPlayerPosition();
        }
    }

    void CreateUITextAtPlayerPosition()
    {
        // "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(playerObject.transform.position);

        if (playerObject != null)
        {
            // UI 프리팹을 복제하여 씬에 추가합니다.
            GameObject uiInstance = Instantiate(textUIPrefab, playerObject.transform.position, Quaternion.identity);

            // 부모 설정 (optional)
            uiInstance.transform.SetParent(transform, false);

            // Text 컴포넌트에 메시지를 설정합니다.
            Text uiText = uiInstance.GetComponentInChildren<Text>();
            if (uiText != null)
            {
                uiText.text = "Hello, World!";
            }

            // UI를 목표 위치로 이동시키기 위해 Coroutine 시작
            //StartCoroutine(MoveUIText(uiInstance.transform, playerObject.transform.position));
            uiInstance.transform.position = playerObject.transform.position;
        }
    }

    IEnumerator MoveUIText(Transform uiTransform, Vector3 targetWorldPosition)
    {
        float elapsedTime = 0f;
        float moveTime = 3f; // 이동에 걸리는 시간

        Vector3 initialPosition = uiTransform.position;

        // 3초 동안 목표 위치로 이동
        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;
            uiTransform.position = Vector3.Lerp(initialPosition, targetWorldPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 1초 후에 UI 삭제
        yield return new WaitForSeconds(1f);
        Destroy(uiTransform.gameObject);
    }
}