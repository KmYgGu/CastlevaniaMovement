using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDamage : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigid2D;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            animator.SetTrigger("DownD");
            rigid2D.velocity = Vector3.zero;
            PlayerMove.PlayerHp = PlayerMove.PlayerHp - 10;
        }
    }
}
