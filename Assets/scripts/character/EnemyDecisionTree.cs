using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EnemyDecisionTree : MonoBehaviour
{
    
    ConditionNode rootDecision;
    public Enemy enemyScript;
    
    public IEnumerator AIisOn()
    {
        //Debug.Log("Coroutine started");
        while (true)
        {
            if(enemyScript.enemyState == Enemy.EnemyState.Idle)
            {
                if(rootDecision == null)
                {
                    Debug.Log(" rootdecisionnull");
                }
                else
                {
                    rootDecision.Evaluate();
                }
                
                Debug.Log("decision taken");
                
            }
            //Debug.Log("AI is on");
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void SetRootDecision(ConditionNode rootDecision)
    {
        if (rootDecision != null)
        {
            this.rootDecision = rootDecision;
            //Debug.Log("root decision set");
        }

    }
}

public abstract class DecisionTreeNode
{
    public  abstract void  Evaluate();
}

public class ConditionNode: DecisionTreeNode
{
    Func<bool> condition;
    private DecisionTreeNode trueNode;
    private DecisionTreeNode falseNode;

    public ConditionNode(Func<bool> condition, DecisionTreeNode trueNode, DecisionTreeNode falseNode)
    {
        this.trueNode = trueNode;
        this.falseNode = falseNode;
        this.condition = condition;
    }
    public override void Evaluate()
    {
        if (condition())
        {
            trueNode.Evaluate();
        }
        else
        {
            falseNode.Evaluate(); 
        }
    }

}
public class ActionNode: DecisionTreeNode 
{
    Action action;
    Func<IEnumerator> coroutine;
    Enemy Host;
    public ActionNode(Action action) 
    { 
        this.action = action;
    }

    public ActionNode( Func<IEnumerator> coroutine, Enemy host) 
    {
        this.coroutine = coroutine;
        Host = host;
    }

    public override void Evaluate()
    {
        if(coroutine != null && Host!=null)
        {
            Debug.Log("lalala");
            Host.StartCoroutine(coroutine.Invoke());
        }
        else if(action != null) 
        {
            action.Invoke();
        }
        
    }
}
