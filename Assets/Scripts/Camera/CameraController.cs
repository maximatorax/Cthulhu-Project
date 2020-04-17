using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject Player;
    private Vector3 _offset;

    [Range(0.01f, 1.0f)] public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = true;
    public bool RotateAroundPlayer = false;
    public float RotationsSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - Player.transform.position;
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            RotateAroundPlayer = true;
        }

        if (Input.GetButtonUp("Fire3"))
        {
            RotateAroundPlayer = false;
        }

        if (RotateAroundPlayer)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
            _offset = camTurnAngle * _offset;
        }

        Vector3 newPos = Player.transform.position + _offset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer || RotateAroundPlayer)
        {
            transform.LookAt(Player.transform);
        }
    }

}
