using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleDamage : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigid2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            if (PlayerMove.isJumping==false)
            {
                animator.SetTrigger("idleDamage");
                PlayerMove.PlayerHp = PlayerMove.PlayerHp - 10;
            }
            else if (PlayerMove.isJumping)
            {
                if (PlayerMove.BDamagetime == false)
                {
                    PlayerMove.BDamagetime = true;
                    animator.SetBool("BigDamage", true);
                    rigid2D.velocity = Vector3.zero;
                    rigid2D.AddForce((Vector2.up + (Vector2.right * PlayerMove.LookKey)) * 5, ForceMode2D.Impulse);
                    PlayerMove.BdDelay = true;
                    PlayerMove.PlayerHp = PlayerMove.PlayerHp - 10;
                }
                
            }
            
        }
    }
}
