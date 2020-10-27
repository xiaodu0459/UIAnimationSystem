using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIDirector
{
    [CTimelineTrackAttribute("Agent Track", new CTimelineEnum.ETrackType[] { CTimelineEnum.ETrackType.AgentTrack, CTimelineEnum.ETrackType.MultiAgentTrack }, CTimelineEnum.EItemType.AgentItem)]
    public class CAgentItemTrack : CTimelineTrack
    {
        public static readonly bool EnableLog = false;
        public static void TraceInfo(string message)
        {
            if (EnableLog)
                Debug.Log(message);
        }

        #region Fields
        [SerializeField]
        protected bool m_UseInterval = false;

        [SerializeField]
        protected float m_IntervalTime = 0f; 
        #endregion //Fields

        #region Properties
        public Transform Agent
        {
            get
            {
                CAgentTrackGroup trackGroup = this.TrackGroup as CAgentTrackGroup;
                if (trackGroup == null)
                {
                    Debug.LogError("No CAgentTrackGroup found on parent.", this);
                    return null;
                }

                return trackGroup.Agent;
            }
        }

        public List<Transform> Agents
        {
            get
            {
                CAgentTrackGroup trackGroup = this.TrackGroup as CAgentTrackGroup;
                if (trackGroup != null)
                {
                    List<Transform> agents = new List<Transform>() { };
                    agents.Add(trackGroup.Agent);
                    return agents;
                }

                CMultiAgentTrackGroup multiAgentTrackGroup = this.TrackGroup as CMultiAgentTrackGroup;
                if (multiAgentTrackGroup != null)
                {
                    return multiAgentTrackGroup.Agents;
                }

                return null;
            }
        }

        public List<Transform> SequenceAgents
        {
            get
            {
                CAgentTrackGroup trackGroup = this.TrackGroup as CAgentTrackGroup;
                if (trackGroup != null)
                {
                    if (trackGroup.Agent != null)
                    {
                        List<Transform> agents = new List<Transform>() { };

                        for (int i = 0; i < trackGroup.Agent.childCount; i++)
                        {
                            agents.Add(trackGroup.Agent.GetChild(i).transform);
                        }

                        return agents;
                    }
                }

                CMultiAgentTrackGroup multiAgentTrackGroup = this.TrackGroup as CMultiAgentTrackGroup;
                if (multiAgentTrackGroup != null)
                {
                    return multiAgentTrackGroup.Agents;
                }

                return null;
            }
        }

        public CTimelineAgentEvent[] AgentEvents
        {
            get
            {
                return base.GetComponentsInChildren<CTimelineAgentEvent>();
            }
        }

        public CTimelineAgentAction[] AgentActions
        {
            get
            {
                return base.GetComponentsInChildren<CTimelineAgentAction>();
            }
        }
        #endregion //Properties

        public override void Initialize()
        {
            base.Initialize();

            foreach (CTimelineAgentEvent agentEvent in this.AgentEvents)
            {
                foreach (Transform agent in Agents)
                {
                    if (agent != null)
                    {
                        agentEvent.Initialize(agent.gameObject);
                    }
                }

                if (m_UseInterval)
                {
                    foreach (Transform agent in SequenceAgents)
                    {
                        if (agent != null)
                        {
                            agentEvent.Initialize(agent.gameObject);
                        }
                    }
                }
            }

            foreach (CTimelineAgentAction agentAction in this.AgentActions)
            {
                foreach (Transform agent in Agents)
                {
                    if (agent != null)
                    {
                        agentAction.Initialize(agent.gameObject);
                    }
                }

                if (m_UseInterval)
                {
                    foreach (Transform agent in SequenceAgents)
                    {
                        if (agent != null)
                        {
                            agentAction.Initialize(agent.gameObject);
                        }
                    }
                }
            }
        }

        private void _doUpdateTrack(float previousTime, float runningTime, float deltaTime)
        {
            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineAgentEvent)
                {
                    CTimelineAgentEvent agentEvent = item as CTimelineAgentEvent;
                    if ((previousTime < agentEvent.FireTime) && (elapsedTime >= agentEvent.FireTime))
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {
                                TraceInfo(string.Format("[{0}] Trigger Event: {1}, {2}", runningTime, agent.name, agentEvent.name));
                                agentEvent.Trigger(agent.gameObject);                                
                            }
                        }
                    }
                    else if ((previousTime >= agentEvent.FireTime) && (elapsedTime < agentEvent.FireTime))
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {
                                TraceInfo(string.Format("[{0}] Trigger Event: {1}, {2}", runningTime, agent.name, agentEvent.name));
                                agentEvent.Reverse(agent.gameObject);
                            }
                        }
                    }
                }
            }

            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineAgentAction)
                {
                    CTimelineAgentAction agentAction = item as CTimelineAgentAction;
                    if ((previousTime < agentAction.FireTime) && (elapsedTime >= agentAction.FireTime) && elapsedTime < agentAction.EndTime)
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {
                                TraceInfo(string.Format("[{0}] Trigger Action: {1}, {2}", runningTime, agent.name, agentAction.name));
                                agentAction.Trigger(agent.gameObject);
                            }
                        }
                    }
                    else if ((previousTime <= agentAction.EndTime) && (elapsedTime > agentAction.EndTime))
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {
                                TraceInfo(string.Format("[{0}] Action End: {1}, {2}", runningTime, agent.name, agentAction.name));
                                agentAction.End(agent.gameObject);
                            }
                        }
                    }
                    else if ((previousTime >= agentAction.FireTime) && (previousTime < agentAction.EndTime) && (elapsedTime < agentAction.FireTime))
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {
                                agentAction.ReverseTrigger(agent.gameObject);
                            }
                        }
                    }
                    else if ((previousTime > agentAction.EndTime) && (elapsedTime > agentAction.FireTime) && (elapsedTime <= agentAction.EndTime))
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {                              
                                agentAction.ReverseEnd(agent.gameObject);
                            }
                        }
                    }
                    else if (base.elapsedTime > agentAction.FireTime && base.elapsedTime <= agentAction.EndTime)
                    {
                        float t = runningTime - agentAction.FireTime;
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                            {
                                agentAction.UpdateTime(agent.gameObject, t, deltaTime);
                            }
                        }
                    }
                }
            }
        }

        private void _doUpdateSequenceTrack(float previousTime, float runningTime, float deltaTime)
        {
            for (int i = 0; i < SequenceAgents.Count; i++)
            {
                foreach (CTimelineItem item in GetTimelineItems())
                {
                    if (item is CTimelineAgentEvent)
                    {
                        CTimelineAgentEvent agentEvent = item as CTimelineAgentEvent;
                        float fireTime = agentEvent.FireTime + m_IntervalTime * i;
                        if ((previousTime < fireTime) && (elapsedTime >= fireTime))
                        {
                            if (SequenceAgents[i] != null)
                                agentEvent.Trigger(SequenceAgents[i].gameObject);
                        }
                        else if ((previousTime >= fireTime) && (elapsedTime < fireTime))
                        {
                            if (SequenceAgents[i] != null)
                                agentEvent.Reverse(SequenceAgents[i].gameObject);
                        }
                    }
                }

                foreach (CTimelineItem item in GetTimelineItems())
                {
                    if (item is CTimelineAgentAction)
                    {
                        CTimelineAgentAction agentAction = item as CTimelineAgentAction;
                        float fireTime = agentAction.FireTime + m_IntervalTime * i;
                        float endTime = agentAction.EndTime + m_IntervalTime * i;                  
                        if ((previousTime < fireTime) && (elapsedTime >= fireTime) && (elapsedTime < endTime))
                        {
                            if (SequenceAgents[i] != null)
                            {
                                agentAction.Trigger(SequenceAgents[i].gameObject);
                                //Debug.Log("[" + fireTime.ToString() + "] " + i.ToString() + ", Trigger");
                            }
                        }
                        else if ((previousTime <= endTime) && (elapsedTime > endTime))
                        {
                            if (SequenceAgents[i] != null)
                            {
                                agentAction.End(SequenceAgents[i].gameObject);
                                //Debug.Log("[" + endTime.ToString() + "] " + i.ToString() + ", End");
                            }
                        }
                        else if ((previousTime >= fireTime) && (previousTime < endTime) && (elapsedTime < fireTime))
                        {
                            if (SequenceAgents[i] != null)
                                agentAction.ReverseTrigger(SequenceAgents[i].gameObject);
                        }
                        else if ((previousTime > endTime) && (elapsedTime > fireTime) && (elapsedTime <= endTime))
                        {
                            if (SequenceAgents[i] != null)
                                agentAction.ReverseEnd(SequenceAgents[i].gameObject);
                        }
                        else if (base.elapsedTime > fireTime && base.elapsedTime <= endTime)
                        {
                            float t = runningTime - fireTime;
                            if (SequenceAgents[i] != null)
                                agentAction.UpdateTime(SequenceAgents[i].gameObject, t, deltaTime);
                        }
                    }
                }
            }
        }

        public override void UpdateTrack(float runningTime, float deltaTime)
        {
            float previousTime = elapsedTime;
            base.UpdateTrack(runningTime, deltaTime);

            if (m_UseInterval)
            {
                _doUpdateSequenceTrack(previousTime, runningTime, deltaTime);
            }
            else
            {
                _doUpdateTrack(previousTime, runningTime, deltaTime);
            }
        }

        public override void Stop()
        {
            base.Stop();
            //base.elapsedTime = -1f;
            foreach (CTimelineItem item in GetTimelineItems())
            {
                CTimelineAgentEvent agentEvent = item as CTimelineAgentEvent;
                if (agentEvent != null)
                {
                    foreach (Transform agent in Agents)
                    {
                        if (agent != null)
                            agentEvent.Stop(agent.gameObject);
                    }

                    if (m_UseInterval)
                    {
                        foreach (Transform agent in SequenceAgents)
                        {
                            if (agent != null)
                            {
                                agentEvent.Stop(agent.gameObject);
                            }
                        }
                    }
                }

                CTimelineAgentAction agentAction = item as CTimelineAgentAction;
                if (agentAction != null)
                {
                    foreach (Transform agent in Agents)
                    {
                        if (agent != null)
                            agentAction.Stop(agent.gameObject);
                    }

                    if (m_UseInterval)
                    {
                        foreach (Transform agent in SequenceAgents)
                        {
                            if (agent != null)
                            {
                                agentAction.Stop(agent.gameObject);
                            }
                        }
                    }
                }
            }
        }

        private void _doSetTime(float previousTime, float time)
        {
            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineAgentEvent)
                {
                    // check if it is a agent event
                    CTimelineAgentEvent agentEvent = item as CTimelineAgentEvent;
                    if (agentEvent != null)
                    {
                        if ((previousTime < agentEvent.FireTime) && (elapsedTime >= agentEvent.FireTime))
                        {
                            foreach (Transform agent in Agents)
                            {
                                if (agent != null)
                                    agentEvent.Trigger(agent.gameObject);
                            }
                        }
                        else if ((previousTime >= agentEvent.FireTime) && (elapsedTime < agentEvent.FireTime))
                        {
                            foreach (Transform agent in Agents)
                            {
                                if (agent != null)
                                    agentEvent.Reverse(agent.gameObject);
                            }
                        }
                    }
                }//
            }

            foreach (CTimelineItem item in GetTimelineItems())
            {
                if (item is CTimelineAgentAction)
                {
                    // check if it is a agent action
                    CTimelineAgentAction agentAction = item as CTimelineAgentAction;
                    if (agentAction != null)
                    {
                        foreach (Transform agent in Agents)
                        {
                            if (agent != null)
                                agentAction.SetTime(agent.gameObject, (time - agentAction.FireTime), time - previousTime);
                        }
                    }
                }
            }
        }

        private void _doSetSequenceTime(float previousTime, float time)
        {
            if (SequenceAgents == null)
                return;

            for (int i = 0; i < SequenceAgents.Count; i++)
            {
                foreach (CTimelineItem item in GetTimelineItems())
                {
                    // check if it is a agent event
                    CTimelineAgentEvent agentEvent = item as CTimelineAgentEvent;
                    if (agentEvent != null)
                    {
                        float fireTime = agentEvent.FireTime + m_IntervalTime * i;
                        if ((previousTime < fireTime) && (elapsedTime >= fireTime))
                        {
                            if (SequenceAgents[i] != null)
                                agentEvent.Trigger(SequenceAgents[i].gameObject);
                        }
                        else if ((previousTime >= fireTime) && (elapsedTime < fireTime))
                        {
                            if (SequenceAgents[i] != null)
                                agentEvent.Reverse(SequenceAgents[i].gameObject);
                        }
                    }

                    // check if it is a agent action
                    CTimelineAgentAction agentAction = item as CTimelineAgentAction;
                    if (agentAction != null)
                    {
                        float fireTime = agentAction.FireTime + m_IntervalTime * i;
                        if (SequenceAgents[i] != null)
                            agentAction.SetTime(SequenceAgents[i].gameObject, (time - fireTime), time - previousTime);
                    }
                }
            }
        }
   
        public override void SetTime(float time)
        {
            float previousTime = elapsedTime;
            base.SetTime(time);

            if (m_UseInterval)
            {
                _doSetSequenceTime(previousTime, time);
            }
            else
            {
                _doSetTime(previousTime, time);
            }
        }

        public override List<float> GetMilestone(float from, float to)
        {
            bool isReverse = from > to;

            List<float> times = new List<float>();
            if (m_UseInterval)
            {
                if (SequenceAgents == null)
                    return times;

                for (int i = 0; i < SequenceAgents.Count; i++)
                {
                    foreach (CTimelineItem item in GetTimelineItems())
                    {
                        float fireTime = item.FireTime + m_IntervalTime * i;
                        if ((!isReverse && from < fireTime && to >= fireTime) || (isReverse && from > fireTime && to <= fireTime))
                        {
                            if (!times.Contains(fireTime))
                            {
                                times.Add(fireTime);
                            }
                        }

                        if (item is CTimelineAction)
                        {
                            float endTime = (item as CTimelineAction).EndTime + m_IntervalTime * i;
                            if ((!isReverse && from < endTime && to >= endTime) || (isReverse && from > endTime && to <= endTime))
                            {
                                if (!times.Contains(endTime))
                                {
                                    times.Add(endTime);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (CTimelineItem item in GetTimelineItems())
                {
                    if ((!isReverse && from < item.FireTime && to >= item.FireTime) || (isReverse && from > item.FireTime && to <= item.FireTime))
                    {
                        if (!times.Contains(item.FireTime))
                        {
                            times.Add(item.FireTime);
                        }
                    }

                    if (item is CTimelineAction)
                    {
                        float endTime = (item as CTimelineAction).EndTime;
                        if ((!isReverse && from < endTime && to >= endTime) || (isReverse && from > endTime && to <= endTime))
                        {
                            if (!times.Contains(endTime))
                            {
                                times.Add(endTime);
                            }
                        }
                    }
                }
            }
            
            times.Sort();
            return times;
        }
    }

}//namespace UIDirector