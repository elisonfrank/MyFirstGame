using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    const int left = 180;
    const int right = 0;
    const int trIdle = 0;
    const int trAttack = 1;

    private float _initialSpeed;
    private bool _isDead;
    private Transform _player;

    [Header("Properties")]
    public int Health;
    public float Speed;
    public bool IsRight;
    public float StopDistance;
    public float Damage;

    [Header("Components")]
    public Rigidbody2D Rig;
    public Animator Anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _initialSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, _player.position);
        float playerPosition = transform.position.x - _player.position.x;

        IsRight = playerPosition < 0;

        if (distance <= StopDistance)
        {
            Speed = 0f;

            _player.GetComponent<Player>().OnHit(Damage);
            Anim.SetInteger("transition", trAttack);
        } 
        else
            Speed = _initialSpeed;
    }

    void FixedUpdate() 
    {
        if (!_isDead)
        {
            if (IsRight)
            {
                Rig.velocity = new Vector2(Speed, Rig.velocity.y);

                transform.eulerAngles = new Vector2(0, right);
                Anim.SetInteger("transition", trIdle);
            }
            else
            {
                Rig.velocity = new Vector2(-Speed, Rig.velocity.y);

                transform.eulerAngles = new Vector2(0, left);
                Anim.SetInteger("transition", trIdle);
            }
        } else {
            Speed = 0f;
        }
    }

    public void OnHit()
    {
        Health--;

        if (Health <= 0)
        {
            Speed = 0f;
            Anim.SetTrigger("death");
            Destroy(gameObject, 0.6f);
            _isDead = true;
        } 
        else 
        {
            Anim.SetTrigger("hit");
        }            
    }
}
