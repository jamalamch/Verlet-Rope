using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBridge : RopeBase
{
    public Transform EndPoint;

    protected override void Start()
    {
        float distance = Vector3.Distance(EndPoint.position, StartPoint.position);

        ropeSegLen = distance*0.93f/ segmentLength;

        this.lineRenderer = this.GetComponent<LineRenderer>();

        for (int i = 0; i < segmentLength; i++)
        {
            Vector3 ropeStartPoint = Vector3.Lerp(StartPoint.position, EndPoint.position, (i * ropeSegLen) / (distance * 0.93f));
            ropeStartPoint.y += Mathf.SmoothStep(0, distance * 0.5f,1 - Mathf.Abs((distance * 0.5f) - i * ropeSegLen)/ (distance*0.5f));
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
        }
    }

    protected override void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.StartPoint.position;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = this.EndPoint.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.ropeSegments.Count - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }
}
