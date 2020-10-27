// ========================================================================
// UGUI Animation
// Created: 2016/11/15

// Brief: Enable GameObject or not
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{
    [CTimelineItemAttribute("Enable", "GameObject", CTimelineEnum.EItemType.AgentItem)]
    public class CUIEnableGameObjectEvent : CTimelineAgentEvent
    {
        public GameObject TargetObject = null;
        public bool UseAgent = true;
        public bool IsEnable = false;

        public override void Trigger(GameObject agent)
        {
            if (TargetObject != null) TargetObject.SetActive(IsEnable);            
            else if (UseAgent)
            {
                agent.SetActive(IsEnable);
            }
        }

    }

}//namespace UIDirector