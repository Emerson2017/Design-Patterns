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
    public class ProjetoController : Controller
    {
        private CatalogoDeProdutosEntities db = new CatalogoDeProdutosEntities();

        // GET: Projeto
        public ActionResult Index()
        {
            var projeto = db.Projeto.Include(p => p.Unidade);
            return View(projeto.ToList());
        }

        // GET: Projeto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projeto.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // GET: Projeto/Create
        public ActionResult Create()
        {
            ViewBag.IdUnidade = new SelectList(db.Unidade, "Id", "Nome");
            return View();
        }

        // POST: Projeto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cabecalho,Descricao,IdUnidade")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                db.Projeto.Add(projeto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUnidade = new SelectList(db.Unidade, "Id", "Nome", projeto.IdUnidade);
            return View(projeto);
        }

        // GET: Projeto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projeto.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUnidade = new SelectList(db.Unidade, "Id", "Nome", projeto.IdUnidade);
            return View(projeto);
        }

        // POST: Projeto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cabecalho,Descricao,IdUnidade")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projeto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUnidade = new SelectList(db.Unidade, "Id", "Nome", projeto.IdUnidade);
            return View(projeto);
        }

        // GET: Projeto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projeto.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // POST: Projeto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projeto projeto = db.Projeto.Find(id);
            db.Projeto.Remove(projeto);
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
