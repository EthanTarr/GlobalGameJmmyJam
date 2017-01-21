using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask;

    const float skinWidth = 0.015f;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigin;
    public int horizontalRayCount = 4;
    public int VerticalRayCount = 4;

    float horizontalRaySpacing;
    float VecticalRaySpacing;
    public CollisionInfo collisions;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity) {
        UpdateRaycastOrigins();
        collisions.Reset();
        if(velocity.x != 0)
            HorizontalCollisions(ref velocity);

        if(velocity.y != 0)
            VerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.bottomLeft : raycastOrigin.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
                collisions.other = hit.transform.gameObject;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
            rayOrigin += Vector2.right * (VecticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                collisions.other = hit.transform.gameObject;
            }
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigin.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        VecticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, BottomRight;
    }

    public struct CollisionInfo{
        public bool above, below, left, right;
        public GameObject other;

        public void Reset() {
            above = below = false;
            left = right = false;
            other = null;
        }
    }
}
