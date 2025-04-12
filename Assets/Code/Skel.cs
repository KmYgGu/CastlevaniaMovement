using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skel : MonoBehaviour
{
    Transform PlayerPosition;
    public GameObject Bone;
    public static bool GoFront;
    public static bool GoBack;
    public float PlayerRay;
    public bool AttackDelay;

    public int SkelHP;

    RaycastHit2D hit;
    SpriteRenderer SkelSp;

    Transform SkeltransForm;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("Player");
        PlayerPosition = obj.GetComponent<Transform>();
        SkeltransForm = GetComponent<Transform>();
        SkelSp = GetComponent<SpriteRenderer>();
        SkelHP = 10;
        StartCoroutine(BoneThrow());
        AttackDelay = true;
    }

    // Update is called once per frame
    void Update()
    {
        //TransFromCheck();
        if (SkelHP > 0)
        {
            PlayerLook();
            Walk();
            RayCast2();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    void TransFromCheck()
    {
        if(this.gameObject.transform.position.x+20< PlayerPosition.transform.position.x || PlayerPosition.transform.position.x > this.gameObject.transform.position.x - 20)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.gameObject.tag == "Stane")
        {
            SkelHP = SkelHP - 1;
            StartCoroutine(SkelDamage());
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PAttack")
        {
            if (AttackDelay)
            {
                AttackDelay = false;
                Debug.Log(555);
                SkelHP = SkelHP - 3;
                StartCoroutine(SkelDamage());
            }
            
        }
    }
    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PAttack")
        {
            Debug.Log(456);

        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Smoke")
        {
            SkelHP = SkelHP - 1;
            StartCoroutine(SkelDamage());
        }
        if (collision.gameObject.tag == "PinkBall")
        {
            
            SkelHP = SkelHP - 3;
            StartCoroutine(SkelDamage());
        }
    }

    IEnumerator SkelDamage()
    {
        SkelSp.color = new Color(1, 1, 1, 0.6f);
        yield return new WaitForSeconds(0.5f);
        SkelSp.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);
        AttackDelay = true;
    }

    void PlayerLook()
    {
        if (gameObject.transform.position.x > PlayerPosition.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (gameObject.transform.position.x < PlayerPosition.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator BoneThrow()
    {
        yield return new WaitForSeconds(1f);
        Vector3 newPosition = transform.position + new Vector3(-1f * transform.localScale.x, 2f, 0f);
        GameObject projectile = Instantiate(Bone, newPosition, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        rb.AddForce((Vector2.left * transform.localScale.x) * 5f, ForceMode2D.Impulse);
        Destroy(projectile, 2f);
        StartCoroutine(BoneThrow());
    }

    void Walk()
    {
        float scaleX = SkeltransForm.localScale.x;


        if (hit.distance > 3)
        {
            if (GoFront)
            {
                Vector3 newPosition = SkeltransForm.position + new Vector3((scaleX * -1), 0.0f, 0.0f) * 4 * Time.deltaTime;
                transform.position = newPosition;
            }
            
        }
        else
        {
            if (GoBack)
            {
                Vector3 newPosition2 = SkeltransForm.position + new Vector3(scaleX, 0.0f, 0.0f) * 4 * Time.deltaTime;//뒤로감               
                transform.position = newPosition2;
            }
            
        }
    }

    void RayCast2()
    {
        Vector3 newPosition = transform.position + new Vector3(-1*transform.localScale.x, 0f, 0f);
        Vector2 rayStartPoint = newPosition;

        //Vector2 rayStartPoint = transform.position;
        hit = Physics2D.Raycast(rayStartPoint, (-transform.right)*transform.localScale.x, Mathf.Infinity);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            // 레이가 어떤 오브젝트와 교차했을 때의 동작
            //hit.distance = PlayerRay;
        }
    }
}
