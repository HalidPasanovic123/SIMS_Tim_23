// File:    ActionRepository.cs
// Author:  halid
// Created: Sunday, 24 April 2022 22:59:45
// Purpose: Definition of Class ActionRepository

using System;
using System.Collections.Generic;
using ZdravoCorp.Utility;
using System.Linq;

namespace Repository
{
    //Sequencial Data Base
    public class ActionRepository
    {
        private String dbPath = "..\\..\\Data\\actionsDB.csv";
        private Serializer<Model.Action> serializerAction = new Serializer<Model.Action>();
        private HashSet<int> idMap;

        private static ActionRepository instance = null;

        private static readonly object key = new object();

        public ActionRepository()
        {
            idMap = new HashSet<int>();
            List<Model.Action> actions = serializerAction.FromCSV(dbPath);

            foreach (Model.Action action in actions)
            {
                idMap.Add(action.Id);
            }
        }

        public Boolean CreateAction(Model.Action newAction)
        {
            lock (key)
            {
                //Generate ID for action
                //If ID already exists generate another ID
                Random random = new Random();
                do
                {
                    newAction.Id = random.Next();
                }
                while (idMap.Contains(newAction.Id));

                //Bool for checking if it was added somewhere in the list
                bool added = false;
                List<Model.Action> actions = serializerAction.FromCSV(dbPath);

                for (int i = 0; i < actions.Count; i++)
                {
                    if(DateManipulator.checkIfLaterDate(actions[i].ExecutionDate, newAction.ExecutionDate))
                    {
                        actions.Insert(i, newAction);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    actions.Add(newAction);
                }

                serializerAction.ToCSV(dbPath, actions);

                //Add action to Hashmap for later checks
                idMap.Add(newAction.Id);

                return true;
            }
        }

        private void SortActions(List<Model.Action> actions)
        {
            lock (key)
            {
                serializerAction.ToCSV(dbPath, actions.OrderBy(a => a.ExecutionDate).ToList());
            }
        }

        public Boolean UpdateAction(Model.Action action)
        {
            lock (key)
            {
                List<Model.Action> actions = serializerAction.FromCSV(dbPath);

                bool found = false;
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i].Id == action.Id)
                    {
                        actions[i] = action;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }

                SortActions(actions);

                return true;
            }
        }

        public Boolean DeleteAction(int identificator)
        {
            lock (key)
            {
                List<Model.Action> actions = serializerAction.FromCSV(dbPath);

                bool found = false;
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i].Id == identificator)
                    {
                        actions.RemoveAt(i);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }

                serializerAction.ToCSV(dbPath, actions);

                //Remove action to Hashmap for later checks
                idMap.Remove(identificator);

                return true;
            }
        }

        public void SaveActions(List<Model.Action> actions)
        {
            lock (key)
            {
                serializerAction.ToCSV(dbPath, actions);
            }
        }

        public Model.Action ReadAction(int identificator)
        {
            lock (key)
            {
                if (!idMap.Contains(identificator))
                {
                    return null;
                }
                List<Model.Action> actions = serializerAction.FromCSV(dbPath);

                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i].Id == identificator)
                    {
                        return actions[i];
                    }
                }

                return null;
            }
        }

        public List<Model.Action> GetAllActions()
        {
            lock (key)
            {
                return serializerAction.FromCSV(dbPath);
            }
        }

        public static ActionRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (key)
                    {
                        if (instance == null)
                        {
                            instance = new ActionRepository();
                        }
                    }
                }
                return instance;
            }
        }

    }
}