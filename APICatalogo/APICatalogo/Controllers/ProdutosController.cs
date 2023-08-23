﻿using APICatalogo.Context;
using APICatalogo.Models;
using Humanizer.Localisation.DateToOrdinalWords;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("primeiro")]
        public ActionResult<Produto> GetPrimeiro()
        {
            try
            {
                var produto = _context.Produtos.AsNoTracking().FirstOrDefault();
                if (produto is null)
                {
                    return NotFound();
                }
                return produto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.AsNoTracking().ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados...");
                }
                return produtos;
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }


        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                return produto;
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }



        [HttpPost]
        public ActionResult Post(Produto produto)
        {

            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p=>p.ProdutoId == id);   

            if(produto is null)
            {
                return NotFound("Produto não localizado");
            }

            _context.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

    }
}
