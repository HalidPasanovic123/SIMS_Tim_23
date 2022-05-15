﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class NewMedicationRequestRepository
    {
        private static NewMedicationRequestRepository instance = null;

        private String dbPath = "..\\..\\Data\\newMedicationRequestDB.csv";
        private Serializer<NewMedicationRequest> serializerNewMedicationRequest = new Serializer<NewMedicationRequest>();

        private HashSet<int> idMap = new HashSet<int>();

        public NewMedicationRequestRepository()
        {
            List<NewMedicationRequest> requests = GetAllNewMedicationRequests();
            InstantiateHashSets(requests);
        }

        private int GenerateID()
        {
            int id;
            Random random = new Random();
            do
            {
                id = random.Next();
            }
            while (idMap.Contains(id));
            idMap.Add(id);
            return id;
        }

        private void InstantiateHashSets(List<NewMedicationRequest> requests)
        {
            foreach (NewMedicationRequest request in requests)
            {
                idMap.Add(request.Id);
            }
        }

        private bool CheckIfIDExists(int id)
        {
            return idMap.Contains(id);
        }

        public Boolean CreateNewMedicationRequest(NewMedicationRequest newMedicationRequest)
        {
            newMedicationRequest.Id = GenerateID();
            serializerNewMedicationRequest.ToCSVAppend(dbPath,new List<NewMedicationRequest>() { newMedicationRequest });
            return true;
        }

        public NewMedicationRequest ReadNewMedicationRequest(int id)
        {
            List<NewMedicationRequest> requests= GetAllNewMedicationRequests();
            foreach (NewMedicationRequest temp in requests)
            {
                if (id == temp.Id)
                {
                    return temp;
                }
            }
            return null;
        }

        private void SwapRequestByID(List<NewMedicationRequest> requests, NewMedicationRequest request)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Id == request.Id)
                {
                    requests[i] = request;
                    break;
                }
            }
        }

        public Boolean UpdateNewMedicationRequest(NewMedicationRequest newMedicationRequest)
        {
            if (CheckIfIDExists(newMedicationRequest.Id))
            {
                List<NewMedicationRequest> requests = GetAllNewMedicationRequests();
                SwapRequestByID(requests, newMedicationRequest);
                serializerNewMedicationRequest.ToCSV(dbPath, requests);
                return true;
            }
            else
            {
                return false;
            }

        }

        private void DeleteRequestByID(List<NewMedicationRequest> requests, int id)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Id == id)
                {
                    requests.RemoveAt(i);
                    break;
                }
            }
        }

        public Boolean DeleteNewMedicationRequest(int id)
        {
            if (CheckIfIDExists(id))
            {
                List<NewMedicationRequest> requests = GetAllNewMedicationRequests();
                DeleteRequestByID(requests, id);
                serializerNewMedicationRequest.ToCSV(dbPath, requests);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<NewMedicationRequest> GetAllNewMedicationRequests()
        {
            List<NewMedicationRequest> requests = serializerNewMedicationRequest.FromCSV(dbPath);
            Dictionary<int, MedicationType> types = MedicineRepository.Instance.GetAllMedicationType().ToDictionary(keySelector: m => m.Id, elementSelector: m => m);
            foreach(NewMedicationRequest request in requests)
            {
                if (types.ContainsKey(request.MedicationType.Id))
                {
                    request.MedicationType = types[request.MedicationType.Id];
                }
            }
            return requests;
        }

        public static NewMedicationRequestRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NewMedicationRequestRepository();
                }
                return instance;
            }
        }
    }
}