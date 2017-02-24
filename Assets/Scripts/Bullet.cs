using UnityEngine;
using System.Collections;

public struct AttackInfo
{
    public int source;
    public int target;
    public int typeFlag;
    public Vector3 position;
    public EntityType type;
    public float time;

    public AttackInfo(EntityType ty)
    {
        source = CommonEnum.illegal_id;
        target = CommonEnum.illegal_id;
        typeFlag = 0x0001;
        position = new Vector3();
        time = 0.0f;
        type = ty;
    }

    public bool isNull()
    {
        return source == CommonEnum.illegal_id && target == CommonEnum.illegal_id;
    }
}


public class Bullet : MonoBehaviour {

    AttackInfo _attackInfo;
    TrackInfo _bulletCurve;
    float _restTime; 

    public static void Create(AttackInfo info)
    {
        GameObject bulletPre = Resources.Load("Prefabs/bullet") as GameObject;

        GameObject bullet = (GameObject)Instantiate(bulletPre,
            info.position,
            Quaternion.identity);

        bullet.GetComponent<Bullet>().Init(info);
    }

    public void Init(AttackInfo info)
    {
        _attackInfo = info;

        var ent = GetComponent<EntityBase>();
        ent.Type = _attackInfo.type;

        _bulletCurve = TrackMgr.Instance().GetTrackByName("bullet");

        _restTime = info.time;
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

    public void BelowSea()
    {
        DestroySelf();
    }

    //[System.Obsolete("using FlyToAimC instead")]
    //public void FlyToAimP()
    //{
    //    if(!_attackInfo.isNull())
    //    {
    //        #region physics sim

    //        var targetEnt = EntityMgr.Instance().GetEnttiyById(_attackInfo.target);
    //        var targetPos = targetEnt.transform.FindChild("AttackPos").position;
    //        var toTarget = targetPos - transform.position;

    //        var dist = toTarget.magnitude;
    //        //print(dist);

    //        if ((dist < _attackInfo.speed * Time.deltaTime + CommonEnum.ship_collider_radius))
    //        {
    //            OnArrive();
    //        }

    //        if (transform.position.y < 0)
    //        {
    //            BelowSea();
    //        }

    //        //越靠近速度越快，和距离反相关或者成反比,此处使用 y = c + x^2
    //        float tunringWeight = 1.0f + Mathf.Pow((1 - Mathf.Min(dist / CommonEnum.averange_dist, 1)) * _attackInfo.speed/10, 2);
    //        _velocity += toTarget.normalized * tunringWeight;

    //        //保证在xoz平面速度恒定,方便服务器计算时间
    //        //_velocity = _velocity / (new Vector2(_velocity.x, _velocity.z)).magnitude * _attackInfo.speed;
    //        _velocity = _velocity.normalized * _attackInfo.speed;
    //        transform.position = transform.position + _velocity * Time.deltaTime;
    //        transform.rotation.SetLookRotation(_velocity);

    //        #endregion

    //        #region math sim
    //        /*
    //        var targetEnt = EntityMgr.Instance().GetEnttiyById(_attackInfo.target);
    //        Vector3 tarPos3d = targetEnt.transform.position;
    //        Vector2 tarPos2d = new Vector2(tarPos3d.x, tarPos3d.z);

    //        Vector2 selfPos2d = new Vector2(transform.position.x, transform.position.z);

    //        var ToTar2d = tarPos2d - selfPos2d;
    //        float dist2d = ToTar2d.magnitude;
    //        if(dist2d < _attackInfo.speed*Time.deltaTime + CommonEnum.ship_collider_radius)
    //        {
    //            OnArrive();
    //        }

    //        var height = 4.0f + Mathf.Sin(Mathf.Min(1.0f, dist2d / _attackInfo.originDist) * Mathf.PI) * _attackInfo.height;

    //        selfPos2d += ToTar2d.normalized * _attackInfo.speed * Time.deltaTime;
    //        transform.position = new Vector3(selfPos2d.x, height, selfPos2d.y);
    //        */
    //        #endregion

    //    }
    //}

    public void FlyToAimC()
    {
        if(!_attackInfo.isNull())
        {
            if(_restTime < 0)
            {
                //TimeOut();
                return;
            }

            var target = EntityMgr.Instance().GetEnttiyById(_attackInfo.target);
            if(!target)
            {
                TargetDie();
            }
            else
            {
                var tPos = target.transform.position;

                Vector3 oldPos = transform.position;
                Vector3 newPos = _bulletCurve.Evaluate(1.0f - (_restTime / _attackInfo.time),
                    _attackInfo.position,
                    tPos);

                transform.position = newPos;
                
                switch(_bulletCurve.NodeDirection)
                {
                    case TrackInfo.DirectionType.Fix:
                        break;
                    case TrackInfo.DirectionType.Tangent:

                        Vector3 deltaPos = newPos - oldPos;

                        if(deltaPos != Vector3.zero)
                        {
                            transform.rotation = Quaternion.LookRotation(deltaPos);
                        }

                        break;
                    case TrackInfo.DirectionType.Target:
                        transform.rotation = Quaternion.LookRotation(tPos - newPos);
                        break;
                    default:
                        print("no direction");
                        break;
                }

            }
            _restTime -= Time.deltaTime;
        }
    }

    public void TargetDie()
    {

    }

    public void TimeOut()
    {
        print("time out");
        OnArrive();
        DestroySelf();
    }

    private void FixedUpdate()
    {
        FlyToAimC();
    }

    private void Awake()
    {
        var ent = GetComponent<EntityBase>();
        ent.Type = EntityType.bullet;
    }
}
