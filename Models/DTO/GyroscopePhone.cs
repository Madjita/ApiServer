using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.DTO
{
    public class GyroscopePhone : Point3
    {
        public override string ToString()
        {
            return $"[GyroscopeEvent (x: {X}, y: {Y}, z: {Z})]";
        }
        public string toJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}