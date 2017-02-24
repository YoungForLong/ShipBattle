using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackMgr
{
    #region Singleton
    private static TrackMgr _instance = null;

    public static TrackMgr Instance()
    {
        if (_instance == null)
        {
            _instance = new TrackMgr();
            _instance.Init();
        }
        return _instance;
    }
    #endregion

    Dictionary<string, TrackInfo> _tracks = new Dictionary<string, TrackInfo>();

    public void Init()
    {
        var go = Resources.Load<GameObject>("Prefabs/BulletTrack") as GameObject;
        GameObject realGo = (GameObject)MonoBehaviour.Instantiate(go);

        _tracks.Add("bullet",realGo.GetComponent<TrackInfo>());
    }

    public TrackInfo GetTrackByName(string name)
    {
        TrackInfo ret;
        if (_tracks.TryGetValue(name, out ret))
            return ret;
        else
            return null;
    }
}

