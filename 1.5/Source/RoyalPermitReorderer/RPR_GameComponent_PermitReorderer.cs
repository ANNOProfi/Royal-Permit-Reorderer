using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using UnityEngine;
using System;
using RimWorld;
using Verse;
using System.Configuration;

namespace RoyalPermitReorderer
{
    public class RPR_GameComponent_PermitReorderer : GameComponent
    {
        public int permitsPerRow;

        public bool horizontal;

        public List<RoyalTitlePermitDef> permitDefs = new List<RoyalTitlePermitDef>();

        public List<RoyalTitlePermitDef> permitDefsTemp = new List<RoyalTitlePermitDef>();

        public Dictionary<RoyalTitlePermitDef, bool> permitDefsCheck = new Dictionary<RoyalTitlePermitDef, bool>();

        public HashSet<Type> permitWorkers = new HashSet<Type>();
        
        public RPR_GameComponent_PermitReorderer(Game game)
        {
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();

            PrepareReordering();
        }

        public override void LoadedGame()
        {
            base.LoadedGame();

            PrepareReordering();
        }

        public void PrepareReordering()
        {
            permitDefs = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;

            permitDefsTemp = permitDefs.ToList();

            permitsPerRow = RoyalPermitReordererMod.settings.permitsPerRow;

            horizontal = RoyalPermitReordererMod.settings.horizontal;

            PopulateLists();

            ReorderPermits();
        }

        public void PopulateLists()
        {
            foreach(RoyalTitlePermitDef permit in permitDefsTemp)
            {
                if(permit.minTitle == null)
                {
                    permitDefs.Remove(permit);

                    continue;
                }

                if(!permitWorkers.Contains(permit.workerClass))
                {
                    permitWorkers.Add(permit.workerClass);
                }

                if(permit.prerequisite != null)
                {
                    permit.prerequisite.GetModExtension<RPR_PermitOrder>().descendant = permit;
                }

                permitDefsCheck.Add(permit, false);
            }
        }

        public void ReorderPermits()
        {
            int y = 0;

            int x = 0;

            foreach(Type worker in permitWorkers)
            {
                foreach(RoyalTitlePermitDef permit in permitDefs)
                {
                    if(permit.workerClass != worker || permitDefsCheck[permit] == true)
                    {
                        continue;
                    }

                    if(permit.prerequisite == null)
                    {
                        if(horizontal)
                        {
                            permit.uiPosition = new UnityEngine.Vector2(y,x);
                        }
                        else
                        {
                            permit.uiPosition = new UnityEngine.Vector2(x,y);
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if(permit.GetModExtension<RPR_PermitOrder>().descendant != null)
                    {
                        x = CheckDescendants(permit.GetModExtension<RPR_PermitOrder>().descendant, x);
                    }

                    permitDefsCheck[permit] = true;

                    if(x >= permitsPerRow-1)
                    {
                        y++;
                        x = 0;
                    }
                    else
                    {
                        x++;
                    }
                }

                if(x > 0)
                {
                    y++;
                    x = 0;
                }
            }

            permitDefsCheck.Clear();
        }

        public int CheckDescendants(RoyalTitlePermitDef permit, int x)
        {
            if(permit.prerequisite != null)
            {
                if(horizontal)
                {
                    permit.uiPosition = permit.prerequisite.uiPosition + new UnityEngine.Vector2(0, 1);
                }
                else
                {
                    permit.uiPosition = permit.prerequisite.uiPosition + new UnityEngine.Vector2(1, 0);
                }
            }

            if(permit.prerequisite != null)
            {
                permitDefsCheck[permit] = true;

                x = (int)permit.uiPosition.x;
            }

            if(permit.GetModExtension<RPR_PermitOrder>().descendant != null)
            {
                x = CheckDescendants(permit.GetModExtension<RPR_PermitOrder>().descendant, x);
            }

            return x;
        }
    }
}