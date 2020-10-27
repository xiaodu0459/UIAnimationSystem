// ========================================================================
// UGUI Animation
// Created: 2016/10/14

// Brief: The basic building unit of a timeline, maintains a firetime
// ========================================================================
using UnityEngine;
using System.Collections;

namespace UIDirector
{

    public abstract class CTimelineItem : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        protected float fireTime = 0f;

        [SerializeField]
        protected bool revertWhenFinish = false;
        #endregion //Fields

        #region Properties
        public float FireTime
        {
            get { return this.fireTime; }
            set
            {
                fireTime = value;
                if (fireTime < 0f)
                {
                    fireTime = 0f;
                }
            }
        }

        public CTimelineTrack TimelineTrack
        {
            get
            {
                CTimelineTrack track = null;
                if (transform.parent != null)
                {
                    track = base.transform.parent.GetComponentInParent<CTimelineTrack>();
                    if (track == null)
                    {
                        Debug.LogError("No TimelineTrack found on parent!", this);
                    }
                }
                else
                {
                    Debug.LogError("Timeline Item has no parent!", this);
                }
                return track;
            }
        }
        #endregion //Properties

        #region Virtual functions
        public virtual void Initialize() { }

        public virtual void Stop() { }

        public virtual void SetDefaults() { }
        #endregion //Virtual functions
    }

}//namespace UIDirector