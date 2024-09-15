using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;

    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        GameObject.Instantiate(stateData.deathBloodParticle, entity.transform.position, stateData.deathBloodParticle.transform.rotation);
        GameObject.Instantiate(stateData.deathChunkParticle, entity.transform.position, stateData.deathChunkParticle.transform.rotation);

        SpawnCoin();

        entity.gameObject.SetActive(false);
    }

    private void SpawnCoin()
    {   
        for (int i = 0; i < stateData.coinCount; i++)
        {
            GameObject Coin = GameObject.Instantiate(stateData.coinPrefab, entity.transform.position, Quaternion.identity);
            
            // Tính toán hướng phun trào ngẫu nhiên
            float angle = Random.Range(-stateData.spreadAngle / 2, stateData.spreadAngle / 2);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
            
            Rigidbody2D rb = Coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Áp dụng lực phun trào
                rb.AddForce(direction * stateData.eruptionForce, ForceMode2D.Impulse);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

