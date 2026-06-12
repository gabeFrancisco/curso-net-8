using System.Text.Json;
using System.Text.Json.Serialization;
using ApiCatalogo.Context;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriasController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("LerArquivoConfiguracao")]
        public string[] GetValores()
        {
            var chave1 = _configuration["chave1"]!;
            var chave2 = _configuration["chave2"]!;
            var secao1 = _configuration["secao1"]!.ToList();
            return [
                chave1,
                chave2,
                JsonSerializer.Serialize(secao1)
            ];
        }


        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromServices([FromServices]IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

    [HttpGet("SemUsarFromServices/{nome}")]
        public ActionResult<string> SemUsarFromServices(IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _context.Categorias!
                    .AsNoTracking()
                    .ToList();

                if (categorias is null)
                {
                    return NotFound();
                }

                return Ok(categorias);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação!"
                );
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id, [BindRequired]string nome)
        {
            var categoria = _context.Categorias!.FirstOrDefault(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest();
            }

            _context.Categorias!.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId });
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias!.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound();
            }

            _context.Categorias!.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }

    }
}