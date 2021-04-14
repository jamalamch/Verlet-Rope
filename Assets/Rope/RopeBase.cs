using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RopeBase : MonoBehaviour
{
    [SerializeField]
    protected float ropeSegLen = 0.25f;
    [SerializeField]
    protected int segmentLength = 20;
    [SerializeField]
    protected float lineWidth = 0.1f;
    [SerializeField]
    protected Vector2 forceGravity = new Vector2(0f, 10f);

    protected LineRenderer lineRenderer;
    protected List<RopeSegment> ropeSegments = new List<RopeSegment>();

    public Transform StartPoint;


    private float fixedUpdateTimer = 0.05f;
    private float timer = 0.05f;

    // Use this for initialization
    protected virtual void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = StartPoint.position;

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.time > timer)
        {
            this.DrawRope();
            this.Simulate();

            timer = Time.time + fixedUpdateTimer;
        }
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void Simulate()
    {
        for (int i = 1; i < this.ropeSegments.Count; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    protected virtual void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.ropeSegments.Count; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    protected abstract void ApplyConstraint();

}
