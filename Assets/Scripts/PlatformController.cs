using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    List<GameObject> Platforms = new List<GameObject>();
    List<CollisionSendPlayer> PlatformsComp = new List<CollisionSendPlayer>();
    float TimeUntilNextMove = 1;
    float Timer;
    bool topReached;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Platforms.Add(transform.GetChild(i).gameObject);
            PlatformsComp.Add(transform.GetChild(i).gameObject.AddComponent<CollisionSendPlayer>());
        }
        TimeUntilNextMove = Random.Range(2.5f, 8);
    }

    private void Update()
    {
        NextMove();
    }
    void NextMove()
    {
        if(Time.time - Timer > TimeUntilNextMove)
        {
            int RandomINT = Random.Range(0, Platforms.Count - 1);
            if(PlatformsComp[RandomINT].CurrentState == CollisionSendPlayer.STATE.Idle)
            {
                PlatformsComp[RandomINT].ChangeState(CollisionSendPlayer.STATE.GoUp);
            }
            Timer = Time.time;
        }
    }
}

public class CollisionSendPlayer : MonoBehaviour
{
    public enum STATE { GoUp, GoDown, Wait, Idle}
    List<GameObject> RecentlyHit = new List<GameObject>();
    Vector3 HighestPoint;
    Vector3 StartPoint;

    float Timer;
    public STATE CurrentState;

    private void Start()
    {
        StartPoint = transform.position;

        HighestPoint = transform.position;
        HighestPoint.y += 6;
        ResetTimer();
    }

    private void Update()
    {
        ActOnState();
    }

    void ActOnState()
    {
        switch (CurrentState)
        {
            case STATE.GoUp:
                transform.position = Vector3.Lerp(StartPoint, HighestPoint, Time.time - Timer);
                if (Time.time - Timer > 1)
                {
                    ChangeState(STATE.Wait);
                    ResetTimer();
                }
                break;
            case STATE.GoDown:
                transform.position = Vector3.Lerp(HighestPoint, StartPoint, Time.time - Timer);
                if (Time.time - Timer > 1)
                {
                    ChangeState(STATE.Idle);
                    ResetTimer();
                }
                break;
            case STATE.Wait:
                if(Time.time - Timer > 1)
                {
                    ResetTimer();
                    ChangeState(STATE.GoDown);
                }
                break;
            case STATE.Idle:

                break;
            default:
                break;
        }
    }

    void ResetTimer()
    {
        Timer = Time.time;
    }

    public void ChangeState(STATE _State)
    {
        ResetTimer();
        CurrentState = _State;
        while(RecentlyHit.Count > 0) RecentlyHit.RemoveAt(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CharacterController Temp = collision.gameObject.GetComponent<CharacterController>();
        
        if (Temp)
        {
            for (int i = 0; i < RecentlyHit.Count; i++)
                if (RecentlyHit[i] == collision.gameObject)
                    return;
            RecentlyHit.Add(collision.gameObject);
            Temp.Launch(6); 
        }
    }
}
