using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string animBoolName;

    public PlayerStates(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() //when entering a specific state
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
    }

    public virtual void Exit() //when exiting a specific state
    {
        player.Anim.SetBool(animBoolName, false);


    }

    public virtual void LogicUpdate() //called every frame
    {

    }

    public virtual void PhysicsUpdate() //called every fixed update
    {
        DoChecks();
    }

    public virtual void DoChecks() //called from physics update and from enter
    {

    }

}
