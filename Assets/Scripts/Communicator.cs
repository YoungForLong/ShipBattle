using UnityEngine;
using System.Collections;

public struct Telegram
{
    public long dispatchTime;
    public int sender;
    public int receiver;
    public MsgType msg;
    public object extraInfo;

    public static bool operator<(Telegram one,Telegram other)
    {
        return one.dispatchTime < other.dispatchTime;
    }

    public static bool operator >(Telegram one, Telegram other)
    {
        return one.dispatchTime > other.dispatchTime;
    }

    public Telegram(long d, int s, int r, MsgType m, object e)
    {
        dispatchTime = d;
        sender = s;
        receiver = r;
        msg = m;
        extraInfo = e;
    }
}


public class Communicator : MonoBehaviour {

    public delegate bool MyFunc(Telegram msg);

    public MyFunc HandleMsg = null;

    public void SendMsg(long delay, int sender, int receiver, MsgType msgtype, object extra = null)
    {
        MsgDispatcher.Instance().AddMsg(delay, sender, receiver, msgtype, extra);
    }
}
