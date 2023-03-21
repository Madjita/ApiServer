using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.DTO;

namespace Database.Contexts.Models
{
    public class GPSPosition : BaseEntity
    {
        public GPSPosition()
        {
            
        }

        public GPSPosition(GPSPhonePosition gpsPhonePosition, StaffMobile user)
        {
            this.Longitude = gpsPhonePosition.Longitude;
            this.Latitude = gpsPhonePosition.Latitude;
            this.Altitude = gpsPhonePosition.Altitude;
            this.Heading = gpsPhonePosition.Heading;
            this.Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(gpsPhonePosition.Timestamp).DateTime;
            this.Accuracy = gpsPhonePosition.Accuracy;
            this.SpeedAccuracy = gpsPhonePosition.SpeedAccuracy;
            this.Speed = gpsPhonePosition.Speed;
            this.Floor = gpsPhonePosition.Floor;
            this.isMocked = gpsPhonePosition.isMocked;

            if(user is not null)
            {
                this.StaffMobileId = user.Id;
            }
        }
        
        [DataType(DataType.Date)]
        public DateTime Timestamp {get;set;}
        public double Longitude {get;set;}
        public double Latitude {get;set;}
        public double Altitude {get;set;}
        public double Heading {get;set;}
        public double Speed {get;set;}
        public double SpeedAccuracy {get;set;}
        public double Accuracy {get;set;}
        public int? Floor {get;set;}
        public bool isMocked  {get;set;} = false;
        public int? StaffMobileId { get; set; }
        public virtual StaffMobile StaffMobile { get; set; }
    }
}
