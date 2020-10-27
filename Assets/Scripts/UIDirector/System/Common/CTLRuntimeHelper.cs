using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UIDirector
{

    public static class CTLRuntimeHelper
    {
        private static System.Type[] Registerd_TrackTypes = new System.Type[]
        {
            typeof(CAgentItemTrack),
            typeof(CGlobalItemTrack)
        };

        private static System.Type[] Registerd_ItemTypes = new System.Type[]
        {
            // Event
            typeof(CUIEnableBehaviorEvent),
            typeof(CUIEnableGameObjectEvent),
            typeof(CUISetLayoutGroupSpacingEvent),
            typeof(CUIWidgetAlphaEvent),
            typeof(CUIWidgetPositionEvent),
            typeof(CUIWidgetRotationEvent),
            typeof(CUIWidgetScaleEvent),

            // Action
            typeof(CUILayoutGroupSpacingAction),
            typeof(CUIWidgetFadeAction),
            typeof(CUIWidgetMoveAction),
            typeof(CUIWidgetRotateAction),
            typeof(CUIWidgetZoomAction)
        };

        public static List<Type> GetAllowedTrackTypes(CTimelineTrackGroup trackGroup)
        {
            UnityEngine.Profiling.Profiler.BeginSample("GetAllowedTrackTypes.GetAttributes");
            CTimelineEnum.ETrackType[] types = new CTimelineEnum.ETrackType[0];

            object[] attributes = trackGroup.GetType().GetCustomAttributes(typeof(CTimelineTrackGroupAttribute), true);
            for (int i = 0; i < attributes.Length; i++)
            {
                CTimelineTrackGroupAttribute attribute = attributes[i] as CTimelineTrackGroupAttribute;
                if (attribute != null)
                {
                    types = attribute.AllowedTrackType;
                    break;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("GetAllowedTrackTypes");
            List<Type> allowedTrackTypes = new List<Type>();
            Type[] allSubTypes = _getAllSubTypes(typeof(CTimelineTrack));
            for (int i = 0; i < allSubTypes.Length; i++)
            {
                Type type = allSubTypes[i];

                object[] attributes1 = type.GetCustomAttributes(typeof(CTimelineTrackAttribute), true);
                for (int j = 0; j < attributes1.Length; j++)
                {
                    CTimelineTrackAttribute attribute = attributes1[j] as CTimelineTrackAttribute;
                    if (attribute == null) continue;

                    for (int x = 0; x < attribute.TrackTypes.Length; x++)
                    {
                        CTimelineEnum.ETrackType trackType = attribute.TrackTypes[x];
                        for (int y = 0; y < types.Length; y++)
                        {
                            CTimelineEnum.ETrackType trackType2 = types[y];
                            if (trackType == trackType2)
                            {
                                allowedTrackTypes.Add(type);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();
            return allowedTrackTypes;
        }

        public static List<Type> GetAllowedItemTypes(CTimelineTrack track)
        {
            UnityEngine.Profiling.Profiler.BeginSample("GetAllowedItemTypes.GetAttributes");
            CTimelineEnum.EItemType[] types = new CTimelineEnum.EItemType[0];

            object[] attributes = track.GetType().GetCustomAttributes(typeof(CTimelineTrackAttribute), true);
            for (int i = 0; i < attributes.Length; i++)
            {
                CTimelineTrackAttribute attribute = attributes[i] as CTimelineTrackAttribute;
                if (attribute != null)
                {
                    types = attribute.AllowedItemTypes;
                    break;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("GetAllowedItemTypes");
            List<Type> allowedItemTypes = new List<Type>();
            Type[] allSubTypes = _getAllSubTypes(typeof(CTimelineItem));
            for (int i = 0; i < allSubTypes.Length; i++)
            {
                Type type = allSubTypes[i];

                object[] attributes1 = type.GetCustomAttributes(typeof(CTimelineItemAttribute), true);
                for (int j = 0; j < attributes1.Length; j++)
                {
                    CTimelineItemAttribute attribute = attributes1[j] as CTimelineItemAttribute;
                    if (attribute == null) continue;

                    for (int x = 0; x < attribute.ItemTypes.Length; x++)
                    {
                        CTimelineEnum.EItemType itemType = attribute.ItemTypes[x];
                        for (int y = 0; y < types.Length; y++)
                        {
                            CTimelineEnum.EItemType itemType2 = types[y];
                            if (itemType == itemType2)
                            {
                                allowedItemTypes.Add(type);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();

            return allowedItemTypes;
        }

        private static Type[] _getAllSubTypes(System.Type parentType)
        {
            if (parentType == typeof(CTimelineTrack))
            {
                return Registerd_TrackTypes;
            }
            else if (parentType == typeof(CTimelineItem))
            {
                return Registerd_ItemTypes;
            }

            UnityEngine.Profiling.Profiler.BeginSample("_getAllSubTypes_new_list");
            List<System.Type> list = new List<System.Type>();
            UnityEngine.Profiling.Profiler.EndSample();
            return list.ToArray();
        }
    }

}//namespace UIDirector