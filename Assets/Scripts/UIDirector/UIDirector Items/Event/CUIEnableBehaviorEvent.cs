// ========================================================================
// UGUI Animation
// Created: 2016/11/15

// Brief: Enable monobehavior or not
// ========================================================================
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace UIDirector
{
    [CTimelineItemAttribute("Enable", "MonoBehavior", CTimelineEnum.EItemType.AgentItem)]
    public class CUIEnableBehaviorEvent : CTimelineAgentEvent
    {
        public Component TargetBehavior;
        public bool IsEnable = false;

        public override void Trigger(GameObject agent)
        {
            Component b = agent.GetComponent(TargetBehavior.GetType()) as Component;
            if (b != null)
            {                
                PropertyInfo fieldInfo = TargetBehavior.GetType().GetProperty("enabled");
                if (fieldInfo != null)
                    fieldInfo.SetValue(TargetBehavior, IsEnable, null);
                else
                    Debug.LogError("<CUIEnableBehaviorEvent.Trigger> can't find property \"enabled\" in " + b.name + " of GameObject " + agent.name);
            }
        }
    }

}//namespace UIDirector