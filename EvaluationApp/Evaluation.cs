using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace EvaluationApp
{
    class Evaluation
    {

        public string evalID { get; set; }
        public string driverName { get; set; }
        public string vehicleName { get; set; }
        public string evalType { get; set; }

        public Evaluation(string id, string dName, string vName, string eType)
        {
            evalID = id;
            driverName = dName;
            vehicleName = vName;
            evalType = eType;
        }
    }
}
