using UnityEngine;

public class ThrowableObjectContainer : MonoBehaviour, IKitchenObjectParent
{
    private const float INTERACT_DISTANCE_XZ = .25f;
    private const float INTERACT_DISTANCE_Y = 2f;
    private const float THROW_SPEED = 10f;
    private const float THROW_SPEED_UP = 1f;
    private const float CAST_DISTANCE = 0.5f;

    [SerializeField] private LayerMask counterLayerMask;

    private KitchenObject kitchenObject;

    private Vector3? velocity = null;
    private Vector3? position = null;

    private void Update()
    {
        if (!velocity.HasValue || !position.HasValue)
        {
            return;
        }

        velocity += Physics.gravity * Time.deltaTime;
        position += velocity * Time.deltaTime;

        transform.position = position.Value;

        if (Physics.CapsuleCast(transform.position + Vector3.down * INTERACT_DISTANCE_Y, transform.position, INTERACT_DISTANCE_XZ, velocity.Value.normalized, out RaycastHit hit, CAST_DISTANCE, counterLayerMask)
            || Physics.SphereCast(transform.position, INTERACT_DISTANCE_XZ, velocity.Value.normalized, out hit, CAST_DISTANCE, counterLayerMask))

        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                baseCounter.Interact(this);
                Destroy(gameObject);

                return;
            }
            else
            {
                Destroy(gameObject);

                return;
            }
        }

        if (transform.position.y < 0f)
        {
            Destroy(gameObject);

            return;
        }
    }

    public void Throw(IKitchenObjectParent parent, Vector3 direction)
    {
        if (parent.HasKitchenObject() && parent.GetKitchenObject().GetKitchenObjectSO().throwable)
        {
            transform.position = parent.GetKitchenObjectFollowTransform().position;
            parent.GetKitchenObject().SetKitchenObjectParent(this);
            
            position = transform.position;
            velocity = direction * THROW_SPEED + Vector3.up * THROW_SPEED_UP;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return transform;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
