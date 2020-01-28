using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Educafin.Models;
using System.Data.SqlClient;
using MvcApplication8.Models;

namespace Educafin.Controllers
{
    public class Proveedor_InstitucionController : Controller
    {
        private Model1 db = new Model1();

        // GET: Proveedor_Institucion
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            var proveedor_Institucion = db.Proveedor_Institucion.Include(p => p.institucion).Include(p => p.proveedor);
            return View(proveedor_Institucion.ToList());
        }

        // GET: Proveedor_Institucion/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor_Institucion proveedor_Institucion = db.Proveedor_Institucion.Find(id);
            if (proveedor_Institucion == null)
            {
                return HttpNotFound();
            }
            return View(proveedor_Institucion);
        }

        // GET: Proveedor_Institucion/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            ViewBag.cct_int = new SelectList(db.institucion, "CCT", "nombre");
            ViewBag.rfc_prov = new SelectList(db.proveedor, "RFC", "nombre");

            return View();
        }

        // POST: Proveedor_Institucion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cct_int,rfc_prov,cantidad,estado")] Proveedor_Institucion proveedor_Institucion)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (ModelState.IsValid)
            {
                db.Proveedor_Institucion.Add(proveedor_Institucion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cct_int = new SelectList(db.institucion, "CCT", "nombre", proveedor_Institucion.cct_int);
            ViewBag.rfc_prov = new SelectList(db.proveedor, "RFC", "nombre", proveedor_Institucion.rfc_prov);
            return View(proveedor_Institucion);
        }

        // GET: Proveedor_Institucion/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor_Institucion proveedor_Institucion = db.Proveedor_Institucion.Find(id);
            if (proveedor_Institucion == null)
            {
                return HttpNotFound();
            }
            ViewBag.cct_int = new SelectList(db.institucion, "CCT", "nombre", proveedor_Institucion.cct_int);
            ViewBag.rfc_prov = new SelectList(db.proveedor, "RFC", "nombre", proveedor_Institucion.rfc_prov);
            return View(proveedor_Institucion);
        }

        // POST: Proveedor_Institucion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cct_int,rfc_prov,cantidad,estado")] Proveedor_Institucion proveedor_Institucion)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (ModelState.IsValid)
            {
                db.Entry(proveedor_Institucion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cct_int = new SelectList(db.institucion, "CCT", "nombre", proveedor_Institucion.cct_int);
            ViewBag.rfc_prov = new SelectList(db.proveedor, "RFC", "nombre", proveedor_Institucion.rfc_prov);
            return View(proveedor_Institucion);
        }

        // GET: Proveedor_Institucion/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor_Institucion proveedor_Institucion = db.Proveedor_Institucion.Find(id);
            if (proveedor_Institucion == null)
            {
                return HttpNotFound();
            }
            return View(proveedor_Institucion);
        }

        // POST: Proveedor_Institucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            Proveedor_Institucion proveedor_Institucion = db.Proveedor_Institucion.Find(id);
            db.Proveedor_Institucion.Remove(proveedor_Institucion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
