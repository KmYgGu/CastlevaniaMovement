using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageMove : MonoBehaviour
{
    public GameObject textUIPrefab;  // UI �������� �Ҵ��մϴ�.
    public Transform targetPosition; // �̵��� ��ǥ ��ġ�� �Ҵ��մϴ�.

    void Update()
    {
        // Ű���� �Է� ����
        if (Input.anyKeyDown)
        {
            // "Player" �±׸� ���� ������Ʈ�� ��ġ���� UI ����
            CreateUITextAtPlayerPosition();
        }
    }

    void CreateUITextAtPlayerPosition()
    {
        // "Player" �±׸� ���� ������Ʈ�� ã���ϴ�.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(playerObject.transform.position);

        if (playerObject != null)
        {
            // UI �������� �����Ͽ� ���� �߰��մϴ�.
            GameObject uiInstance = Instantiate(textUIPrefab, playerObject.transform.position, Quaternion.identity);

            // �θ� ���� (optional)
            uiInstance.transform.SetParent(transform, false);

            // Text ������Ʈ�� �޽����� �����մϴ�.
            Text uiText = uiInstance.GetComponentInChildren<Text>();
            if (uiText != null)
            {
                uiText.text = "Hello, World!";
            }

            // UI�� ��ǥ ��ġ�� �̵���Ű�� ���� Coroutine ����
            //StartCoroutine(MoveUIText(uiInstance.transform, playerObject.transform.position));
            uiInstance.transform.position = playerObject.transform.position;
        }
    }

    IEnumerator MoveUIText(Transform uiTransform, Vector3 targetWorldPosition)
    {
        float elapsedTime = 0f;
        float moveTime = 3f; // �̵��� �ɸ��� �ð�

        Vector3 initialPosition = uiTransform.position;

        // 3�� ���� ��ǥ ��ġ�� �̵�
        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;
            uiTransform.position = Vector3.Lerp(initialPosition, targetWorldPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 1�� �Ŀ� UI ����
        yield return new WaitForSeconds(1f);
        Destroy(uiTransform.gameObject);
    }
}