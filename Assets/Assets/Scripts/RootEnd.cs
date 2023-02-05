using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEnd : MonoBehaviour
{
    public const float TRANSITION_TIME = 0.5f;
    private float m_elapsedTime;

    private bool m_deleteAfterTransition = false;
    private bool m_transitioning = false;

    private Vector3 m_destination;
    private RootNode m_root;

    public PlayerController playerController;

    public void MarkForDelete()
    {
        m_deleteAfterTransition = true;
    }

    public bool IsMarkedForDeletion() { return m_deleteAfterTransition; }

    public bool IsClaimed() { return m_transitioning; }

    public void BeginTransition( Vector3 destination, RootNode root )
    {
        m_transitioning = true;
        m_elapsedTime = 0.0f;
        m_destination = destination;
        m_destination.z = -6.0f;
        m_root = root;  //Could probably just use this as an indicator of deletion
    }

    // Update is called once per frame
    void Update()
    {
        if( m_transitioning )
        {
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= TRANSITION_TIME)
            {
                m_transitioning = false;
                transform.position = m_destination;
                if( m_deleteAfterTransition )
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, m_destination, m_elapsedTime / TRANSITION_TIME * 0.25f);
            }
        }
    }

    public void ChangeDirection( RootNode.Direction newDirection )
    {
        m_root.direction = newDirection;
    }

    public RootNode.Direction GetRootGrowthDirection()
    {
        return m_root.direction;
    }

    void OnMouseDown()
    {
        if(playerController != null && !m_transitioning) 
        {
            playerController.UseActionOn(this);
        }
    }
}
