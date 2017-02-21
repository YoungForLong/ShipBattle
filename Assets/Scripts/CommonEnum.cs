using UnityEngine;
using System.Collections;

public enum MsgType
{
    msg_destroy_self,
    msg_reply_bullet_arrived,
    msg_on_attack,
};

public enum ObjectType
{
    bullet,
    stone_bullet,
    ship
};

public enum ComponentType
{
    comp_null_type = 0x0000,
    comp_communicator = 0x0001,
    comp_moving = 0x0002,
    comp_displayer = 0x0004,
    comp_battle = 0x0008,
    comp_controller_test = 0x0016,
};

public enum AttackBonus
{
    //普攻
    normal = 0x0001,
    //暴击
    crit = 0x0002,
    //减速
    slow = 0x0004,
    //晕眩
    stun = 0x0008,
    //持续伤害
    dot = 0x0016
    //to do, more types
};

public static class CommonEnum
{
    //math
    public const double PI = 3.1415926;
    public const float max_float = 3.402823466e+38F;
    public const float precision = 0.001f;

    //sys
    public const float FPS = 60.0f;
    public const float ADT = 1.0f / FPS;

    //map
    public const int WHOLE_MAP_WIDTH = 10000;
    public const int WHOLE_MAP_HEIGHT = 10000;
    public const float illegal_aim = -999.0f;

    //weight for force
    public const float BrakingWeight = 0.1f;
    public const float SeekWeight = 1.0f;
    public const float FleeWeight = 1.0f;
    public const float ObsAvoiWeight = 0.05f;
    public const float PursuitWeight = 1.0f;
    public const float EvadeWeight = 1.0f;
    public const int smoothing_frames = 20;
    public const float seek_offset_precision = 1.0f;

    //id，从1-100为系统保留id
    public const int illegal_id = -9999;
    public const int selfMsgReceiver_id = 1;

    //depth,z_order
    public const int bg_zorder = -10;
    //define [0,100) as world depth
    public const int world_below_sea_zorder = 0;
    public const int world_sea_zorder = 10;
    public const int world_above_sea_zorder = 20;
    public const int world_air_zorder = 30;

    //delay
    public const long deault_delay = 0;
    public const long next_frame_delay = 1;
}