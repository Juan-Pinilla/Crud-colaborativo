using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crud_colaborativo.Data;
using Crud_colaborativo.Models;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dropbox.Api;
using System.Net;
using Dropbox.Api.Files;
using Firebase.Storage;

namespace Crud_colaborativo.Controllers
{
    public class ContratoesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;


        public ContratoesController(ApplicationDbContext context, IWebHostEnvironment env, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _env = env;
            _httpClientFactory = httpClientFactory;
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



        [HttpGet]
        public async Task<IActionResult> DescargarArchivo(string rutaArchivo)
        {
                    Console.WriteLine(rutaArchivo);
            try
            {
                string tokenDeAcceso = "uat.AE0jU12H1UWBuJsKql3dVFCzb7i1BNdphwo3U0Xr3ffn5FE5fDdhziUCuSwzt1ELp6ssBYeC5rZFZ4hW1vgp4OjV263EHAS0z_bisYfZjWBX1tgKclIPAL5AkeVsYOw7WngI-owl-If4D3wxhQ-QJxmCmOim6xmeO97-I5Dg4qS1VDVeCRUDmAU3JS5wtHSG47PI_ViC0YN0IjL2Obs3G6ngnf8me-NGtEf_awcG2p9e4niu6o16e_k8Nh2M_5zuANIt0YaSFijw3Jiot6_jjsBkmxkfJOZpZTaMiKfUiYtcaWOYTx765FF-6w0_RPIo_8uHWvwZYzjmh3dZj0WDzTzn5SyXE7FHX7PWSBkFGCzdUW8w7ne55_pSh2nHTd2UvAyFG391dYX4Q1IdZrWuQRZL4wdVoAEw-QaRv88dD2ozYP9wDkKolXAcATw1cyk4iQ0Avnlsqe6q6bJcxTYQTdflM8sgReHZRaZB73AXkfrQRPcNJHZhrPY1oiO3gw-dIopiui94y45jRfFh7KlYF-7Vx8k38RSAj3sqjUFePNNs6w5RJ4AvEFpfv-FEQhBkrBpcmIX9_kZpQwIVkdyhTCrIErotidBvNqXxC51mJ5jE3a7306nAb6JV_WEKj9qr4wOxEp8-cQPSUOOSKiZu2IL03Xia5QeU9slhhArZfkmQTaRmpxsit6yDOhs6DnKpXjewkX_Dg1t3uq2p7Ed8taC_IL_p1HcXnZIc2du4Wnt9GFCgrFSsaKJOYGQP9JwX8Cr-MvweD3LkWeu2MlYenUZnHaga7HHUsE36IqT6P8__-VkQb3muE0j5simd0ulmpxkP11FQc6A4jKpDhqPPQbMTqVjzaxTto20f1CMx8O1Opp5dJbGAac2fvHvg3g7IISRJbgCiBfS-C5HtGxg5y4AoGK_tJHsqrPGUvMMsAraT7CgC3OdkNTlnLsnh-EOPBZl4st1t5H0_pfxn7jG_hz9FiUIXRzdQ0Ur5W3aZ1g0gmb8Euylloo328iJjSZZj4D65dg4ev_H0lZ8WV7gu0pcA_ZzDdsBvkqlwupvpgG5ZY7EsqSn9NcMASfF9Z8ym1AQ5OQaBqcAqCDEyHWj-bnNGxM_piN6RwZz1OZCARoS70TWwzXg3JbAjm8NtLvvJYSpIRo0XV7dIjmg9kIIrjD_-8gvDUnIaLkBKEI1tfcY6AW1o5VfbVkEHdUU3eYgsbNeql7j0EmYw8IaIx2quNlsQzbtQ8Gq3swSCw8LkdX_eKo1J-6-k3v19oxqDsswouyjw99KP3y1OyVX0tv1ynUTNmQQdG2cBXdIRk-kHt5h3_1boCKqgezfa_gp2fPfQcx0VYMbWBT41YEdRLlDktw7XleVdLUFZeKy20SoK7PNkOt1xxE1eo-YfgPoZdxo8lpmt1m_L-ktBjw8K8wH4AIk_z1JykKMSq2DSiSCjYELHfgAMgMHqSeCCDJ7liOpEi-zycfhCVIyMUEXQ6M9kzxxB"; // Agrega tu token de acceso aquí
                string url = $"https://content.dropboxapi.com/2/files/download";

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    // Configurar el encabezado de autorización con el token de acceso
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenDeAcceso}");

                    // Especificar la ruta del archivo que deseas descargar
                    string ruta = $"/Imagenes/{rutaArchivo}";
                    Console.WriteLine(ruta);
                    httpClient.DefaultRequestHeaders.Add("Dropbox-API-Arg", $"{{\"path\": \"{ruta}\"}}");

                    // Realizar la solicitud para descargar el archivo
                    var response = await httpClient.PostAsync(url, null);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido del archivo como una matriz de bytes
                        var archivoBytes = await response.Content.ReadAsByteArrayAsync();

                        // Devolver el archivo descargado como una respuesta de tipo archivo
                        return File(archivoBytes, "application/octet-stream",ruta);
                    }
                    else
                    {
                        // Manejar el caso de respuesta no exitosa
                        return BadRequest("No se pudo descargar el archivo desde Dropbox.");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Capturar errores de solicitud HTTP
                return StatusCode(500, $"Error de solicitud HTTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Capturar otros errores
                return StatusCode(500, $"Error al descargar el archivo: {ex.Message}");
            }
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
