using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mover : MonoBehaviour
{
    //动态或者静态的物体
    public bool Active;

    public float Speed;
    public float RSpeed;
    public int Target;
    public Vector2 AimPos;

    public delegate void MyFunc();
    public MyFunc OnArrive = null;

    Rigidbody _rigidbody = null;

    //update func
    void moveUpdate()
    {
        var movement = (new Vector3(AimPos.x, 0, AimPos.y) - transform.position).normalized
            * Speed * Time.deltaTime;

        
    }

    bool turningUpdate()
    {
        //近似处理
        if(AimPos.x != CommonEnum.illegal_aim)
        {
            
        }
        return false;
    }

    public void MoveTo(Vector2 aimPos)
    {
        AimPos = aimPos;
    }

    public void Stop()
    {
        AimPos = new Vector2(CommonEnum.illegal_aim, CommonEnum.illegal_aim);
        Target = CommonEnum.illegal_id;
    }

    public void MoveTowards(Vector2 heading)
    {
        MoveTo(transform.position + new Vector3(heading.x * Speed, 0, heading.y));
    }

    //set init datas
    public void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
        Target = CommonEnum.illegal_id;
        AimPos = new Vector2(CommonEnum.illegal_aim, CommonEnum.illegal_aim);
        OnArrive = null;
        RSpeed = 0.0f;

        _rigidbody = GetComponent<Rigidbody>();

        LoadMovingConfig();
    }

    public void FixedUpdate()
    {
        
    }

    protected void LoadMovingConfig()
    {

    }
}
