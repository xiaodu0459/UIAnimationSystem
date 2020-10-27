using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIDirector
{
    [CTimelineItemAttribute("LayoutGroup", "Set Spacing", CTimelineEnum.EItemType.AgentItem)]
    public class CUISetLayoutGroupSpacingEvent : CTimelineAgentEvent
    {
        public int Spacing = 0;

        public override void Trigger(GameObject agent)
        {
            HorizontalOrVerticalLayoutGroup b = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();

            if (b != null)
            {
                b.spacing = Spacing;
            }
        }
    }

}//namespace UIDirector
