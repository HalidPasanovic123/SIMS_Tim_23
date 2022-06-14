﻿using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Service.Interfaces
{
    public interface IHospitalSurveyService : ICrud<HospitalSurvey>
    {

        List<int> getAllHospitalSurveyIds();

        string GetResults(DateTime start, DateTime end);
    }
}
