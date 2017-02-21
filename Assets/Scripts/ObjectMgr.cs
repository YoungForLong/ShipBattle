using System;
using System.Collections.Generic;

/// <summary>
/// singleton class ObjectManenger
/// </summary>
public class ObjectMgr
{
    #region Singleton
    private static ObjectMgr _instance = null;

    public static ObjectMgr Instance()
    {
        if (_instance == null)
        {
            _instance = new ObjectMgr();
        }
        return _instance;
    }
    #endregion

    //(-∞,0]非法id，(0,100)系统静态id，[100,+∞]动态分配id 
    private int _curId = 100;

    private Dictionary<int, EntityBase> _objDict;

    public void RegisterEntity(EntityBase obj)
    {
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
}

