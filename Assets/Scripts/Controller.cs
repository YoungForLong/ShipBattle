using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    int _curKeyState = 0;

    private void OnGUI()
    {
        #region key down
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if(e.isKey)
            {
                var curKey = e.keyCode;

                switch (curKey)
                {
                    case KeyCode.W:
                        _curKeyState = 1;
                        break;
                    case KeyCode.A:
                        _curKeyState = 2;
                        break;
                    case KeyCode.S:
                        _curKeyState = 3;
                        break;
                    case KeyCode.D:
                        _curKeyState = 4;
                        break;
                    case KeyCode.Space:
                        GetComponent<Battle>().NormalAttack();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region key up
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (_curKeyState == 1)
                _curKeyState = 0;
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            if (_curKeyState == 2)
                _curKeyState = 0;
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            if (_curKeyState == 3)
                _curKeyState = 0;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (_curKeyState == 4)
                _curKeyState = 0;
        }
        #endregion

        if (_curKeyState == 0)
            GetComponent<Mover>().Stop();
    }

    private void Update()
    {
        var mover = GetComponent<Mover>();

        switch (_curKeyState)
        {
            case 1:
                mover.MoveTowards(new Vector2(0,1));
                break;
            case 2:
                mover.MoveTowards(new Vector2(-1, 0));
                break;
            case 3:
                mover.MoveTowards(new Vector2(0, -1));
                break;
            case 4:
                mover.MoveTowards(new Vector2(1, 0));
                break;
            default:
                break;
        }
    }
}
