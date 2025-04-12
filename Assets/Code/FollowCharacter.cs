using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform characterTransform; // 2D 캐릭터의 Transform

    void Update()
    {
        if (characterTransform != null)
        {
            // 2D 좌표를 월드 좌표로 변환
            Vector3 worldPosition = new Vector3(characterTransform.position.x, characterTransform.position.y, 0f);

            // 월드 좌표를 스크린 좌표로 변환
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // 스크린 좌표를 UI 캔버스 상의 좌표로 변환
            RectTransform canvasRectTransform = GetComponent<RectTransform>();
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, Camera.main, out canvasPosition);

            // UI 캔버스 상의 좌표를 적용
            transform.localPosition = canvasPosition;
        }
    }
}
