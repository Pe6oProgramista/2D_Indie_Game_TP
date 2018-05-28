using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class GrapplingHook : MonoBehaviour {

    private Rigidbody2D AnchorRb;
    private SpriteRenderer AnchorSprite;
    private bool ropeAttached;
    private List<Vector2> ropePositions = new List<Vector2>();
    private bool distanceSet;
    private Vector3 targetPos;

    public float ropeLength;
    public DistanceJoint2D joint;
    public GameObject Anchor;
    public LineRenderer ropeRenderer;
    public Transform RopeHand;

    void Awake () {
        joint.enabled = false;
        AnchorRb = Anchor.GetComponent<Rigidbody2D>();
        AnchorSprite = Anchor.GetComponent<SpriteRenderer>();
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

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        HandleInput(aimDirection);
        UpdateRopePositions();
    }

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (ropeAttached)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    while(joint.distance > 0.2)
                        joint.distance -= 0.1f;
                }
                return;
            }
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(RopeHand.position, aimDirection, ropeLength);

            if (hit.collider != null && (hit.collider.gameObject.tag.Equals("Grippable") || hit.collider.gameObject.tag.Equals("Ground")))
            {
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    // Jump slightly to distance the player a little from the ground after grappling to something.
                    //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);

                    ropePositions.Add(hit.point);
                    joint.distance = Vector2.Distance(RopeHand.position, hit.point);
                    joint.enabled = true;
                    AnchorSprite.enabled = true;
                }
            }
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                joint.enabled = false;
            } 
        }

        if (!Input.GetKey(KeyCode.W))
        {
            ResetRope();
        }
    }

    private void ResetRope()
    {
        joint.enabled = false;
        ropeAttached = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, RopeHand.position);
        ropeRenderer.SetPosition(1, RopeHand.position);
        ropePositions.Clear();
        AnchorSprite.enabled = false;
        AnchorRb.transform.position = new Vector3(RopeHand.position.x, RopeHand.position.y, 0);
    }

    private void UpdateRopePositions()
    {
        if (!ropeAttached)
        {
            return;
        }

        ropeRenderer.positionCount = ropePositions.Count + 1;

        for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRenderer.positionCount - 1)
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if (ropePositions.Count == 1)
                    {
                        AnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            joint.distance = Vector2.Distance(RopeHand.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        AnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            joint.distance = Vector2.Distance(RopeHand.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    AnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        joint.distance = Vector2.Distance(RopeHand.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                ropeRenderer.SetPosition(i, RopeHand.position);
            }
        }
    }
}
