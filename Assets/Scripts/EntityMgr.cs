using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// singleton class EntityManenger
/// </summary>
public class EntityMgr
{
    #region Singleton
    private static EntityMgr _instance = null;

    public static EntityMgr Instance()
    {
        if (_instance == null)
        {
            _instance = new EntityMgr();
        }
        return _instance;
    }
    #endregion

    //(-∞,0]非法id，(0,100)系统静态id，[100,+∞]动态分配id 
    private int _curId = 100;

    private Dictionary<int, EntityBase> _objDict = new Dictionary<int, EntityBase>();

    public void RegisterEntity(EntityBase obj)
    {
        if(obj == null)
        {
            MonoBehaviour.print("error: null obj");
            return;
        }
        _objDict.Add(obj.Id, obj);  
    }

    public bool RemoveEntity(EntityBase obj)
    {
        return _objDict.Remove(obj.Id);
    }

    public EntityBase GetEnttiyById(int id)
    {
        EntityBase ent;
        _objDict.TryGetValue(id, out ent);

        return ent;
    }

    public int GetNewId() { return _curId++; }

    public EntityBase getNearestEnt(EntityBase center)
    {
        EntityBase ret = null;
        float minDistSq = float.MaxValue;
        Vector2 selfPos2d = new Vector2(center.transform.position.x, center.transform.position.z);

        foreach (KeyValuePair<int,EntityBase> pair in _objDict)
        {
            EntityBase obj = pair.Value;

            if (obj.IsBelongTo(EntityType.ship) && obj != center)
            {
                var pos2d = new Vector2(obj.transform.position.x, obj.transform.position.z);
                var curDistSq = (selfPos2d - pos2d).sqrMagnitude;

                if(curDistSq < minDistSq)
                {
                    minDistSq = curDistSq;
                    ret = obj;
                }
            }
        }

        return ret;
    }

    public List<EntityBase> GetEntitiesInside(EntityBase center,float limit)
    {
        List<EntityBase> ret = new List<EntityBase>();
        Vector2 selfPos2d = new Vector2(center.transform.position.x, center.transform.position.z);

        foreach(KeyValuePair<int,EntityBase> pair in _objDict)
        {
            var obj = pair.Value;

            if (obj.IsBelongTo(EntityType.ship) && obj != center) 
            {
                var pos2d = new Vector2(obj.transform.position.x, obj.transform.position.z);

                if ((pos2d - selfPos2d).sqrMagnitude < limit * limit)
                {
                    ret.Add(obj);
                }
            }
        }

        return ret;
    }
}

