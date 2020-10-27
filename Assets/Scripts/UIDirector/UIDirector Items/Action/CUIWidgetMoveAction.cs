// ========================================================================
// UGUI Animation
// Created: 2016/11/09

// Brief: Move the agent to the position during a time
// ========================================================================
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UIDirector
{
    [CTimelineItemAttribute("Transform", "Move To", CTimelineEnum.EItemType.AgentItem)]
    public class CUIWidgetMoveAction : CTimelineAgentAction
    {
        public Ease easeType = Ease.Linear;

        public Vector3 MoveTo = Vector3.zero;
        public bool isRelative = false;

        public class CAgentData
        {
            public Tweener m_Tweener;
            public Vector3 m_defaultPos;
        }
        private System.Collections.Generic.Dictionary<int, CAgentData> m_dicAgentData = new System.Collections.Generic.Dictionary<int, CAgentData>();
        //private Tweener m_Tweener;
        //private Vector3 m_defaultPos;
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

            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {               
                _getAgentData(agent.GetHashCode()).m_defaultPos = rect.anchoredPosition3D;
                //if (agent.name.Contains("DungeonResultInfo"))
                //    Debug.Log("<Initialize> " + agent.name + ", DefaultPos: " + rect.anchoredPosition3D.ToString());
            }
        }

        public override void Trigger(GameObject agent)
        {
            RectTransform rect = agent.GetComponent<RectTransform>();
            if (rect != null)
            {
                if (isRelative)
                {
                    _getAgentData(agent.GetHashCode()).m_Tweener = rect.DOAnchorPos(rect.anchoredPosition3D + MoveTo, duration).SetEase(easeType);
                    //Debug.Log("<Trigger Move> " + duration.ToString() + ", " + agent.GetHashCode().ToString());
                    //if (agent.name.Contains("DungeonResultInfo"))
                    //    Debug.Log("<Trigger Move> " + agent.name + ", MoveTO: " + MoveTo.ToString() + ", Duration: " + duration.ToString() + "fireTime: " + fireTime);
                }
                else
                {
                    _getAgentData(agent.GetHashCode()).m_Tweener = rect.DOAnchorPos(MoveTo, duration).SetEase(easeType);
                }                
            }
        }

        public override void End(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.Complete();
                _getAgentData(agent.GetHashCode()).m_Tweener = null;
            }
            else
            {
                RectTransform rect = agent.GetComponent<RectTransform>();
                if (rect != null)
                {
                    if (isRelative)
                    {
                        rect.anchoredPosition3D = _getAgentData(agent.GetHashCode()).m_defaultPos + MoveTo;
                    }
                    else
                    {
                        rect.anchoredPosition3D = MoveTo;
                    }                    
                }
            }
        }

        public override void SetTime(GameObject agent, float time, float deltaTime)
        {
            if (time == 0)
            {
                RectTransform rect = agent.GetComponent<RectTransform>();
                if (rect != null)
                {
                    //rect.DOAnchorPos(m_defaultPos, 0.0f);
                    rect.anchoredPosition3D = _getAgentData(agent.GetHashCode()).m_defaultPos;
                }
                Trigger(agent);
            }
            else if (Mathf.Approximately(time, this.Duration))
            {
                End(agent);
            }
            else if (time > 0 && time < this.Duration)
            {
                if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
                {
                    _getAgentData(agent.GetHashCode()).m_Tweener.Goto(time);
                }
            }
        }

        public override void Pause(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null)
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.Pause();
            }
        }

        public override void Resume(GameObject agent)
        {
            if (_getAgentData(agent.GetHashCode()).m_Tweener != null && !_getAgentData(agent.GetHashCode()).m_Tweener.IsPlaying())
            {
                _getAgentData(agent.GetHashCode()).m_Tweener.TogglePause();
            }
        }

        public override void Stop(GameObject agent)
        {
            if (revertWhenFinish && m_bInitialized)
            {
                RectTransform rect = agent.GetComponent<RectTransform>();
                if (rect != null)
                {
                    //rect.DOAnchorPos(m_defaultPos, 0.0f);
                    rect.anchoredPosition3D = _getAgentData(agent.GetHashCode()).m_defaultPos;
                }
            }

            base.Stop(agent);
            //Debug.Log("<Stop> " + agent.GetHashCode().ToString());
        }
    }

}//namespace UIDirector