// ========================================================================
// UGUI Animation
// Created: 2016/11/07

// Brief: Called when running time hits the firetime of the action
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public abstract class CTimelineAgentAction : CTimelineAction
    {
        #region Fields
        protected bool m_bInitialized = false;
        #endregion Fields

        #region abstract functions
        public abstract void Trigger(GameObject agent);

        public abstract void End(GameObject agent);
        #endregion //abstract functions

        #region virtual functions
        public virtual void Initialize(GameObject agent) { m_bInitialized = true; }

        public virtual void UpdateTime(GameObject agent, float time, float deltaTime) { }

        public virtual void SetTime(GameObject agent, float time, float deltaTime) { }

        public virtual void Pause(GameObject agent) { }

        public virtual void Resume(GameObject agent) { }

        public virtual void Stop(GameObject agent) { }

        public virtual void ReverseTrigger(GameObject agent) { }

        public virtual void ReverseEnd(GameObject agent) { }
        #endregion //virtual functions

        #region Public functions
        public int CompareTo(object other)
        {
            CTimelineGlobalAction otherAction = (CTimelineGlobalAction)other;
            return (int)(otherAction.FireTime - this.FireTime);
        }
        #endregion //Public functions
    }

}//namespace UIDirector