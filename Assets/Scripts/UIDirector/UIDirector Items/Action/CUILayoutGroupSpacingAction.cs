using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UIDirector
{
    [CTimelineItemAttribute("LayoutGroup", "Spacing To", CTimelineEnum.EItemType.AgentItem)]
    public class CUILayoutGroupSpacingAction : CTimelineAgentAction
    {
        public Ease easeType = Ease.Linear;

        public float TargetSpacing = 0;

        public class CAgentData
        {
            public float m_defaultSpacing;
        }
        private System.Collections.Generic.Dictionary<int, CAgentData> m_dicAgentData = new System.Collections.Generic.Dictionary<int, CAgentData>();
        CAgentData _getAgentData(int hashCode)
        {
            if (!m_dicAgentData.ContainsKey(hashCode))
            {
                CAgentData newData = new CAgentData();
                m_dicAgentData.Add(hashCode, newData);
            }

            return m_dicAgentData[hashCode];
        }

        public override void Initialize(GameObject agent)
        {
            base.Initialize(agent);

            HorizontalOrVerticalLayoutGroup layout = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (layout != null)
            {
                _getAgentData(agent.GetHashCode()).m_defaultSpacing = layout.spacing;
            }
        }

        public override void Trigger(GameObject agent)
        {
            HorizontalOrVerticalLayoutGroup layout = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (layout != null)
            {
                float from = _getAgentData(agent.GetHashCode()).m_defaultSpacing;
                layout.spacing = DOVirtual.EasedValue(from, TargetSpacing, 0, easeType);
            }
        }

        public override void End(GameObject agent)
        {
            HorizontalOrVerticalLayoutGroup layout = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (layout != null)
            {
                layout.spacing = TargetSpacing;
            }
            //Debug.Log("<End> " + agent.GetHashCode().ToString());
        }

        public override void UpdateTime(GameObject agent, float time, float deltaTime)
        {
            base.UpdateTime(agent, time, deltaTime);

            HorizontalOrVerticalLayoutGroup layout = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();

            if (time == 0)
            {
                if (layout != null)
                {
                    layout.spacing = _getAgentData(agent.GetHashCode()).m_defaultSpacing;
                }
                Trigger(agent);
            }
            else
            {
                if (layout != null)
                {
                    float from = _getAgentData(agent.GetHashCode()).m_defaultSpacing;
                    layout.spacing = DOVirtual.EasedValue(from, TargetSpacing, Mathf.Clamp01(time / this.Duration), easeType);
                }
            }
        }

        public override void SetTime(GameObject agent, float time, float deltaTime)
        {
            HorizontalOrVerticalLayoutGroup layout = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();

            if (time == 0)
            {                
                if (layout != null)
                {                    
                    layout.spacing = _getAgentData(agent.GetHashCode()).m_defaultSpacing;
                }
                Trigger(agent);
            }
            else if (Mathf.Approximately(time, this.Duration))
            {
                End(agent);
            }
            else if (time > 0 && time < this.Duration)
            {
                if (layout != null)
                {
                    float from = _getAgentData(agent.GetHashCode()).m_defaultSpacing;
                    layout.spacing = DOVirtual.EasedValue(from, TargetSpacing, Mathf.Clamp01(time / this.Duration), easeType);
                }
            }
        }

        public override void Pause(GameObject agent)
        {
        }

        public override void Resume(GameObject agent)
        {
        }

        public override void Stop(GameObject agent)
        {
            if (revertWhenFinish && m_bInitialized)
            {
                HorizontalOrVerticalLayoutGroup layout = agent.GetComponent<HorizontalOrVerticalLayoutGroup>();
                if (layout != null)
                {
                    layout.spacing = _getAgentData(agent.GetHashCode()).m_defaultSpacing;
                }
            }

            base.Stop(agent);
        }
    }

}//namespace UIDirector
