using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallTrajectoryPredic
{
    public Queue<NextContact> NextContacts { get; private set; } = new();

    private LayerMask layerMaskNextContacts;
    private float ballWidth;
    private int numOfContacts;

    private int resolution = 1000;

    public BallTrajectoryPredic(LayerMask layerMask, float ballWidth, int maxNumOfContacts = 10)
    {
        layerMaskNextContacts = layerMask;
        this.ballWidth = ballWidth;
        numOfContacts = maxNumOfContacts;
    }

    private bool CylinderRayCast(Vector2 origin, Vector2 direction, float distance, float width, out RaycastHit2D hit)
    {
        float step = distance / resolution;

        hit = Physics2D.CircleCast(origin + direction * 0.1f, width / 2f, direction, distance, layerMaskNextContacts);

        return hit;
    }

    private bool CalculateNextContact(Vector2 position, Vector2 velocity, out NextContact nextContact)
    {
        nextContact = new NextContact();

        if (!CylinderRayCast(position, velocity.normalized, 100f, ballWidth, out RaycastHit2D hit))
            return false;

        nextContact = new NextContact()
        {
            point = hit.centroid,
            normal = hit.normal,
            inVector = velocity,
            outVector = Vector2.Reflect(velocity, hit.normal),
            type = hit.collider.tag,
        };

        return true;
    }

    public void CalculateNextContacts(Vector2 point, Vector2 normal, Vector2 ballVelocity)
    {
        int contactsNeeded = numOfContacts - NextContacts.Count;

        NextContact lastContact = NextContacts.LastOrDefault();

        if (lastContact.type is null)
        {
            if (!CalculateNextContact(point + normal * (ballWidth / 2f), ballVelocity, out NextContact lContact))
                return;

            lastContact = lContact;
            NextContacts.Enqueue(lContact);
            contactsNeeded--;
        }

        for (int i = 0; i < contactsNeeded; i++)
        {
            if (!CalculateNextContact(lastContact.point + lastContact.normal * (ballWidth / 2f), lastContact.outVector, out NextContact nContact))
                break;

            NextContacts.Enqueue(nContact);
            lastContact = nContact;
        }
    }

    public bool CheckLayer(LayerMask layer)
        => (layerMaskNextContacts.value & (1 << layer)) != 0;

    public void Dequeue()
    {
        if (NextContacts.Count != 0)
            NextContacts.Dequeue();
    }

    public void Clear()
    {
        if (NextContacts.Count != 0)
            NextContacts.Clear();
    }

    public void DrawGizmos(Ball ball)
    {
        Vector2 lastPoint = ball.transform.position;

        if (NextContacts.Count == 0)
        {
            Gizmos.DrawRay(lastPoint, Ball.Velocity);
            return;
        }

        foreach (NextContact contact in NextContacts)
        {
            Gizmos.DrawLine(lastPoint, contact.point + contact.normal * (ballWidth / 2f));
            Gizmos.DrawSphere(contact.point + contact.normal * (ballWidth / 2f), 0.1f);
            lastPoint = contact.point + contact.normal * (ballWidth / 2f);
        }
    }
}

public struct NextContact
{
    public Vector2 point;
    public Vector2 normal;
    public Vector2 inVector;
    public Vector2 outVector;
    public string type;
}
