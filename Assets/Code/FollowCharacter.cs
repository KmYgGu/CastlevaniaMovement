using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform characterTransform; // 2D ĳ������ Transform

    void Update()
    {
        if (characterTransform != null)
        {
            // 2D ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = new Vector3(characterTransform.position.x, characterTransform.position.y, 0f);

            // ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // ��ũ�� ��ǥ�� UI ĵ���� ���� ��ǥ�� ��ȯ
            RectTransform canvasRectTransform = GetComponent<RectTransform>();
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, Camera.main, out canvasPosition);

            // UI ĵ���� ���� ��ǥ�� ����
            transform.localPosition = canvasPosition;
        }
    }
}
