using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class employeeUtility
    {
        public List<Employees> GetEmployeesList(int pageid)
        {
            using (NorthwindContext db = new NorthwindContext())
            {
                var emp = from s in db.Employees
                          select s;

                int sk = 0;

                if (pageid>0)
                {
                    sk = (pageid-1) * 2;
                }

                var q = (from s in db.Employees
                        orderby s.EmployeeID descending
                        select s).Skip(sk).Take(2);

                return q.ToList();
            }

        }
    }
}