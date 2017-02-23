using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float Smoothing;
    Vector3 _offset;

	// Use this for initialization
	void Start () {
        target = EntityMgr.Instance().GetEnttiyById(CommonEnum.hero_id).transform;
        _offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetCamPos = target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, Smoothing * Time.deltaTime);
    }
}
