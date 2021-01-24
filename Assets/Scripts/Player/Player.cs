using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    const int left = 180;
    const int right = 0;
    const int trIdle = 0;
    const int trRun = 1;
    const int trJump = 2;
    const int trAttack = 3;
    const int layGround = 8;

    private bool _isJumping;
    private bool _isAttacking;
    private float _recoveryCount;
    private bool _isDead;

    [Header("Properties")]
    public float Speed;
    public float JumpForce;
    public float AttackingRadius;
    public float RecoveryTime;
    public float Health;

    [Header("Components")]
    public Rigidbody2D Rig;
    public Animator Anim;
    public Transform FirePoint;
    public LayerMask EnemyLayer;
    public Image HealthBar;
    public GameController GameController;

    [Header("Audio settings")]
    public AudioSource AudioSource;
    public AudioClip Sfx;

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(FirePoint.position, AttackingRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            OnJump();
            OnAttack();
        }
    }

    void FixedUpdate()
    {
        if (!_isDead)
            OnMove();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == layGround)
            _isJumping = false;
    }

    IEnumerator OnAttacking()
    {
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
    }

    void OnMove()
    {
        var hdirection = Input.GetAxis("Horizontal");

        Rig.velocity = new Vector2(hdirection * Speed, Rig.velocity.y);

        if (hdirection > 0 && !_isJumping && !_isAttacking)
        {
            transform.eulerAngles = new Vector2(0, right);
            Anim.SetInteger("transition", trRun);
        }
        
        if (hdirection < 0 && !_isJumping && !_isAttacking)
        {
            transform.eulerAngles = new Vector2(0, left);
            Anim.SetInteger("transition", trRun);
        }

        if (hdirection == 0 && !_isJumping && !_isAttacking)
        {
            Anim.SetInteger("transition", trIdle);
        }
    }

    void OnJump()
    {
        if (!_isJumping && Input.GetButtonDown("Jump"))
        {
            Anim.SetInteger("transition", trJump);
            Rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            _isJumping = true;
        }
    }

    void OnAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _isAttacking = true;
            Anim.SetInteger("transition", trAttack);
            AudioSource.PlayOneShot(Sfx);

            Collider2D hit = Physics2D.OverlapCircle(FirePoint.position, AttackingRadius, EnemyLayer);
            if (hit != null)
            {
                hit.GetComponent<FlyingEye>().OnHit();
            }

            StartCoroutine(OnAttacking());
        }
    }

    public void OnHit(float damage)
    {
        _recoveryCount += Time.deltaTime;

        if (!_isDead && _recoveryCount >= RecoveryTime)
        {
            Anim.SetTrigger("hit");
            Health -= damage;

            HealthBar.fillAmount = Health/100;

            if (Health <= 0)
            {
                Anim.SetTrigger("death");
                _isDead = true;
                GameController.ShowGameOver();
            }

            _recoveryCount = 0f;
        }

    }
}
