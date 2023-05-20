using Cainos.LucidEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    [SerializeField] private float maxHp = 100;
    private float curHp;

    [SerializeField] private float jumpHeight = 0.2f;
    public Transform groundCheck;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isClimbing = false;

    public bool isRight;
    public bool isMove;
    public bool isSlide;
    public bool isAttack;

    Animator anim;
    Rigidbody2D rb;
    private float _walkSpeed = 3;

    public bool isDash = false;
    private float dashDistance = 10f;
    KeyCode lastKeyCode;
    float doubleTapTime;

    private float XSkillKD = 2;
    private float XSkillCurTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        curHp = maxHp;
    }

    void Update()
    {
        Attack();
        CheckGround();
        Flip();
        CheckMove();
        Walk();
        Jump();
        Dash();
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing && !isDash && !isAttack)
            anim.SetInteger("State", 3);
        if (isGrounded && !isClimbing && !isDash && !isAttack)
            anim.SetInteger("State", 0);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetInteger("State", 2);
            rb.velocity = Vector2.up * jumpHeight;
        }
    }
    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0 && !isAttack)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isRight = true;
        }
        if (Input.GetAxis("Horizontal") < 0 && !isAttack)
        {
            isRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public void Walk()
    {
        float translationX = Input.GetAxis("Horizontal") * _walkSpeed;

        translationX *= Time.deltaTime;
        if (isMove && !isDash && !isAttack)
            transform.localPosition += new Vector3(translationX, 0, 0);

        if (isMove && isGrounded && !isDash && !isAttack)
        {
            anim.SetInteger("State", 1);
        }
    }
    public void CheckMove()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }
    public void Dash()
    {
        XSkillCurTime += Time.deltaTime + 0.0001f;
        m_Dash(KeyCode.D,0.7f);
        m_Dash(KeyCode.A,-0.7f);
    }
    private void m_Dash(KeyCode keyCode,float direct)
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (XSkillCurTime >= XSkillKD)//доделать кд
            {
                if (doubleTapTime > Time.time && lastKeyCode == keyCode)
                {
                    XSkillCurTime = 0;
                    StartCoroutine(DashX(direct));
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f;
                }
                lastKeyCode = keyCode;
            }
        }
    }
    
    IEnumerator DashX(float direction)
    {
        isDash = true;
        anim.SetInteger("State", 4);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        isDash = false;
    }
    public void Attack()
    {
        if(!isAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack_anim(10, 0.93f));
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(Attack_anim(6, 0.74f));
            }
        }
    }
    IEnumerator Attack_anim(int AnimState,float AnimTime)
    {
        isMove = false;
        isAttack = true;
        anim.SetInteger("State", AnimState);
        yield return new WaitForSeconds(AnimTime);
        isAttack = false;
        isMove = true;
    }
    public void RecountHp(float deltaHp)
    {
        if (deltaHp < 0)
        {
            curHp = curHp + deltaHp;
        }
        else if (curHp > maxHp)
        {
            curHp = curHp + deltaHp;
            curHp = maxHp;//если игрок подберет +1жизнь, а у него их уже 3, то хп останется на том же уровне
        }
    }
}
