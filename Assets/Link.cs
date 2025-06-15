using TMPro.EditorUtilities;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] private FixedJoint _joint;
    [SerializeField] private Transform _invisiblePoint;
    [SerializeField] private Rigidbody _target;
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Vector3 _velocity;

    [SerializeField] private float _maxDist = 10f;
    [SerializeField] private float _minDist = 0.1f;

    private Vector3 _prevPosition;

    [SerializeField ] private float _currentDist;
    
    public void ChangeDist(float delta)
    {
        _currentDist = Mathf.Clamp(_currentDist + delta, _minDist, _maxDist);
        Vector3 deltaFromCamera = (_invisiblePoint.position - _playerCamera.position ).normalized * _currentDist;
        _invisiblePoint.position = _playerCamera.position + deltaFromCamera;
    }


    public void MakeLink(Rigidbody target)
    {
        _target = target;
        _invisiblePoint.position = target.transform.position;
        target.useGravity = false;
        _joint.connectedBody = target;
        target.freezeRotation = true;
        target.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _currentDist = (_invisiblePoint.position - _playerCamera.position).magnitude;
    }

    public void DestroyLink()
    {
        if (_target == null)
            return;
        Debug.Log(_target.linearVelocity);
        _joint.connectedBody = null;
        _target.useGravity = true;
        _target.freezeRotation = false;
        _target.collisionDetectionMode = CollisionDetectionMode.Discrete;
        _target.linearVelocity = _velocity / Time.fixedDeltaTime;
        _velocity = Vector3.zero;
        _target = null;
    }

    private void FixedUpdate()
    {
        if (_target == null)
            return;
        _velocity = _target.position - _prevPosition;
        _prevPosition = _target.position;

    }
}