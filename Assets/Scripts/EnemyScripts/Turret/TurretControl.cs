using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretControl : BaseShip
{
    [Header("Turret Control")]
    public float ActiveRange;
    private Transform _player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //find player
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Aim();
        Shoot();
    }

    private void Aim()
    {
        //set reticle
        Reticle.transform.position = _player.transform.position;

        //rotate turret
        Helm.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.up, Reticle.position - transform.position));
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.SmoothDampAngle(transform.eulerAngles.z, Helm.eulerAngles.z, ref _currentTurningVelocity, 0.0f, _finalMaxTurningSpeed * k_turningSpeedDegreeModifier * Time.deltaTime, Time.deltaTime));
    }

    void Shoot()
    {
        if (Vector3.Distance(Reticle.position, transform.position) <= ActiveRange)
        {
            Fire1Auto();
        }
    }
}
