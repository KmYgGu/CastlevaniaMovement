using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSpin : MonoBehaviour
{
    public float rotationSpeed = 45.0f;
    
    // Update is called once per frame
    void Update()
    {
        Transform objectTransform = GetComponent<Transform>();

        // Z축 주위의 회전 각도 계산
        float rotation = rotationSpeed * Time.deltaTime;

        // 현재 오브젝트를 Z축 주위로 회전
        objectTransform.Rotate(0.0f, 0.0f, rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PAttack")
        {
            //Debug.Log(123);
            Destroy(this.gameObject);
        }
    }
}
