using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    [SerializeField] public Effect BlastEffect;
    [SerializeField] public Effect HitEffect;
    private TrailRenderer _trail;

    private float _speed;
    private float _lifeTimeRemaining;
    private Team _firingTeam;
    private float _attackDamage;
    private float _ionDamage;
    private float _piercingDamage;
    private bool _isExplosive;
    private bool _damagesAll;
    private float _blastRadius;

    private void Awake()
    {
        //init trail
        _trail = GetComponent<TrailRenderer>();
    }

    public void Init(float speed, float persistence, Team firingTeam, float attackDamage, float ionDamage, float piercingDamage, bool isExplosive, bool damagesAll, float blastRadius)
    {
        _speed = speed;
        _lifeTimeRemaining = persistence;
        _firingTeam = firingTeam;
        _attackDamage = attackDamage;
        _ionDamage = ionDamage;
        _piercingDamage = piercingDamage;
        _isExplosive = isExplosive;
        _damagesAll = damagesAll;
        _blastRadius = blastRadius;

        //enable trail
        _trail.emitting = true;
        _trail.Clear();
    }

    private void Update()
    {
        //move
        transform.position += transform.up * _speed * Time.deltaTime;

        //update life time remaining
        _lifeTimeRemaining -= Time.deltaTime;

        //check existence
        if (_lifeTimeRemaining <= 0.0f)
        {
            //despawn
            _trail.emitting = false;
            OnDespawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseShip otherShip = other?.GetComponentInParent<BaseShip>();
        if (otherShip != null && otherShip.ShipTeam != _firingTeam)
        {
            if (_isExplosive)
            {
                //spawn explosion effect
                Effect blastEffect = (Effect)PoolManager.Instance.Spawn(BlastEffect.name, transform.position);
                blastEffect.Init();

                //hit ships within blast radius
                Collider2D[] otherObjects = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), _blastRadius);
                foreach (Collider2D otherObject in otherObjects)
                {
                    BaseShip ship = otherObject?.GetComponentInParent<BaseShip>();
                    if (ship != null && (_damagesAll || ship.ShipTeam != _firingTeam))
                    {
                        //deal explosive damage
                        ship.Damage(_attackDamage, _ionDamage, _piercingDamage);

                        //spawn hit effect
                        Effect hitEffect = (Effect)PoolManager.Instance.Spawn(HitEffect.name, ship.transform.position);
                        hitEffect.Init();
                    }
                }
            }
            else
            {
                //deal projectile damage
                otherShip.Damage(_attackDamage, _ionDamage, _piercingDamage);

                //spawn hit effect
                Effect hitEffect = (Effect)PoolManager.Instance.Spawn(HitEffect.name, transform.position);
                hitEffect.Init();
            }

            //despawn
            _trail.emitting = false;
            OnDespawn();
        }
    }
}
