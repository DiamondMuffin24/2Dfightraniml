using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    Animator anim;

    private float attackRate;
    bool[] attack = new bool[5];
    float[] attackTimer = new float[5];
    int[] timesPressed = new int[5];

    public Transform firePoint;
    public GameObject fireBall;

    public float jumpHeight;
    public bool isJumping = false;
    private bool canDash;
    public bool DoubleJump = false; // Toggleable in the Unity Editor
    private bool hasDoubleJumped = false; // Tracks if the player has double-jumped

    public KeyCode projectileShoot;
    public KeyCode attackOne;
    public KeyCode attackTwo;
    public KeyCode attackThree;
    public KeyCode attackFour;
    public KeyCode leftMove;
    public KeyCode rightMove;
    public KeyCode jumpButton;
    public KeyCode dashButton;

    public Rigidbody2D player;
    public Rigidbody2D otherPlayer;

    public float dashDistance;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (projectileShoot == KeyCode.None) { projectileShoot = KeyCode.U; }
        if (attackOne == KeyCode.None) { attackOne = KeyCode.I; }
        if (attackTwo == KeyCode.None) { attackTwo = KeyCode.O; }
        if (attackThree == KeyCode.None) { attackThree = KeyCode.P; }
        if (leftMove == KeyCode.None) { leftMove = KeyCode.A; }
        if (rightMove == KeyCode.None) { rightMove = KeyCode.D; }
        if (jumpButton == KeyCode.None) { jumpButton = KeyCode.W; }
        if (dashButton == KeyCode.None) { dashButton = KeyCode.V; }
        if (attackFour == KeyCode.None) { attackFour = KeyCode.J; }
    }

    void Update()
    {
        Movement();
        AttackInput();
        Jump();
    }

    void Jump()
    {
        if (Input.GetKeyDown(jumpButton))
        {
            if (!isJumping) // First jump
            {
                JumpAction();
                isJumping = true;
            }
            else if (DoubleJump && !hasDoubleJumped) // Double jump
            {
                JumpAction();
                hasDoubleJumped = true;
                anim.SetTrigger("DoubleJump"); // Optional animation trigger for double-jump
            }
        }
    }

    void JumpAction()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
        anim.SetBool("OnGround", false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJumping = false;
            hasDoubleJumped = false; // Reset double jump on landing
            canDash = true;
            anim.SetBool("OnGround", true);
        }
    }

    void Movement()
    {
        if (!isJumping)
        {
            if (Input.GetKey(rightMove))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.eulerAngles = new Vector2(0, 0);
                anim.SetFloat("Movement", 1f);
            }
            else if (Input.GetKey(leftMove))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 180);
                anim.SetFloat("Movement", 1f);
            }
            else
            {
                anim.SetFloat("Movement", 0f);
            }
        }

        if (Input.GetKeyDown(dashButton) && Input.GetKey(rightMove))
        {
            if (isJumping && canDash)
            {
                StartCoroutine(DashRight());
                canDash = false;
            }
            else if (!isJumping)
            {
                StartCoroutine(DashRight());
            }
        }

        if (Input.GetKeyDown(dashButton) && Input.GetKey(leftMove))
        {
            if (isJumping && canDash)
            {
                StartCoroutine(DashLeft());
                canDash = false;
            }
            else if (!isJumping)
            {
                StartCoroutine(DashLeft());
            }
        }
    }

    IEnumerator DashRight()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce((Vector2.right * dashDistance));
        if (isJumping)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            anim.SetFloat("JumpingAttacks", 1f);
        }
        else
        {
            anim.SetBool("Dashing", true);
        }
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Dashing", false);
        anim.SetFloat("JumpingAttacks", 0.0f);
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    IEnumerator DashLeft()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce((Vector2.left * dashDistance));
        if (isJumping)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            anim.SetFloat("JumpingAttacks", 1f);
        }
        else
        {
            anim.SetBool("Dashing", true);
        }
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Dashing", false);
        anim.SetFloat("JumpingAttacks", 0.0f);
        transform.eulerAngles = new Vector3(0, 180, 0);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void AttackInput()
    {
        // Your attack logic remains unchanged
    }
}
