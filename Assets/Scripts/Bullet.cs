using UnityEngine;
using System.Collections;

public struct AttackInfo
{
    public int source;
    public int target;
    public int typeFlag;
    public Vector3 heading;
    public Vector2 position;
    public float speed;
    public EntityType type;

    #region net protocal
    public float time;
    #endregion

    public AttackInfo(EntityType ty)
    {
        source = CommonEnum.illegal_id;
        target = CommonEnum.illegal_id;
        typeFlag = 0x0001;
        heading = Vector3.up;
        position = new Vector2();
        speed = 10.0f;
        type = ty;
        time = Time.fixedTime;
    }

    public bool isNull()
    {
        return source == CommonEnum.illegal_id && target == CommonEnum.illegal_id;
    }
}


public class Bullet : MonoBehaviour {

    AttackInfo _attackInfo;

    Vector3 _velocity;

    public static void Create(AttackInfo info)
    {
        GameObject bulletPre = Resources.Load("Prefabs/bullet") as GameObject;

        GameObject bullet = (GameObject)Instantiate(bulletPre,
            new Vector3(info.position.x, 4, info.position.y),
            Quaternion.LookRotation(info.heading));

        bullet.GetComponent<Bullet>().Init(info);
    }

    public void Init(AttackInfo info)
    {
        _attackInfo = info;

        var ent = GetComponent<EntityBase>();
        ent.Type = _attackInfo.type;

        _velocity = _attackInfo.heading.normalized * _attackInfo.speed;
    }

    public virtual void OnArrive()
    {
        var comm = GetComponent<Communicator>();
        var ent = GetComponent<EntityBase>();
        comm.SendMsg(0, ent.Id, _attackInfo.target, MsgType.msg_on_attack, _attackInfo.typeFlag);

        this.DestroySelf();
    }

    private void DestroySelf()
    {
        EntityMgr.Instance().RemoveEntity(GetComponent<EntityBase>());
        Destroy(gameObject, 0.0f);
    }

    public void FlyToAim()
    {
        if(!_attackInfo.isNull())
        {
            var targetEnt = EntityMgr.Instance().GetEnttiyById(_attackInfo.target);
            var toTarget = targetEnt.transform.position - transform.position;

            var dist = toTarget.magnitude;

            if ((dist < _attackInfo.speed * Time.deltaTime + CommonEnum.ship_collider_radius)
                || transform.position.y < 2)
            {
                OnArrive();
                DestroySelf();
            }

            //if (dist < CommonEnum.ship_collider_radius)
            //{
            //    transform.position = Vector3.Lerp(transform.position, targetEnt.transform.position, 0.3f * _attackInfo.speed * Time.deltaTime);
            //    return;
            //}

            //越靠近速度越快，和距离反相关或者成反比,0.4 ~ 4.0
            float tunringWeight = 3.4f - Mathf.Min(dist / CommonEnum.averange_dist, 1) * 3;

            _velocity += toTarget.normalized * tunringWeight;

            //保证在xoz平面速度恒定,方便服务器计算时间
            _velocity = _velocity / (new Vector2(_velocity.x, _velocity.z)).magnitude * _attackInfo.speed;

            transform.position = transform.position + _velocity * Time.deltaTime;

            transform.rotation.SetLookRotation(_velocity);
        }
    }

    private void FixedUpdate()
    {
        FlyToAim();
    }

    private void Awake()
    {
        var ent = GetComponent<EntityBase>();
        ent.Type = EntityType.bullet;
    }
}
