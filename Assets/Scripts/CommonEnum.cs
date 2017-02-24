using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MsgType
{
    msg_destroy_self,
    msg_reply_bullet_arrived,
    msg_on_attack,
};

public enum EntityType
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
    public const float precision = 0.01f;

    //sys
    public const float FPS = 60.0f;
    public const float ADT = 1.0f / FPS;

    //map
    public const float illegal_aim = -999.0f;

    //id，从1-100为系统保留id
    public const int illegal_id = -9999;
    public const int selfMsgReceiver_id = 1;
    public const int hero_id = 2;

    //delay
    public const long deault_delay = 0;
    public const long next_frame_delay = 1;

    //collider
    public const float ship_collider_radius = 2.0f;

    //dist
    public const float averange_dist = 60.0f;
    public const float max_height = 40.0f;
    public const float min_dist = 4.0f;
}