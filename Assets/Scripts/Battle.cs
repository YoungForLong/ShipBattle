using UnityEngine;
using System.Collections;

public struct BattleConfig
{
    public int hp;
    public int damage;
    public int armour;
    public float attackDuration;
    public int attackLimit;
}

public class Battle : MonoBehaviour {

    BattleConfig _originConfig;

    int _hp;
    int _damage;
    int _armour;
    float _attackDuration;
    int _attackLimit;

    float _attackCoolDown;

    int _bonusFlag;

    ObjectType _bulletType;

    #region test
    void SimiOriginInfo()
    {
        _originConfig.armour = 1;
        _originConfig.attackDuration = 1.2f;
        _originConfig.attackLimit = 600;
        _originConfig.damage = 4;
        _originConfig.hp = 40;
    }
    #endregion

    private void Awake()
    {
        SimiOriginInfo();

        _hp = _originConfig.hp;
        _damage = _originConfig.damage;
        _attackLimit = _originConfig.attackLimit;
        _attackDuration = _originConfig.attackDuration;
        _armour = _originConfig.armour;

        _attackCoolDown = 0;
        _bonusFlag = 0x0001;

        _bulletType = ObjectType.stone_bullet;
    }

    private void Update()
    {
        _attackCoolDown -= Time.deltaTime;
        _attackCoolDown = _attackCoolDown > 0 ? _attackCoolDown : 0;
    }

    public bool NormalAttack()
    {
        

        return false;
    }

    public bool NormalAttack(int targetId)
    {
        if(_attackCoolDown <=0)
        {
            AttackInfo info = new AttackInfo(ObjectType.stone_bullet);
            info.heading = transform.rotation;
            info.position = transform.position;
            info.speed = 20;
            info.source = GetComponent<EntityBase>().Id;
            info.target = targetId;
            info.typeFlag = _bonusFlag;
            info.type = _bulletType;

            Bullet.Create(info);
        }

        return false;
    }

    public void OnAttck()
    {
        print("on Attack");
    }

}
