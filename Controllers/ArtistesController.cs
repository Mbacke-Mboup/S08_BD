using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using S08_Labo.Data;
using S08_Labo.Models;
using S08_Labo.ViewModel;
using S08_Labo.ViewModels;

namespace S08_Labo.Controllers
{
    public class ArtistesController : Controller
    {
        private readonly S08_EmployesContext _context;

        public ArtistesController(S08_EmployesContext context)
        {
            _context = context;
        }

        // GET: Artistes
        public async Task<IActionResult> Index()
        {
            var s08_EmployesContext = _context.Artistes.Include(a => a.Employe);
            return View(await s08_EmployesContext.ToListAsync());
        }

        public async Task<IActionResult> Index2()
        {
            var artistes = await  _context.VwListeArtistes.ToListAsync();
            return View(artistes);
        }

        // GET: Artistes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artistes == null)
            {
                return NotFound();
            }

            var artiste = await _context.Artistes
                .Include(a => a.Employe)
                .FirstOrDefaultAsync(m => m.ArtisteId == id);
            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        // GET: Artistes/Create
        public IActionResult Create()
        {
            ArtisteEmployeViewModel artiste = new ArtisteEmployeViewModel();
            return View(artiste);
        }

        // POST: Artistes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtisteEmployeViewModel artisteEmploye)
        {
            string query = "EXEC Employes.USP_AjouterArtiste @Prenom, @Nom, @NoTel, @Courriel, @Specialite";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Prenom", Value = artisteEmploye.Employe.Prenom},
                new SqlParameter{ParameterName = "@Nom", Value = artisteEmploye.Employe.Nom},
                new SqlParameter{ParameterName = "@NoTel", Value = artisteEmploye.Employe.NoTel},
                new SqlParameter{ParameterName = "@Courriel", Value = artisteEmploye.Employe.Courriel},
                new SqlParameter{ParameterName = "@Specialite", Value = artisteEmploye.Artiste.Specialite},
            };
            await _context.Database.ExecuteSqlRawAsync(query, parameters.ToArray());
            await _context.SaveChangesAsync();
            return View(artisteEmploye);
        }

        // GET: Artistes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artistes == null)
            {
                return NotFound();
            }

            var artiste = await _context.Artistes.FindAsync(id);
            if (artiste == null)
            {
                return NotFound();
            }
            ViewData["EmployeId"] = new SelectList(_context.Employes, "EmployeId", "EmployeId", artiste.EmployeId);
            return View(artiste);
        }

        // POST: Artistes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtisteId,Specialite,EmployeId")] Artiste artiste)
        {
            if (id != artiste.ArtisteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artiste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtisteExists(artiste.ArtisteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeId"] = new SelectList(_context.Employes, "EmployeId", "EmployeId", artiste.EmployeId);
            return View(artiste);
        }

        // GET: Artistes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artistes == null)
            {
                return NotFound();
            }

            var artiste = await _context.Artistes
                .Include(a => a.Employe)
                .FirstOrDefaultAsync(m => m.ArtisteId == id);
            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        // POST: Artistes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artistes == null)
            {
                return Problem("Entity set 'S08_EmployesContext.Artistes'  is null.");
            }
            var artiste = await _context.Artistes.FindAsync(id);
            if (artiste != null)
            {
                _context.Artistes.Remove(artiste);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtisteExists(int id)
        {
          return (_context.Artistes?.Any(e => e.ArtisteId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Query1()
        {
            // Données des employés embauchés en 2023 (Utilisez VwListeArtiste)
            DateTime date = new DateTime(2023, 1, 1);
            IEnumerable<VwListeArtiste> artistes = await _context.VwListeArtistes.Where(a => a.DateEmbauche >= date).ToListAsync();

            // N'oubliez pas d'envoyer artistes à la vue Razor ! 
            return View(artistes);
        }

        public async Task<IActionResult> Query2()
        {
            IEnumerable<VwListeArtiste> artistes = await _context.VwListeArtistes.Where(a => a.Specialite == "modélisation 3D").ToListAsync();

            return View(artistes);
        }

        public async Task<IActionResult> Query3()
        {
            // Prénom et nom de tous les employés, classés par prénom ascendant
            // Concaténez prénoms et noms (avec une espace au centre) pour simplement obtenir une liste de strings.
            IEnumerable<Employe> employes = await _context.Employes.OrderBy(e=> e.Prenom).ToListAsync();
            List<string> noms = new List<string>();

            foreach(Employe employe in employes)
            {
                noms.Add(employe.Prenom + " " + employe.Nom);
                
            }
            return View(noms);
        }

        public async Task<IActionResult> Query4()
        {
            
            IEnumerable<VwListeArtiste> vWartistes = await _context.VwListeArtistes.ToListAsync();
            IEnumerable<Artiste> artistes = await _context.Artistes.ToListAsync();
            IEnumerable<Employe> employes = await _context.Employes.ToListAsync();
            List<ArtisteEmployeViewModel> viewModels = new List<ArtisteEmployeViewModel>();

            vWartistes.ToList().ForEach(async v =>
            {
                ArtisteEmployeViewModel view = new ArtisteEmployeViewModel();
                view.Artiste = artistes.FirstOrDefault(a => a.ArtisteId == v.ArtisteId);
                view.Employe = employes.FirstOrDefault(a => a.EmployeId == v.EmployeId);
                viewModels.Add(view);
            });




            // N'oubliez pas d'envoyer artistes à la vue Razor ! 
            return View(viewModels);

        }

        public async Task<IActionResult> Query5()

        {
            IEnumerable<VwListeArtiste> vWartistes = await _context.VwListeArtistes.ToListAsync();
            var requete = vWartistes.Select(v => new
            {
                specialite = v.Specialite,
                id = v.ArtisteId
            }).GroupBy(s => s.specialite).Select(b => new NbSpecialiteViewModel(b.Key, b.Count())) ;
            
            return View(requete);
        }

        public async Task<IActionResult> Query6()
        {
            IEnumerable<VwNbEmpl0yesParSpecialite> artistes = await _context.VwNbEmpl0yesParSpecialites.ToListAsync();

            return View(artistes);

        }
    }
}
