﻿using UnityEngine;
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
        if (projectileShoot == KeyCode.None)
        {
            projectileShoot = KeyCode.U;
        }
        if (attackOne == KeyCode.None)
        {
            attackOne = KeyCode.I;
        }
        if (attackTwo == KeyCode.None)
        {
            attackTwo = KeyCode.O;
        }
        if (attackThree == KeyCode.None)
        {
            attackThree = KeyCode.P;
        }
        if (leftMove == KeyCode.None)
        {
            leftMove = KeyCode.A;
        }
        if (rightMove == KeyCode.None)
        {
            rightMove = KeyCode.D;
        }
        if (jumpButton == KeyCode.None)
        {
            jumpButton = KeyCode.W;
        }
        if (dashButton == KeyCode.None)
        {
            dashButton = KeyCode.V;
        }
        if (attackFour == KeyCode.None)
        {
            attackFour = KeyCode.J;
        }
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

    /// <summary>
    /// attackRate is changed per attack. Set attack[] to whichever attack needed. Attack names are seperate, just remember which attack is which.
    /// Also remember to change HealthControl to affect how much dmg is done, its in seperate script.
    /// attackRate also needs to be precise or else hitbox will hit multipletimes. Change Times Pressed to make spamming not-working.
    /// </summary>
    void AttackInput()
    {
        //StandMed
        if (Input.GetKeyDown(attackOne))
        {
            if (anim.GetFloat("Movement") < 0.1f) //Only allows this attack when not moving.
            {
                attack[0] = true;
                attackTimer[0] = 0;
                timesPressed[0]++;
                anim.SetBool("Attack0", true);
            }
        }

        if (attack[0])
        {
            attackRate = 0.3f; //attack rate here works for StandMed.
            attackTimer[0] += Time.deltaTime;
            if (attackTimer[0] > attackRate || timesPressed[0] >= 3) //after 3 presses, attack animation cuts, but no one can hit that fast.
            {
                attackTimer[0] = 0;
                attack[0] = false;
                timesPressed[0] = 0;
                anim.SetBool("Attack0", false);
            }
        }
        //StandHeavy
        if (Input.GetKeyDown(attackTwo))
        {
            if (anim.GetFloat("Movement") < 0.1f)
            {
                attack[1] = true;
                attackTimer[1] = 0;
                timesPressed[1]++;
                anim.SetBool("Attack1", true);
            }
        }
        if (attack[1])
        {
            attackRate = 1.2f;
            attackTimer[1] += Time.deltaTime;
            if (attackTimer[1] > attackRate || timesPressed[1] >= 7)
            {
                attackTimer[1] = 0;
                attack[1] = false;
                timesPressed[1] = 0;
                anim.SetBool("Attack1", false);
            }
        }
        //LightKick
        if (Input.GetKeyDown(attackThree))
        {
            if (anim.GetFloat("Movement") < 0.1f)
            {
                attack[2] = true;
                attackTimer[2] = 0;
                timesPressed[2]++;
                anim.SetBool("Attack2", true);
            }
        }
        if (attack[2])
        {
            attackRate = 0.2f;
            attackTimer[2] += Time.deltaTime;
            if (attackTimer[2] > attackRate || timesPressed[2] >= 3)
            {
                attackTimer[2] = 0;
                attack[2] = false;
                timesPressed[2] = 0;
                anim.SetBool("Attack2", false);
            }
        }
        //FireballShoot
        if (Input.GetKeyDown(projectileShoot))
        {
            if (anim.GetFloat("Movement") < 0.1f)
            {
                attack[3] = true;
                attackTimer[3] = 0;
                timesPressed[3]++;
                anim.SetBool("Attack3", true);
            }
        }
        if (attack[3])
        {
            attackRate = 0.45f;
            attackTimer[3] += Time.deltaTime;
            if (attackTimer[3] > attackRate || timesPressed[3] >= 5)
            {
                Instantiate(fireBall, firePoint.position, firePoint.rotation);
                attackTimer[3] = 0;
                attack[3] = false;
                timesPressed[3] = 0;
                anim.SetBool("Attack3", false);
            }
        }
        //JumpingHeavyPunch (To Make it Work in the Air, Now modify JumpingAttacks to the number of the Attack you want,
        // e.g JumpingHeavyPunch is now Float 0.5f, while regular jumping is 0.0f.
        if (Input.GetKeyDown(attackFour))
        {
            if ((anim.GetFloat("Movement") < 0.1f && anim.GetBool("OnGround") == false)
                || anim.GetBool("OnGround") == false && anim.GetFloat("Movement") > 0.1f)
            {
                attack[4] = true;
                attackTimer[4] = 0;
                timesPressed[4]++;
                anim.SetFloat("JumpingAttacks", 0.5f);
            }
        }
        if (attack[4])
        {
            attackRate = 0.3f;
            attackTimer[4] += Time.deltaTime;
            if (attackTimer[4] > attackRate || timesPressed[4] >= 3)
            {
                attackTimer[4] = 0;
                attack[4] = false;
                timesPressed[4] = 0;
                anim.SetFloat("JumpingAttacks", 0.0f);
            }
        }
    }
}
