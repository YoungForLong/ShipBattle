using UnityEngine;
using System.Collections;

public struct BattleConfig
{
    public int hp;
    public int damage;
    public int armour;
    public float attackDuration;
    public float attackLimit;
}

public class Battle : MonoBehaviour {

    BattleConfig _originConfig;

    int _hp;
    int _damage;
    int _armour;
    float _attackDuration;
    float _attackLimit;

    float _attackCoolDown;

    int _bonusFlag;

    EntityType _bulletType;

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

        _bulletType = EntityType.stone_bullet;
    }

    private void Update()
    {
        _attackCoolDown -= Time.deltaTime;
        _attackCoolDown = _attackCoolDown > 0 ? _attackCoolDown : 0;
    }

    public bool NormalAttack()
    {
        var nearestEnt = EntityMgr.Instance().getNearestEnt(GetComponent<EntityBase>());

        NormalAttack(nearestEnt.Id);

        return false;
    }

    public bool NormalAttack(int targetId)
    {
        if(_attackCoolDown <= 0)
        {
            AttackInfo info = new AttackInfo(EntityType.stone_bullet);

            var bulletPos = transform.FindChild("BulletPos").position;
            info.position = new Vector2(bulletPos.x, bulletPos.z);

            var targetPos = EntityMgr.Instance().GetEnttiyById(targetId).transform.position;

            var curPos = transform.position;
            var mid = (targetPos + curPos) / 2;
            var toTarget = targetPos - curPos;

            float height = Mathf.Min(
                Mathf.Max(toTarget.magnitude - CommonEnum.min_dist, 0.01f)
                / CommonEnum.averange_dist,
                1) * CommonEnum.max_height;

            mid.Set(mid.x, height, mid.z);
            print(height);

            info.heading = mid - curPos;

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //go.transform.position = info.position;

            info.speed = 40;
            info.source = GetComponent<EntityBase>().Id;
            info.target = targetId;
            info.typeFlag = _bonusFlag;
            info.type = _bulletType;

            Bullet.Create(info);
            _attackCoolDown = _attackDuration;
        }

        return false;
    }

    public void OnAttck()
    {
        print("on Attack");
    }

}
