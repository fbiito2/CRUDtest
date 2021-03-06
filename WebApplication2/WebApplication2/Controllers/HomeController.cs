﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindContext _db = new NorthwindContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Index(string searchName, int pageNumber = 1, int page = 1)
        {
            int nowPage = page;
            var a = (from s in _db.Employees
                     select s);

            int pageCount = (int)Math.Ceiling((a.Count() / 2d));

            ViewBag.firstPage = 1;
            ViewBag.lastPage = pageCount;

            if (pageNumber == -2)
            {
                nowPage = nowPage + 1;
            }
            else if (pageNumber == -1)
            {
                nowPage = nowPage - 1;
            }else if (pageNumber== pageCount)
            {
                nowPage = pageCount;
            }

            if (nowPage > pageCount)
            {
                page = pageCount;
                nowPage = pageCount;
            }else if (nowPage<1)
            {
                page = 1;
                nowPage = 1;
            }

            ViewBag.nowPage = nowPage;

            employeeUtility utility = new employeeUtility();
            var employeeList = utility.GetEmployeesList(nowPage);

            if (!string.IsNullOrEmpty(searchName))
            {
                employeeList = _db.Employees.Where(t => t.FirstName.Contains(searchName)).ToList();
            }

            return View(employeeList);
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employees _employee)
        {

            _db.Employees.Add(_employee);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int? _id)
        {
            if (_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees _employees = _db.Employees.Find(_id);
            if (_employees == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(_employees);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employees _employees)
        {
            if (_employees == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                _db.Entry(_employees).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index", new { _id = _employees.EmployeeID });
            }
            else
            {
                return Content("更新失敗");
            }
        }

        public ActionResult Delete(int? _id)
        {
            if (_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees _employees = _db.Employees.Find(_id);
            if (_employees == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(_employees);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int _id)
        {

            Employees employees = _db.Employees.Find(_id);
            _db.Employees.Remove(employees);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}