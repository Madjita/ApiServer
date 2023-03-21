using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Hubs.DTO
{
    public class GyroscopePhone
    {
        double X {get;set;}
        double Y {get;set;}
        double Z {get;set;}

        public override string ToString()
        {
            return $"[GyroscopeEvent (x: {X}, y: {Y}, z: {Z})]";
        }
    }
}