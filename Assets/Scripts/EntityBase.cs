using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityBase: MonoBehaviour {
    protected int _id;
    public int Id {
        get { return _id; }
    }
    public EntityType Type { set; get; }

    public EntityBase(int id) { _id = id; }

    /// <summary>
    /// 类型树
    /// 存放的是父节点
    /// </summary>
    public static Dictionary<EntityType, EntityType> TypeTree = new Dictionary<EntityType, EntityType>()
    {
        { EntityType.bullet,EntityType.stone_bullet},
    };

    public bool IsBelongTo(EntityType ty)
    {
        //test
        return ty == Type;
    }

    private void Awake()
    {
        //test
        Type = EntityType.ship;

        this._id = EntityMgr.Instance().GetNewId();
        EntityMgr.Instance().RegisterEntity(this);
    }

    private void OnDestroy()
    {
        EntityMgr.Instance().RemoveEntity(this);
    }
}
