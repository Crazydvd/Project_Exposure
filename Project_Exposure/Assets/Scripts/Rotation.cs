using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Header("How much ° to rotate in each direction\n")]
    [Header("Use 0 and 1s to rotate infinite")]
    [Header("X and Z are local, Y is world")]
    public float maxX = 0;
    public float minX = 0;
    public float maxY = 0;
    public float minY = 0;
    public float maxZ = 0;
    public float minZ = 0;

    [Header("° per frame")]
    public float RotationStep = 1;

    [Header("The direction to rotate in")]
    public bool Positive = true;

    bool _positiveY;
    bool _positiveZ;

    Vector3 _rotation;

    float _rotationX;
    float _rotationY;
    float _rotationZ;

    void Start()
    {
        _positiveY = Positive;
        _positiveZ = Positive;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 euler = transform.rotation.eulerAngles;

        rotateX(euler);
        rotateY(euler);
        rotateZ(euler);

        transform.rotation = Quaternion.Euler(_rotation);
    }

    void rotateX(Vector3 euler)
    {
        if (maxX == 1 && minX == 0)
        {
            _rotation = new Vector3(euler.x + RotationStep, 0, 0);
            return;
        }

        if (maxX == 0 && minX == 1)
        {
            _rotation = new Vector3(euler.x - RotationStep, 0, 0);
            return;
        }

        if (maxX == 0 && minX == 0)
        {
            _rotation = new Vector3(euler.x, 0, 0);
            return;
        }

        if (Positive)
        {
            if (_rotationX > maxX)
            {
                Positive = false;
                _rotation = new Vector3(euler.x + RotationStep, 0, 0);
                _rotationX -= RotationStep;
                return;
            }

            _rotation = new Vector3(euler.x + RotationStep, 0, 0);
            _rotationX += RotationStep;
        }
        else
        {
            if (_rotationX < minX)
            {
                Positive = true;
                _rotation = new Vector3(euler.x + RotationStep, 0, 0);
                _rotationX += RotationStep;
                return;
            }

            _rotation = new Vector3(euler.x - RotationStep, 0, 0);
            _rotationX -= RotationStep;
        }
    }
    
    void rotateY(Vector3 euler)
    {
        if (maxY == 1 && minY == 0)
        {
            _rotation += new Vector3(0, euler.y + RotationStep, 0);
            return;
        }

        if (maxY == 0 && minY == 1)
        {
            _rotation += new Vector3(0, euler.y - RotationStep, 0);
            return;
        }

        if (maxY == 0 && minY == 0)
        {
            _rotation += new Vector3(0, euler.y , 0);
            return;
        }

        if (_positiveY)
        {
            if (_rotationY > maxY)
            {
                _positiveY = false;
                _rotation += new Vector3(0, euler.y - RotationStep, 0);
                _rotationY -= RotationStep;
                return;
            }

            _rotation += new Vector3(0, euler.y + RotationStep, 0);
            _rotationY += RotationStep;
        }
        else
        {
            if (_rotationY < minY)
            {
                _positiveY = true;
                _rotation += new Vector3(0, euler.y + RotationStep, 0);
                _rotationY += RotationStep;
                return;
            }

            _rotation += new Vector3(0, euler.y - RotationStep, 0);
            _rotationY -= RotationStep;
        }
    }

    void rotateZ(Vector3 euler)
    {
        if (maxZ == 1 && minZ == 0)
        {
            _rotation += new Vector3(0, 0, euler.z + RotationStep);
            return;
        }

        if (maxZ == 0 && minZ == 1)
        {
            _rotation += new Vector3(0, 0, euler.z - RotationStep);
            return;
        }

        if (maxZ == 0 && minZ == 0)
        {
            _rotation += new Vector3(0, 0, euler.z);
            return;
        }

        if (_positiveZ)
        {
            if (_rotationZ > maxZ)
            {
                _positiveZ = false;
                _rotation += new Vector3(0, 0, euler.z - RotationStep);
                _rotationZ -= RotationStep;
                return;
            }

            _rotation += new Vector3(0, 0, euler.z + RotationStep);
            _rotationZ += RotationStep;
        }
        else
        {
            if (_rotationZ < minZ)
            {
                _positiveZ = true;
                _rotation += new Vector3(0, 0, euler.z + RotationStep);
                _rotationZ += RotationStep;
                return;
            }

            _rotation += new Vector3(0, 0, euler.z - RotationStep);
            _rotationZ -= RotationStep;
        }
    }
}
