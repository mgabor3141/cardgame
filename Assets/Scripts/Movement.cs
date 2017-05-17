using UnityEngine;
using UnityEngine.Networking;

public class Movement : NetworkBehaviour
{
    private bool _resting = false;
    public void Wake() { _resting = false; }

    [SyncVar]
    private Vector3 _targetPosition;
    public Vector3 TargetPosition
    {
        get
        {
            return _targetPosition;
        }
        set
        {
            _resting = false;
            _targetPosition = value;
        }
    }

    private IContainer _container;
    public IContainer Container
    {
        get
        {
            return _container;
        }
        set
        {
            if (value != null)
            {
                IFlippable flippable = GetComponent<IFlippable>();
                if (flippable != null)
                    switch (value.AutoFacing)
                    {
                        case AutoFacingOptions.Down:
                            flippable.FacingUp = false;
                            break;
                        case AutoFacingOptions.Up:
                            flippable.FacingUp = true;
                            break;
                    }
            }

            _container = value;
        }
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;
        _targetPosition = position;
    }

    public void Awake()
    {
        TargetPosition = transform.position;
    }

    public void Update()
    {
        if (_resting) return;

        if ((_targetPosition - transform.position).magnitude < 0.0005)
        {
            _resting = true;
            return;
        }

        transform.position += (_targetPosition - transform.position) * 20 * Time.deltaTime;
    }
}
