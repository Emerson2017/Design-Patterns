using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using versionamento.Models;

namespace versionamento.Controllers
{
    public class RepositorioController : Controller
    {
        private CatalogoDeProdutosEntities db = new CatalogoDeProdutosEntities();

        // GET: Repositorios
        public ActionResult Index()
        {
            var repositorio = db.Repositorio.Include(r => r.Projeto).Include(r => r.Tipo);
            return View(repositorio.ToList());
        }

        // GET: Repositorios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repositorio repositorio = db.Repositorio.Find(id);
            if (repositorio == null)
            {
                return HttpNotFound();
            }
            return View(repositorio);
        }

        // GET: Repositorios/Create
        public ActionResult Create()
        {
            ViewBag.IdProjeto = new SelectList(db.Projeto, "Id", "Cabecalho");
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nome");
            return View();
        }

        // POST: Repositorios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdProjeto,Codigo,IdTipo")] Repositorio repositorio)
        {
            if (ModelState.IsValid)
            {
                db.Repositorio.Add(repositorio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProjeto = new SelectList(db.Projeto, "Id", "Cabecalho", repositorio.IdProjeto);
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nome", repositorio.IdTipo);
            return View(repositorio);
        }

        // GET: Repositorios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repositorio repositorio = db.Repositorio.Find(id);
            if (repositorio == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProjeto = new SelectList(db.Projeto, "Id", "Cabecalho", repositorio.IdProjeto);
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nome", repositorio.IdTipo);
            return View(repositorio);
        }

        // POST: Repositorios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdProjeto,Codigo,IdTipo")] Repositorio repositorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repositorio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProjeto = new SelectList(db.Projeto, "Id", "Cabecalho", repositorio.IdProjeto);
            ViewBag.IdTipo = new SelectList(db.Tipo, "Id", "Nome", repositorio.IdTipo);
            return View(repositorio);
        }

        // GET: Repositorios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repositorio repositorio = db.Repositorio.Find(id);
            if (repositorio == null)
            {
                return HttpNotFound();
            }
            return View(repositorio);
        }

        // POST: Repositorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Repositorio repositorio = db.Repositorio.Find(id);
            db.Repositorio.Remove(repositorio);
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
