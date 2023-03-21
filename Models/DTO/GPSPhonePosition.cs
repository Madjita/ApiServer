using System;
using Newtonsoft.Json;

namespace Models.DTO
{
    public class GPSPhonePosition
    {
        public double Longitude {get;set;}
        public double Latitude {get;set;}
        public long Timestamp {get;set;} //millisecondsSinceEpoch
        public double Accuracy {get;set;}
        public double Altitude {get;set;}
        public double Heading {get;set;}
        public double Speed {get;set;}
        public double SpeedAccuracy {get;set;}
        public int? Floor {get;set;}
        public bool isMocked  {get;set;} = false;

        public string toJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
