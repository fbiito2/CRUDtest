using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class employeeUtility
    {
        public object GetEmployeesList()
        {
            using (NorthwindContext db = new NorthwindContext())
            {
                var q = from s in db.Employees
                        orderby s.EmployeeID descending
                        select s;
                return q.ToList();
            }

        }
    }
}