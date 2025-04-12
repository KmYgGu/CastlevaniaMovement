using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrailCreator : MonoBehaviour
{
    public GameObject trailPrefab;    // 잔상을 위한 프리팹을 드래그앤드롭으로 연결해주세요
    public float trailInterval = 1f;  // 잔상 생성 간격 (초)

    private Animator characterAnimator;

    Sprite AnimationSprite;
    //public Transform Player;

    // 이전 위치를 저장하기 위한 변수
    private Vector3 previousPosition;
    public bool Check;

    void Start()
    {
        characterAnimator = this.gameObject.GetComponent<Animator>();
        InvokeRepeating("CreateTrail", 0f, 0.2f);
        previousPosition = this.gameObject.transform.position;
    }

    void Update()
    {
        // 아무 키를 입력받으면 잔상을 생성
        /*if (Input.anyKeyDown)
        {
            CreateTrail();
        }*/
        //CreateTrail();
        
        if (this.gameObject.transform.position != previousPosition)
        {
            Check = true;
            //Debug.Log("Position changed to: " + transform.position);
            previousPosition = transform.position;
        }
        else if (this.gameObject.transform.position == previousPosition)
        {
            Check = false;
        }
    }

    void CreateTrail()
    {
        if (Check)
        {
            // 현재 캐릭터의 위치와 애니메이션 프레임을 기반으로 잔상을 생성
            GameObject trail = Instantiate(trailPrefab, this.transform.position, this.transform.rotation);
            SpriteRenderer trailSpriteRenderer = trail.GetComponent<SpriteRenderer>();

            // 위치값 설정
            trail.transform.position = this.transform.position;

            trail.transform.localScale = this.transform.localScale;

            trailSpriteRenderer.sprite = AnimationSprite;

            StartCoroutine(FadeTrail(trailSpriteRenderer, trailInterval));
        }
            
    }

    IEnumerator FadeTrail(SpriteRenderer trailRenderer, float duration)//잔상없애기
    {
        float elapsedTime = 0f;
        Color startColor = trailRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // 알파값을 0으로 설정

        while (elapsedTime < duration)
        {
            // 알파값을 서서히 감소시키기
            trailRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null; // 한 프레임 대기
        }

        // 잔상 제거
        Destroy(trailRenderer.gameObject);
    }

    void HandleAnimationEvent(Sprite sprite)
    {
        // 이 함수에서 스프라이트를 받아와서 원하는 동작을 수행
        // 예: 스프라이트를 사용하여 특정 효과를 줌
        //Debug.Log("Handling Animation Event for Sprite: " + sprite);
        AnimationSprite = sprite;
    }
}
