using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class GrapplingHook : MonoBehaviour {

    private Rigidbody2D AnchorRb;
    private SpriteRenderer AnchorSprite;
    private bool ropeAttached;
    private List<Vector2> ropePositions;
    private bool distanceSet;
    private Vector3 targetPos;

    public float ropeLength;
    public DistanceJoint2D joint;
    public GameObject Anchor;
    public LineRenderer ropeRenderer;
    public Transform RopeHand;

    Vector3 aimDirection;

    void Awake () {
        joint.enabled = false;
        AnchorRb = Anchor.GetComponent<Rigidbody2D>();
        AnchorSprite = Anchor.GetComponent<SpriteRenderer>();
        ropePositions = new List<Vector2>();
    }
	
	void Update () {
        if (transform.localScale.x < 0)
        {
            targetPos = new Vector3(RopeHand.position.x + Mathf.Cos(40), RopeHand.position.y + Mathf.Sin(40), 0f);
        }
        else
        {
            targetPos = new Vector3(RopeHand.position.x - Mathf.Cos(40), RopeHand.position.y + Mathf.Sin(40), 0f);
        }
        Vector3 facingDir = targetPos - RopeHand.position;
        var aimAngle = Mathf.Atan2(facingDir.y, facingDir.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        HandleInput(aimDirection);
        UpdateRopePositions();
    }

    bool clicked = false;

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(clicked)
            {
                float distance = Vector3.Distance(RopeHand.position, ropePositions[0]);
                if (distance < 0.1)
                {
                    ResetRope();
                    return;
                }
                ropePositions[0] = RopeHand.position + (Vector3)aimDirection * distance * 0.92f;
            }
            else
            {
                if (ropeAttached)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        while (joint.distance > 0.2)
                            joint.distance -= 0.1f;
                    }
                    return;
                }

                var hit = Physics2D.Raycast(RopeHand.position, aimDirection, ropeLength);

                if (hit.collider != null && (hit.collider.gameObject.tag.Equals("Grippable") || hit.collider.gameObject.tag.Equals("Ground")))
                {
                    ropeAttached = true;
                    if (!ropePositions.Contains(hit.point))
                    {
                        ropePositions.Clear();
                        ropePositions.Add(hit.point);
                        joint.distance = Vector2.Distance(RopeHand.position, hit.point);
                        joint.enabled = true;
                    }
                }
                else
                {
                    ropeAttached = false;
                    joint.enabled = false;
                    ropePositions.Clear();
                    ropePositions.Add(RopeHand.position + (Vector3)aimDirection * ropeLength);
                    clicked = true;
                }
                ropeRenderer.enabled = true;
                AnchorSprite.enabled = true;
            }
        }

        if (!Input.GetKey(KeyCode.W))
        {
            clicked = false;
            ResetRope();
        }
    }

    private void ResetRope()
    {
        ropeAttached = false;
        joint.enabled = false;
        ropeRenderer.enabled = false;
        AnchorSprite.enabled = false;

        ropeRenderer.positionCount = 0;
        ropePositions.Clear();
        AnchorRb.transform.position = RopeHand.position;
    }

    private void UpdateRopePositions()
    {
        ropeRenderer.positionCount = ropePositions.Count + 1;
        for (int i = 0; i < ropeRenderer.positionCount; i++)
        {
            if (i == 0)
            {
                ropeRenderer.SetPosition(i, RopeHand.position);
            }
            else
            {
                AnchorSprite.enabled = true;
                var pos = ropePositions[i - 1];
                ropeRenderer.SetPosition(i, pos);
                var sinSide = Mathf.Abs(pos.y - ropeRenderer.GetPosition(0).y);
                var cosSide = Mathf.Abs(pos.x - ropeRenderer.GetPosition(0).x);
                var hipotenuse = Mathf.Sqrt(Mathf.Pow(cosSide, 2) + Mathf.Pow(sinSide, 2));
                Debug.Log(cosSide);
                if (pos.x < ropeRenderer.GetPosition(0).x)
                {
                    AnchorRb.transform.rotation = Quaternion.Euler(0, 0, -(Mathf.Asin(sinSide / hipotenuse) * 180 / Mathf.PI + 90));
                }
                else
                {
                    AnchorRb.transform.rotation = Quaternion.Euler(0, 0, Mathf.Asin(sinSide / hipotenuse) * 180 / Mathf.PI + 90);
                }
                AnchorRb.transform.position = pos;

                if(ropeAttached)
                {
                    if (!distanceSet)
                    {
                        joint.distance = Vector2.Distance(RopeHand.position, pos);
                        distanceSet = true;
                    }
                }
            }
        }
    }
}
