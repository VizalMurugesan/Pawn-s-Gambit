using UnityEngine;

//ENEMY AI BEHAVIOUR
public class EnemyPawnAI : EnemyDecisionTree
{
    EnemyPawn enemy;
    public bool AIon = false;
    private void Start()
    {
        enemy = GetComponent<EnemyPawn>();
        CreateDecisionTree();
    }
    
    void CreateDecisionTree()
    {
        if (enemy.player == null)
    {
        //Debug.LogWarning("Player reference is missing. Decision tree not created.");
        return;
    }
       // Debug.Log("decision tree created");
        ActionNode attack = new ActionNode(() => enemy.StartCoroutine(enemy.Attack()));
        ActionNode chasePlayer = new ActionNode(() => enemy.StartCoroutine(enemy.ChasePlayer()));
        ActionNode doNothing = new ActionNode(enemy.DoNothing);

        ConditionNode isChasing = new ConditionNode(enemy.IsChasing,doNothing, chasePlayer);
        ConditionNode isAttacking = new ConditionNode(enemy.IsAttacking, doNothing, attack);

        ConditionNode isPlayerNear = new ConditionNode(enemy.IsPlayerNear, isAttacking, isChasing);
        SetRootDecision(isPlayerNear);
        
    }
}
