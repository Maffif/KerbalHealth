﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalHealth
{
    public class KerbalHealthList : List<KerbalHealthStatus>
    {
        public void Add(string name, double health = double.NaN)
        {
            if (Contains(name))
            {
                Core.Log(name + " already registered.");
                return;
            } else Core.Log("Registering " + name + " with " + health + " health.", Core.LogLevel.Important);
            KerbalHealthStatus khs;
            if (double.IsNaN(health)) khs = new KerbalHealth.KerbalHealthStatus(name);
            else khs = new KerbalHealth.KerbalHealthStatus(name, health);
            Add(khs);
        }

        public void RegisterKerbals()
        {
            Core.Log("Registering kerbals...");
            KerbalRoster kerbalRoster = HighLogic.fetch.currentGame.CrewRoster;
            Core.Log("" + kerbalRoster.Count + " kerbals in CrewRoster: " + HighLogic.fetch.currentGame.CrewRoster.Crew.Count() + " crew, " + HighLogic.fetch.currentGame.CrewRoster.Tourist.Count() + " tourists.", Core.LogLevel.Important);
            foreach (ProtoCrewMember pcm in kerbalRoster.Crew.Concat(kerbalRoster.Tourist))
                if (IsKerbalTrackable(pcm)) Add(pcm.name, KerbalHealthStatus.GetMaxHP(pcm));
                else Core.Log(pcm?.name + " is not trackable (" + (pcm == null ? "is null" : ("status: " + pcm.rosterStatus)) + ").");
            Core.Log("" + Count + " kerbal(s) processed.", Core.LogLevel.Important);
        }

        public void Update(double interval)
        {
            for (int i = 0; i < Count; i++)
            {
                KerbalHealthStatus khs = this[i];
                ProtoCrewMember pcm = khs.PCM;
                if (IsKerbalTrackable(pcm)) khs.Update(interval);
                else
                {
                    if (pcm != null) Core.Log(khs.Name + " (" + pcm.type + ", " + pcm.rosterStatus + ") is not trackable anymore. Removing.");
                    else Core.Log(khs.Name + " is not trackable anymore. Removing.");
                    RemoveAt(i);
                    i--;
                }
            }
            //if (HighLogic.fetch.currentGame.CrewRoster.GetAssignedCrewCount() + HighLogic.fetch.currentGame.CrewRoster.GetAvailableCrewCount() != Count) RegisterKerbals();
            if (HighLogic.fetch.currentGame.CrewRoster.Crew.Count() + HighLogic.fetch.currentGame.CrewRoster.Tourist.Count() != Count) RegisterKerbals();
        }

        public void ProcessEvents()
        {
            foreach (KerbalHealthStatus khs in this)
            {
                Core.Log("Processing " + Core.Events.Count + " potential events for " + khs.Name + "...", Core.LogLevels.Important);
                foreach (Event e in Core.Events) e.Process(khs);
            }
        }

        bool IsKerbalTrackable(ProtoCrewMember pcm)
        {
            return (pcm != null) && ((pcm.rosterStatus == ProtoCrewMember.RosterStatus.Assigned) || (pcm.rosterStatus == ProtoCrewMember.RosterStatus.Available));
        }

        public KerbalHealthStatus Find(ProtoCrewMember pcm)
        {
            foreach (KerbalHealthStatus khs in this) if (khs.Name == pcm.name) return khs;
            return null;
        }

        public bool Contains(string name)
        {
            foreach (KerbalHealthStatus khs in this) if (khs.Name == name) return true;
            return false;
        }
    }
}
