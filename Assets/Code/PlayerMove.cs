using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMove : MonoBehaviour
{
    public class DoubleKeyPressDetection//�뽬�� ���� �߰� �ڵ�
    {
        public KeyCode Key { get; private set; }

        /// <summary> �� �� ������ ������ ���� </summary>
        public bool SinglePressed { get; private set; }

        /// <summary> �� �� ������ ������ ���� </summary>
        public bool DoublePressed { get; private set; }

        private bool doublePressDetected;
        private float doublePressThreshold;//ù°Ű�Է¿� �ι�°Ű�Է��� ��
        private float lastKeyDownTime;//���������� Ű�� ���� �ð�

        public DoubleKeyPressDetection(KeyCode key, float threshold = 0.3f)
        {
            this.Key = key;
            SinglePressed = false;
            DoublePressed = false;
            doublePressDetected = false;
            doublePressThreshold = threshold;
            lastKeyDownTime = 0f;
        }

        public void ChangeKey(KeyCode key)
        {
            this.Key = key;
        }
        public void ChangeThreshold(float seconds)
        {
            doublePressThreshold = seconds > 0f ? seconds : 0f;
        }

        /// <summary> MonoBehaviour.Update()���� ȣ�� : Ű ���� ������Ʈ </summary>
        public void Update()
        {
            DoubleKeyCheck();
        }

        void DoubleKeyCheck()
        {
            if (Input.GetKeyDown(Key))
            {
                doublePressDetected =
                    (Time.time - lastKeyDownTime < doublePressThreshold);

                lastKeyDownTime = Time.time;
            }

            if (Input.GetKey(Key))
            {
                if (doublePressDetected)
                    DoublePressed = true;
                else
                    SinglePressed = true;
            }
            else
            {
                doublePressDetected = false;
                DoublePressed = false;
                SinglePressed = false;
            }
        }

        /// <summary> MonoBehaviour.Update()���� ȣ�� : Ű �Է¿� ���� ���� </summary>
        public void UpdateAction(Action singlePressAction, Action doublePressAction)
        {
            if (SinglePressed) singlePressAction?.Invoke();
            if (DoublePressed) doublePressAction?.Invoke();
        }

    }

    Rigidbody2D rigid2D;
    Animator animator;
    Vector3 moveVelocity = Vector3.zero;

    private DoubleKeyPressDetection[] keys2;

    public static int LookKey = 1; //�ٶ󺸴� ����
    public float movePower = 5f; // �̵��ӵ�
    public int jumpPower = 10;//������
    float JumpStart;
    float JumpEnd;

    float KeyJump;// Ű ���� �ð� ����
    public bool LoopJump = false;// �� ������ ���� ���� �ȵǰ� ����
    public bool Delay;//������ ���۸���
    public bool JADlay=false;
    public static bool BdDelay=false;

    public static bool isJumping = false;
    public bool ctJump = false;//Ʈ���� ��� �����Ұ�
    bool isRunning = false;
    public static bool BDamagetime = false;//������ �ǰݴ��ϸ� ���� ���������� ���ظ� ��������

    public GameObject DoggeBall;
    public GameObject Stick;
    public GameObject Smoke;
    public float BallForce = 20f;

    public GameObject idleBox;
    public GameObject DuckBox;
    public GameObject SBox;
    bool Sliding = false;

    public static float PlayerHp = 159;
    public static float Energy = 159;
    public Image EnImage;
    public Image Hpimage;

    // Start is called before the first frame update

    private PlayerMove[] keys;
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        idleBox.SetActive(true);
        DuckBox.SetActive(false);
        SBox.SetActive(false);

        keys2 = new[]
       {
        new DoubleKeyPressDetection(KeyCode.RightArrow),
        new DoubleKeyPressDetection(KeyCode.LeftArrow),
    };

        Delay = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHp > 0)
        {
            if (!BdDelay)
            {
                if (!Delay)
                {
                    Move();
                    Attack();
                    SubWeapon();
                }
                Ducking();
                Jump2();
                BackStep();
            }
        }
        
        

        RectTransform rectTransform = EnImage.GetComponent<RectTransform>();
        float currentWidth = rectTransform.sizeDelta.x;
        float newWidth = Energy;
        // �ʺ� �� ����
        rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);

        RectTransform rectTransform2 = Hpimage.GetComponent<RectTransform>();
        float currentWidth2 = rectTransform.sizeDelta.x;
        float newWidth2 = PlayerHp;
        rectTransform2.sizeDelta = new Vector2(newWidth2, rectTransform.sizeDelta.y);
    }
    void SubWeapon()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(Energy > 5)
            {
                if (ctJump)
                {
                    Energy -= 7;
                    animator.SetTrigger("Smoke");
                    StartCoroutine(Gas());
                }
                else if (isJumping)
                {
                    if (Energy > 10)
                    {
                        Energy -= 10;
                        animator.SetTrigger("JumpBall");
                        StartCoroutine(BallThrow());
                    }
                        
                }
                else if (isRunning)
                {
                    Energy -= 5;
                    animator.SetTrigger("Throw");
                    StartCoroutine(Throw());
                }
                else
                {
                    if (Energy > 10)
                    {
                        Energy -= 10;
                        animator.SetTrigger("Kick");
                        StartCoroutine(BallKick());
                    }
                    
                }
            }
            
            
        }
    }
    IEnumerator Gas()
    {
        yield return new WaitForSeconds(0.38f);
        Vector3 newPosition = transform.position + new Vector3(1f * LookKey * -1, 0f, 0f);
        GameObject projectile = Instantiate(Smoke, newPosition, Quaternion.identity);

        // RigidBody2D ������Ʈ ��������
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            
            rb.AddForce( (Vector2.left * LookKey) * 5f, ForceMode2D.Impulse);
        }
        yield return null;
    }
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.38f);
        Vector3 newPosition = transform.position + new Vector3(1.5f * LookKey * -1, 0.4f, 0f);
        GameObject projectile = Instantiate(Stick, newPosition, Quaternion.identity);
        Destroy(projectile, 5f);
    }

    IEnumerator BallThrow()
    {
        Vector3 newPosition = transform.position + new Vector3(1f * LookKey * -1, 0f, 0f);
        GameObject projectile = Instantiate(DoggeBall, newPosition, Quaternion.identity);

        // RigidBody2D ������Ʈ ��������
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            
            JumpEnd = gameObject.transform.position.y;
            //Debug.Log(Math.Abs(Math.Abs(JumpStart) - Math.Abs(JumpEnd))*3);
            rb.AddForce(transform.up * (Math.Abs(Math.Abs(JumpStart)-Math.Abs(JumpEnd))*3), ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.38f);
            rb.AddForce((Vector2.down+(Vector2.left * LookKey)) * BallForce, ForceMode2D.Impulse);
        }
        yield return null;
        Destroy(projectile, 5f);
    }

    IEnumerator BallKick()
    {
        Vector3 newPosition = transform.position + new Vector3(1f * LookKey * -1, 1f, 0f);
        GameObject projectile = Instantiate(DoggeBall, newPosition, Quaternion.identity);

        // RigidBody2D ������Ʈ ��������
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            yield return new WaitForSeconds(0.38f);
            rb.AddForce((Vector2.up + (Vector2.left * LookKey)) * BallForce, ForceMode2D.Impulse);
        }
        yield return null;
        Destroy(projectile, 5f);
    }

    void DelaySlove()
    {
        if(Delay == true)
        {
            Delay = false;
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isJumping)
            {
                if (!JADlay)
                {
                    JADlay = true;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("JumpAttack");
                }
                
            }
            else if (!isJumping)
            {
                Delay = true;
                Invoke("DelaySlove", 0.3f);
                animator.SetBool("Walk", false);
                animator.SetTrigger("Attack");
            }
            
        }  
    }

    void Jump()
    {
        if (!isJumping)
        {
            if (!ctJump)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    JumpStart = gameObject.transform.position.y;//���߿� �豸�� ���� �궧 ���
                    animator.SetBool("Jump", true);
                    rigid2D.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
                    isJumping = true;

                }
            }
            
        }
        
    }

    void Jump2()
    {
        if (!isJumping)
        {
            if (!ctJump)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    LoopJump = false;
                    Delay = false;
                    JumpStart = gameObject.transform.position.y;//���߿� �豸�� ���� �궧 ���
                    KeyJump = 0;
                }
                if (Input.GetKey(KeyCode.X))
                {
                   
                    KeyJump = KeyJump + 1 * Time.deltaTime;
                    //Debug.Log((KeyJump*20)+5);
                    if (((KeyJump * 30) + 5) > 10)
                    {
                        if (!LoopJump)
                        {
                            animator.SetBool("Jump", true);
                            rigid2D.AddForce(transform.up * ((KeyJump * 30) + 5), ForceMode2D.Impulse);
                            isJumping = true;
                            KeyJump = 0;
                            LoopJump = true;
                        }                    
                    }
                    
                   
                }
                if (Input.GetKeyUp(KeyCode.X))
                {
                    if (((KeyJump * 30) + 5) <= 10)
                    {
                        animator.SetBool("Jump", true);
                        rigid2D.AddForce(transform.up * ((KeyJump * 30) + 5), ForceMode2D.Impulse);
                        isJumping = true;
                        KeyJump = 0;
                       
                    }
                       
                }
            }

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("Jump", false);
            animator.SetBool("BigDamage", false);
            isJumping = false;
            BDamagetime = false;
            JADlay = false;
            BdDelay = false;
        }
    }

    void Ducking()
    {
        
        if (!isJumping)
        {
            if (!Delay)
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    ctJump = true;
                    animator.SetBool("Down", true);

                    idleBox.SetActive(false);
                    if (!Sliding)
                    {
                        DuckBox.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        Delay = true;
                        Invoke("DelaySlove", 0.5f);
                        Sliding = true;
                        Invoke("Slideoff", 0.5f);

                        animator.SetTrigger("Slide");
                        rigid2D.velocity = Vector3.zero;
                        rigid2D.AddForce(transform.right * -12 * LookKey, ForceMode2D.Impulse);
                        DuckBox.SetActive(false);
                        SBox.SetActive(true);
                    }
                }
            }
            
            if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (!Sliding)
                {
                    idleBox.SetActive(true);
                    DuckBox.SetActive(false);
                    ctJump = false;
                    animator.SetBool("Down", false);
                }
                
                
            }
        }         
    }

    void Slideoff()
    {
        if (Sliding)
        {
            Sliding = false;
            rigid2D.velocity = Vector3.zero;
            idleBox.SetActive(true);
            DuckBox.SetActive(false);
            ctJump = false;
            animator.SetBool("Down", false);
        }
    }
    void SBoxOut()
    {
        SBox.SetActive(false);
    }
    void BackStep()
    {
        if (!isJumping)
        {
            if (!ctJump)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    animator.SetTrigger("Step");
                    rigid2D.velocity = Vector3.zero;
                    rigid2D.AddForce(transform.right * 10 * LookKey, ForceMode2D.Impulse);
                }
            }
            
        }
    }

    void Move()
    {
        moveVelocity = Vector3.zero;
        for (int i = 0; i < keys2.Length; i++)
        {
            keys2[i].Update();
        }

        keys2[0].UpdateAction(() =>
        {
            //rigid2D.velocity = Vector3.zero;�������� �̰ɾ��� ���� ���ϰ� ��
            //Debug.Log("Right Arrow");
            movePower = 5f;
            moveVelocity = Vector3.right;
            LookKey = -1;
            animator.SetBool("Walk", true);
        }, () =>
        {
            //Debug.Log("Double Right Arrow");
            movePower = 10f;
            moveVelocity = Vector3.right;
            LookKey = -1;
            animator.SetBool("Run", true);
            isRunning = true;
        });

        keys2[1].UpdateAction(() =>
        {
            //Debug.Log("Left Arrow");
            movePower = 5f;
            moveVelocity = Vector3.left;
            LookKey = 1;
            animator.SetBool("Walk", true);
        }, () =>
        {
            //Debug.Log("Double Left Arrow");
            movePower = 10f;
            moveVelocity = Vector3.left;
            LookKey = 1;
            animator.SetBool("Run", true);
            isRunning = true;

        });
        if(!ctJump)transform.position += moveVelocity * movePower * Time.deltaTime;
        if (LookKey != 0)
        {
            transform.localScale = new Vector3(LookKey, 1, 1);
        }
        if ((!Input.GetKey(KeyCode.LeftArrow)) && (!Input.GetKey(KeyCode.RightArrow))) 
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            isRunning = false;
        } 

    }
}
