using UnityEngine;
using System.Collections;

public struct AttackInfo
{
    public int source;
    public int target;
    public int typeFlag;
    public Quaternion heading;
    public Vector2 position;
    public float speed;
    public ObjectType type;

    public AttackInfo(ObjectType ty)
    {
        source = CommonEnum.illegal_id;
        target = CommonEnum.illegal_id;
        typeFlag = 0x0001;
        heading = Quaternion.identity;
        position = new Vector2();
        speed = 10.0f;
        type = ty;
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
        GameObject bullet = (GameObject)Instantiate(bulletPre, info.position, info.heading);

        bullet.GetComponent<Bullet>().Init(info);
    }

    public void Init(AttackInfo info)
    {
        _attackInfo = info;

        var ent = GetComponent<EntityBase>();
        ent.Type = _attackInfo.type;

        _velocity = _attackInfo.heading.eulerAngles;
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
        Destroy(gameObject, 0.2f);
    }

    public void FlyToAim()
    {
        if(!_attackInfo.isNull())
        {
            var targetEnt = ObjectMgr.Instance().GetEnttiyById(_attackInfo.target);
            var toTarget = targetEnt.transform.position - transform.position;

            _velocity += toTarget.normalized * _attackInfo.speed;

            transform.position = transform.position + _velocity * Time.deltaTime;

            transform.rotation.SetLookRotation(_velocity);
        }

        print("info is not applied");
    }

    private void FixedUpdate()
    {
        FlyToAim();
    }
}
