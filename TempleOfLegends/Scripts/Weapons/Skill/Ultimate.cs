using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{
    private Character owner;
    private Vector3 targetDir;
    private float velocity;
    private Vector3 startPos;
    private Vector3 endPos;
    private WeaponType mainWeapon;
    private float delayTime;
    private bool atkTrigger;

    SphereCollider col;
    ParticleSystem particle;

    private List<Character> targetList = new List<Character>();

    private void OnEnable()
    {
        velocity = 12f;
        delayTime = 0.3f;
        col = GetComponent<SphereCollider>();
        particle = GetComponent<ParticleSystem>();
        particle.Play();
        col.enabled = true;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.F2))
        {
            PoolManager.Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * velocity);

        

        if (atkTrigger)
        {
            if (delayTime > 0)
            {
                delayTime -= Time.deltaTime;
            }
            else
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    TargetAttack(targetList[i]);
                }
                targetList.Clear();
                particle.Stop();
                PoolManager.Destroy(gameObject);
                atkTrigger = false;
            }
        }
        else
        {
            if (transform.position == endPos && particle.isPlaying)
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    TargetAttack(targetList[i]);
                }
                targetList.Clear();
                particle.Stop();
                PoolManager.Destroy(gameObject);
            }
        }
    }

    public void SetUlt(Character _owner, Vector3 _targetDir, float _range, WeaponType _mainWeapon)
    {
        startPos = transform.position;
        owner = _owner;
        targetDir = _targetDir.normalized;
        endPos = startPos + (targetDir * _range);
        mainWeapon = _mainWeapon;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.gameObject.TryGetComponent(out Character target))
    //    {
    //        if (target.CheckEnemy(owner))
    //        {
    //            atkTrigger = true;
    //            col.enabled = false;
    //            rend.enabled = false;
    //            //PoolManager.Destroy(gameObject);
    //        }
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Character target))
        {
            if (target.CheckEnemy(owner))
            {
                if (!targetList.Contains(target))
                    targetList.Add(target);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Character target))
        {
            if (target.CheckEnemy(owner))
            {
                if (transform.position.DistanceXZ(targetList[0].transform.position) < 0.1f)
                {
                    atkTrigger = true;
                    //col.enabled = false;
                    particle.Stop();
                }
            }
        }
    }

    public void TargetAttack(Character target, float multiplier = 0.9f)
    {
        switch (mainWeapon)
        {
            case WeaponType.Calibrum:
                owner.CurrentWeapon.NormalAttack(target, true);
                CalibrumUlt(target, multiplier);
                break;
            case WeaponType.Severum:
                owner.CurrentWeapon.NormalAttack(target, true);
                SeverumUlt(target, multiplier);
                break;
            case WeaponType.Gravitum:
                owner.CurrentWeapon.NormalAttack(target, true);
                GravitumUlt(target, multiplier);
                break;
            case WeaponType.Infernum:
                InfernumUlt(target, 1.4f);
                break;

        }
    }

    public Character[] GetTargetList()
    {
        return targetList.ToArray();
    }

    public void CalibrumUlt(Character target, float multiplier)
    {
        //Debug.Log(1);
        target.TakeDamage(owner, owner.DamagePower(), multiplier);
        //target.SetCalibrumMarkTrigger(true);
    }

    public void SeverumUlt(Character target, float multiplier)
    {
        //Debug.Log(2);
        owner.TakeHeal(150);
        target.TakeDamage(owner, owner.DamagePower(), multiplier);
    }

    public void GravitumUlt(Character target, float multiplier)
    {
        //Debug.Log(3);
        target.TakeDamage(owner, owner.DamagePower(), multiplier);
    }

    public void InfernumUlt(Character target, float multiplier)
    {
        //Debug.Log(4);
        target.TakeDamage(owner, owner.DamagePower(), multiplier);
    }
}
