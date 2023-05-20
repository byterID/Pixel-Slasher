using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 0.2f;
    public Transform groundCheck;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isClimbing = false;

    public bool isRight;
    public bool isMove;
    public bool isSlide;

    Animator anim;
    Rigidbody2D rb;
    private float _walkSpeed = 3;

    public bool isDash = false;
    public float dashDistance = 2f;
    KeyCode lastKeyCode;
    float doubleTapTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
        if (!isGrounded && !isClimbing)
            anim.SetInteger("State", 3);
        if (isGrounded && !isClimbing)
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
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isRight = true;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            isRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public void Walk()
    {
        float translationX = Input.GetAxis("Horizontal") * _walkSpeed;

        translationX *= Time.deltaTime;

        transform.localPosition += new Vector3(translationX, 0, 0);

        if (isMove && isGrounded)
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
        if (Input.GetKeyDown(KeyCode.D))//dddddddddddddddddddddddd
        {
            if (XSkillCurTime >= XSkillKD)//доделать кд
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
                {
                    XSkillCurTime = 0;
                    StartCoroutine(DashX(0.5f));
                    Debug.Log("sad");
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f;
                }
                lastKeyCode = KeyCode.D;
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
}
