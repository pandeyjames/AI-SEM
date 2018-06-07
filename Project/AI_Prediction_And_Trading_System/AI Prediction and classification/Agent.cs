using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Prediction_and_classification
{
    class Agent 
    {
        double EnergyConsumption;

        double tempstorage;
        bool license;
        double capital = 6000;
        double buyEnergy;
        double sellEnergy;
        double hourTime = 24;

       public  double calculateConsumption()
        {
            return EnergyConsumption;
        }

    }
}
