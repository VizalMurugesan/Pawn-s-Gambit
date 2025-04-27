using System.Linq.Expressions;
using UnityEngine;

public class EnemyBossMageAI : EnemyDecisionTree
{
    EnemyBossMage enemy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<EnemyBossMage>();
        CreateDecisionTree();
    }

    void CreateDecisionTree()
    {
        ActionNode normalAttack = new ActionNode(enemy.Attack, enemy);
        ActionNode MeteorStrike = new ActionNode(enemy.UseAbilityMeteor, enemy);
        ActionNode GoToPlayer = new ActionNode(enemy.ChasePlayer,enemy);
        ActionNode teleportAwayFromPlayer = new ActionNode(() => enemy.TeleportAwayFromPlayer());
        ActionNode teleportToHealth = new ActionNode(()=>  enemy.TeleportToHealth());

        ConditionNode CanDoNormalAttack = new ConditionNode(()=> enemy.CanDoNormalAttack(), normalAttack, GoToPlayer);
        ConditionNode canUseMeteor = new ConditionNode(()=> enemy.IsMeteorAvailable(), MeteorStrike, CanDoNormalAttack);
        ConditionNode CanUseTPtoHealth = new ConditionNode(() => enemy.IsTPAvailable(), teleportToHealth, canUseMeteor);
        ConditionNode CanUseTPAwayfromPlayer = new ConditionNode(() => enemy.IsTPAvailable(), teleportAwayFromPlayer, canUseMeteor);
        ConditionNode isPlayerNear = new ConditionNode(() => enemy.IsPlayerNear(), CanUseTPAwayfromPlayer, canUseMeteor);
        ConditionNode IsHealing = new ConditionNode(() => enemy.IsHealing(), isPlayerNear, CanUseTPtoHealth);

        ConditionNode NeedHealing = new ConditionNode(() => enemy.IsInNeedOfHealing(), IsHealing, isPlayerNear);


        SetRootDecision(CanUseTPAwayfromPlayer);
    }
}
