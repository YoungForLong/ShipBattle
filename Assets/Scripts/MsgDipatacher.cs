using System;
using System.Collections.Generic;

public class MsgDispatcher
{
    #region Singleton
    private static MsgDispatcher _instance = null;

    public static MsgDispatcher Instance()
    {
        if (_instance == null)
        {
            _instance = new MsgDispatcher();
        }
        return _instance;
    }
    #endregion

    private SortedList<long,Telegram> _msgQueue;

    protected void Discharge(Communicator receiver, Telegram msg)
    {
        if(receiver.HandleMsg != null)
        {
            receiver.HandleMsg(msg);
        }
    }

    public void AddMsg(long delay,int sender,int receiver,MsgType msgtype,object extrainfo)
    {
        var ent_receiver = ObjectMgr.Instance().GetEnttiyById(receiver);

        Telegram tel = new Telegram((long)(UnityEngine.Time.time * 1000) + delay, sender, receiver, msgtype, extrainfo);

        this._msgQueue.Add(tel.dispatchTime, tel);
    }

    /// <summary>
    /// update func
    /// </summary>
    public void DispatchMsg()
    {
        while( _msgQueue.Count != 0 )
        {
            var current = _msgQueue.GetEnumerator().Current;
            if (current.Value.dispatchTime > (UnityEngine.Time.time * 1000))
                break;

            EntityBase ent = ObjectMgr.Instance().GetEnttiyById(current.Value.receiver);
            Communicator receiver = ent.GetComponent<Communicator>();

            Discharge(receiver, current.Value);

            _msgQueue.RemoveAt(0);
        }
    }
}
