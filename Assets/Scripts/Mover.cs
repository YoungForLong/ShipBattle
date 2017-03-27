using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mover : MonoBehaviour
{
    //动态或者静态的物体
    public bool Active;

    public float Speed;
    public int Target;
    public Vector2 AimPos;

    public delegate void MyFunc();
    public MyFunc OnArrive = null;

    Rigidbody _rigidbody = null;
    private Vector2 _heading = new Vector2(0, 0);

    //update func
    void moveUpdate()
    {
        var movement = new Vector3(_heading.x, 0, _heading.y);

        movement = movement.normalized * Speed * Time.deltaTime;

        _rigidbody.MovePosition(transform.position + movement);
    }

    void turningUpdate()
    {
        var toTarget = new Vector3(_heading.x, 0, _heading.y);
        Quaternion toTarget_q = Quaternion.LookRotation(toTarget);
        _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, toTarget_q, 0.1f));
    }

    public void MoveTo(Vector2 aimPos)
    {
        AimPos = aimPos;
    }

    public void Stop()
    {
        AimPos.Set(CommonEnum.illegal_aim, CommonEnum.illegal_aim);
        Target = CommonEnum.illegal_id;
        _heading.Set(0, 0);
    }

    public void MoveTowards(Vector2 heading)
    {
        _heading = heading;
    }

    //set init datas
    public void Awake()
    {
        transform.position = new Vector3(20, 1, 20);
        //transform.rotation = Quaternion.LookRotation(new Vector3(270, 0, 0));
        Target = CommonEnum.illegal_id;
        AimPos = new Vector2(CommonEnum.illegal_aim, CommonEnum.illegal_aim);
        OnArrive = null;

        _rigidbody = GetComponent<Rigidbody>();

        LoadMovingConfig();
    }

    public void FixedUpdate()
    {
        if (AimPos.x != CommonEnum.illegal_aim)
        {
            _heading = AimPos - new Vector2(transform.position.x, transform.position.z);
        }
        else if(Target != CommonEnum.illegal_id)
        {
            Vector3 targetPos = EntityMgr.Instance().GetEnttiyById(Target).transform.position;
            Vector2 target2D = new Vector2(targetPos.x, targetPos.z);
            _heading = target2D - new Vector2(transform.position.x, transform.position.z);
        }

        if (_heading.magnitude < 0.01f)
        {
            Stop();
            if (OnArrive != null)
                OnArrive();
        }

        if (_heading != new Vector2(0, 0))
        {
            turningUpdate();
            moveUpdate();
        }
    }

    protected void LoadMovingConfig()
    {

    }
}
