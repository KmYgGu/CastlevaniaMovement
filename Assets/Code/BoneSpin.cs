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

        // Z�� ������ ȸ�� ���� ���
        float rotation = rotationSpeed * Time.deltaTime;

        // ���� ������Ʈ�� Z�� ������ ȸ��
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
