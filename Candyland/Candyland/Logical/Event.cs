﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Candyland
{
    /// <summary>
    /// Basic Class from which all GameObjects, UI-Elements and the Camera are derived
    /// </summary>
    public class Event
    {
        private GameObject m_triggerable;

        private SwitchGroup m_switchGroup;

        private bool m_triggered;
        
        public Event( string triggerableID, List<String> switchIDs, Dictionary<string,GameObject> objects )
        {
            //string triggerableID = "0.1.0.obstacle";
            m_triggerable = objects[triggerableID];
            m_switchGroup = new SwitchGroup(switchIDs, objects, this);
        }

        public void Trigger()
        {
            m_triggered = true;
            m_triggerable.isdestroyed = true;
        }

        public void Reset()
        {
            m_switchGroup.Reset();
            m_triggered = false;
        }
    }
}