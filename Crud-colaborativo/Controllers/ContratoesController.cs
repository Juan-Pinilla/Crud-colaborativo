using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crud_colaborativo.Data;
using Crud_colaborativo.Models;

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
                if (propuestaContrato != null && propuestaContrato.Length > 0)
                {
                    // Generar un nombre de archivo único
                    string nombreArchivo = Path.GetRandomFileName() + Path.GetExtension(propuestaContrato.FileName);

                    // Ruta donde se almacenará el archivo en el servidor (puedes cambiarla según tus necesidades)
                    string rutaDestino = Path.Combine(_env.WebRootPath, "Multimedia", nombreArchivo);

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(rutaDestino, FileMode.Create))
                    {
                        await propuestaContrato.CopyToAsync(stream);
                    }

                    // Almacenar la ruta del archivo en la propiedad PropuestaContrato del modelo Contrato
                    contrato.PropuestaContrato = "/Multimedia/" + nombreArchivo;
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
