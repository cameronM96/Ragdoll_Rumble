using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    public bool overRideAble = true;
    public Activity[] activities;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState (StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions (StateController controller)
    {
        for (int i = 0; i < activities.Length; i++)
        {
            activities[i].Act(controller);
        }
    }

    private void CheckTransitions (StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionsSucceeded = transitions[i].decision.Decide(controller);

            if (decisionsSucceeded)
                controller.TransitionToState(transitions[i].trueState);
            else
                controller.TransitionToState(transitions[i].falseState);
        }
    }
}
