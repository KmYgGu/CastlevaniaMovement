using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        

        
        if (collision.gameObject.tag == "Ground")
        {
            rigid2D.velocity = Vector3.zero;
            animator.SetTrigger("Smoke");
        }
    }
}
