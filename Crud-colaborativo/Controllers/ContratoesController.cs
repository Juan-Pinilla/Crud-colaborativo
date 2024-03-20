using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crud_colaborativo.Data;
using Crud_colaborativo.Models;
using Firebase.Storage;


namespace Crud_colaborativo.Controllers
{
    public class ContratoesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        

        public ContratoesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Contratoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contratos.ToListAsync());
        }

        // GET: Contratoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = await _context.Contratos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contrato == null)
            {
                return NotFound();
            }

            return View(contrato);
        }

        // GET: Contratoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contratoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoCliente,Empresa,Referencia,EstadoContrato,FechaInicio,FechaFinalizacion,Socio,Gerente,Senior,SocioParticipacion,SocioComercial,PropuestaContrato")] Contrato contrato, IFormFile propuestaContrato)
        {
            Random random = new Random();
            int numero = random.Next(1, 999999999);
            string IdActual = contrato.Id;
            string NuevoID = IdActual + numero.ToString();
            contrato.Id = NuevoID;

            if (ModelState.IsValid)
            {
                try
                {
                    string firebaseBucket = "almacenamiento-6efa5.appspot.com"; // URL base del bucket de Firebase Storage

                    // Configurar FirebaseStorage
                    FirebaseStorage firebaseStorage = new FirebaseStorage(firebaseBucket, new FirebaseStorageOptions
                    {
                        // Puedes agregar opciones adicionales aquí si es necesario
                    });

                    // Subir el archivo a Firebase Storage
                    using (var stream = propuestaContrato.OpenReadStream())
                    {
                        // Especifica la carpeta y el nombre del archivo en Firebase Storage
                        var task = await firebaseStorage
                            .Child("Archivos") // Especifica la carpeta
                            .Child(propuestaContrato.FileName) // Especifica el nombre del archivo
                            .PutAsync(stream);
                    }

                    // Almacenar la ruta del archivo en la propiedad PropuestaContrato del modelo Contrato
                    contrato.PropuestaContrato = propuestaContrato.FileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al subir archivo a Firebase Storage: {ex.Message}");
                    ModelState.AddModelError("PropuestaContrato", $"Error al subir archivo a Firebase Storage: {ex.Message}");
                    return View(contrato);
                }

                // Agregar el contrato a la base de datos    
                _context.Add(contrato);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(contrato);
        }


        // GET: Contratoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        // POST: Contratoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TipoCliente,Empresa,Referencia,EstadoContrato,FechaInicio,FechaFinalizacion,Socio,Gerente,Senior,SocioParticipacion,SocioComercial,PropuestaContrato")] Contrato contrato)
        {
            if (id != contrato.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contrato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratoExists(contrato.Id))
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
            return View(contrato);
        }

        // GET: Contratoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = await _context.Contratos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contrato == null)
            {
                return NotFound();
            }

            return View(contrato);
        }

        // POST: Contratoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                _context.Contratos.Remove(contrato);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContratoExists(string id)
        {
            return _context.Contratos.Any(e => e.Id == id);
        }
    }
}
