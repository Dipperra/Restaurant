using UnityEngine;

public class StuffSelector : MonoBehaviour
{
    [SerializeField] private Selectable _selectedObject;
    [SerializeField] private float _rangeChangeSpeed = 1;



    [Header("Raycast")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float _rayLength = 5f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Link _link;
    private bool _draged = false;

    private void Update()
    {
        if (_draged == false)
        {
            // ищем новый объект
            var hits = Physics.RaycastAll(_camera.position, _camera.forward * _rayLength, _rayLength, _layerMask);
            _selectedObject = null;
            if (hits.Length > 0)
            {
                Debug.Log(hits.Length);
                _selectedObject = hits[0].transform.GetComponent<Selectable>();
            }
                
        }
        if (_draged && Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f)
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel") * _rangeChangeSpeed;
            _link.ChangeDist(scrollDelta);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryDrag();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Drop();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_camera.position, _camera.forward * _rayLength);
    }

    public bool TryDrag()
    {
        if (_selectedObject != null)
        {
            Debug.Log("Drug successful");
            _link.MakeLink(_selectedObject.GetComponent<Rigidbody>());
            _draged = true;
            return true;
        }
        else
        {
            Debug.Log("Drug failed");
            return false;
        }
    }

    public void Drop()
    {
        _link.DestroyLink();
        Debug.Log("Drop");
        _draged = false;
    }
}
