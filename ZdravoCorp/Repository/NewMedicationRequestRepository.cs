﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class NewMedicationRequestRepository : Repository<NewMedicationRequest>
    {
        private static NewMedicationRequestRepository instance = null;
        public NewMedicationRequestRepository()
        {
            dbPath = "..\\..\\Data\\newMedicationRequestDB.csv";
            InstantiateIDSet(GetAll());
        }

        public Boolean AcceptNewMedicationRequest(NewMedicationRequest newMedicationRequest)
        {
            List<NewMedicationRequest> requests = GetAll();
            foreach (NewMedicationRequest request in requests)
            {
                if (request.Id == newMedicationRequest.Id)
                {
                    Controller.MedicationController medicationController = new Controller.MedicationController();
                    medicationController.CreateMedicationType(newMedicationRequest.MedicationType);
                    requests.Remove(request);
                    SaveChanges(requests);
                    return true;
                }
            }
            return false;
        }

        public Boolean RejectNewMedicationRequest(NewMedicationRequest newMedicationRequest, String comment)
        {
            newMedicationRequest.Status = Status.REJECTED;
            newMedicationRequest.Comment = comment;
            Update(newMedicationRequest);
            return true;
        }

        public override NewMedicationRequest Read(int id)
        {
            lock (key)
            {
                CheckIfIDExists(id);
                return FindRequestByID(GetAll(), id);
            }
        }

        public override void Create(NewMedicationRequest element)
        {
            lock (key)
            {
                element.Id = GenerateID();
                AppendToDB(element);
                idMap.Add(element.Id);
            }
        }

        public override void Update(NewMedicationRequest element)
        {
            lock (key)
            {
                CheckIfIDExists(element.Id);
                List<NewMedicationRequest> elements = GetAll();
                SwapRequestByID(elements, element);
                SaveChanges(elements);
            }
        }

        public override void Delete(int id)
        {
            lock (key)
            {
                CheckIfIDExists(id);
                List<NewMedicationRequest> elements = GetAll();
                DeleteRequestByID(elements, id);
                SaveChanges(elements);
            }
        }

        public new List<NewMedicationRequest> GetAll()
        {
            List<NewMedicationRequest> requests = base.GetAll();
            Dictionary<int, MedicationType> types = MedicationTypeRepository.Instance.GetAll()
                .ToDictionary(keySelector: m => m.Id, elementSelector: m => m);
            foreach (NewMedicationRequest request in requests)
            {
                for (int i = 0; i < request.MedicationType.Replacement.Count; i++)
                {
                    if (types.ContainsKey(request.MedicationType.Replacement[i].Id))
                    {
                        request.MedicationType.Replacement[i] = types[request.MedicationType.Replacement[i].Id];
                    }
                }
            }
            return requests;
        }

        protected override void InstantiateIDSet(List<NewMedicationRequest> elements)
        {
            lock (key)
            {
                foreach (NewMedicationRequest element in elements)
                {
                    idMap.Add(element.Id);
                }
            }
        }

        private void CheckIfIDExists(int id)
        {
            if (!idMap.Contains(id))
                throw new Exception("MedicalRecord doesnt exist");
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

        private void DeleteRequestByID(List<NewMedicationRequest> requests, int id)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Id == id)
                {
                    requests.RemoveAt(i);
                    idMap.Remove(id);
                    break;
                }
            }
        }

        private NewMedicationRequest FindRequestByID(List<NewMedicationRequest> elements, int id)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Id == id)
                {
                    return elements[i];
                }
            }
            throw new Exception("MedicalRecord doesnt exist");
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
