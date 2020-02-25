using DevOps.Data.Interfaces;
using DevOps.DataEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Data.DataRepository
{
    public class SubMenuDataRepository : ISubMenuDataRepository
    {
        private DevOpsEntities db;
        public SubMenuDataRepository()
        {
            db = new DevOpsEntities();
        }
        public List<SubMenu> GetSubMenus()
        {
            return db.SubMenus.ToList();
        }
    }
}
