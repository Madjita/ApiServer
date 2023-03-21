using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MobileDrill.Services.RepositoryFolder;
using Database.Contexts.Models;
using Models.DTO;

namespace Database.Contexts.Services
{
    public class DbContextService
    {
        private readonly IRepository<StaffMobile> _rStaffMobile;
        private readonly IRepository<GPSPosition> _rGPSPosition;


        public DbContextService(
            IRepository<StaffMobile> rStaffMobile,
            IRepository<GPSPosition> rGPSPosition
            )
        {
            _rStaffMobile = rStaffMobile;
            _rGPSPosition = rGPSPosition;
        }

        public async Task saveGPSAsync(GPSPhonePosition gpsPhonePosition,StaffMobile user)
        {
            await _rGPSPosition.AddAsync(new GPSPosition(gpsPhonePosition,user));
        }

        public async Task<StaffMobile> setUserAsync(string realIP)
        {
            var findUser = (await _rStaffMobile.WhereAsync(x => x.realIP == realIP)).FirstOrDefault();
            if(findUser is null)
            {   
                var user = new StaffMobile(realIP);
                await _rStaffMobile.AddAsync(user);
                return user;
            }
            return findUser;
        }
    }
}