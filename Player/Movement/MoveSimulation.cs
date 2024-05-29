using System;
using UnityEngine;

// logic for moving the player
public class MoveSimulation
{
    public static MoveSimulation Instance { get; private set; }
    public Action<Vector3> OnTargetAim;
    public Action<Vector3> OnChangePosition;

    public float CurrentSpeed;          // used for animations and other stuff
    
    public MoveSimulation Startup()
    {
        if (Instance != null)
        {
            return Instance;
        }
        Instance = this;
        return Instance;
    }

    public void Cleanup()
    {
        Instance = null;
    }

    public Vector3 UpdatePosition(Vector2 dir, float move_spd)
    {
        float y = dir.y;
        Vector3 move_dir = Vector3.forward * y;
        Vector3 new_pos = move_dir * move_spd * Time.deltaTime;
        return new_pos;
    }

    public Vector3 UpdateStrafePosition(int strafe_dir, float move_spd)
    {
        Vector3 move_dir = Vector3.right * strafe_dir;
        Vector3 new_pos = move_dir * move_spd * Time.deltaTime;
        return new_pos;
    }

    public Vector3 UpdateVelocity(Vector3 velocity, Vector3 current, float accel)
    {
        Vector3 new_vel = Vector3.Lerp(velocity, current, accel * Time.deltaTime);
        return new_vel;
    }

    public Vector3 UpdateRotation(Vector2 dir, float turn_spd)
    {
        float x = dir.x;
        Vector3 new_rot = new Vector3(0, x * turn_spd * Time.deltaTime, 0);
        return new_rot;
    }

    public float GetSpeed(Vector3 a, Vector3 b)
    {
        float speed = Vector3.Distance(a, b) / Time.deltaTime;
        CurrentSpeed = speed;
        return speed;
    }

    public void SetTargetRotation(Transform target)
    {
        OnTargetAim?.Invoke(target.position);
    }

    public void SetPosition(Vector3 pos)
    {
        OnChangePosition?.Invoke(pos);
    }
}