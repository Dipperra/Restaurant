using System.Collections.Generic;
using UnityEngine;

public class Podnos : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _itemsOnPodnos;
    [SerializeField] private float _velocityThreshold = 10f;
    [SerializeField] private BoxCollider _freezeZone;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private bool _isFrezingActive = false;
    [SerializeField] private bool _isPickedUp = false;
    private Rigidbody _selfRb;


    private float SelfVelocityAmplitude => _selfRb.linearVelocity.magnitude;

    private void Start()
    {
        _selfRb = GetComponent<Rigidbody>();
        Vector3 size = _freezeZone.size / 2f;
        foreach (var collider in Physics.OverlapBox(_freezeZone.transform.position + _freezeZone.center, size, Quaternion.identity, _layerMask))
        {
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody itemRb))
                _itemsOnPodnos.Add(itemRb);
        }
    }

    private void Update()
    {
        if (_isPickedUp && _isFrezingActive && SelfVelocityAmplitude > _velocityThreshold)
            UnFreeze();
        if (_isPickedUp && _isFrezingActive == false && SelfVelocityAmplitude < _velocityThreshold)
            Freeze();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody itemRb))
            _itemsOnPodnos.Add(itemRb);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody itemRb) && _itemsOnPodnos.Contains(itemRb))
            _itemsOnPodnos.Remove(itemRb);
    }


    public void PickUp()
    {
        _isPickedUp = true;
        Freeze();
    }

    public void Release()
    {
        _isPickedUp = false;
        UnFreeze();
    } 

    private void Freeze()
    {
        _isFrezingActive = true;
        foreach (var item in _itemsOnPodnos)
            item.isKinematic = true;
    }

    private void UnFreeze()
    {
        _isFrezingActive = false;
        foreach (var item in _itemsOnPodnos)
            item.isKinematic = false;
    }
}
