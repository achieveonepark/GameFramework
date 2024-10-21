using PureMVC.Patterns.Mediator;
using UnityEngine;

public class MonoBehaviourMediator : Mediator
{
    public MonoBehaviourMediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {

    }
    
    public GameObject ViewGameObject
    {
        get
        {
            return (GameObject)ViewComponent;
        }
    }
}