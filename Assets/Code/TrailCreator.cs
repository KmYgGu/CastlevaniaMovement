using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrailCreator : MonoBehaviour
{
    public GameObject trailPrefab;    // �ܻ��� ���� �������� �巡�׾ص������ �������ּ���
    public float trailInterval = 1f;  // �ܻ� ���� ���� (��)

    private Animator characterAnimator;

    Sprite AnimationSprite;
    //public Transform Player;

    // ���� ��ġ�� �����ϱ� ���� ����
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
        // �ƹ� Ű�� �Է¹����� �ܻ��� ����
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
            // ���� ĳ������ ��ġ�� �ִϸ��̼� �������� ������� �ܻ��� ����
            GameObject trail = Instantiate(trailPrefab, this.transform.position, this.transform.rotation);
            SpriteRenderer trailSpriteRenderer = trail.GetComponent<SpriteRenderer>();

            // ��ġ�� ����
            trail.transform.position = this.transform.position;

            trail.transform.localScale = this.transform.localScale;

            trailSpriteRenderer.sprite = AnimationSprite;

            StartCoroutine(FadeTrail(trailSpriteRenderer, trailInterval));
        }
            
    }

    IEnumerator FadeTrail(SpriteRenderer trailRenderer, float duration)//�ܻ���ֱ�
    {
        float elapsedTime = 0f;
        Color startColor = trailRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // ���İ��� 0���� ����

        while (elapsedTime < duration)
        {
            // ���İ��� ������ ���ҽ�Ű��
            trailRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null; // �� ������ ���
        }

        // �ܻ� ����
        Destroy(trailRenderer.gameObject);
    }

    void HandleAnimationEvent(Sprite sprite)
    {
        // �� �Լ����� ��������Ʈ�� �޾ƿͼ� ���ϴ� ������ ����
        // ��: ��������Ʈ�� ����Ͽ� Ư�� ȿ���� ��
        //Debug.Log("Handling Animation Event for Sprite: " + sprite);
        AnimationSprite = sprite;
    }
}
