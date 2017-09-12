using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TreinaWeb.Api.AcessoDados.Entity.Context;
using TreinaWeb.Api.Dominio;
using TreinaWeb.Comum.Repositorios.Interfaces;
using TreinaWeb.MinhaApi.Api.AutoMapper;
using TreinaWeb.MinhaApi.Api.DTOs;
using TreinaWeb.MinhaApi.Api.Filters;
using TreinaWeb.MinhaApi.Repositorios.Entity;

namespace TreinaWeb.MinhaApi.Api.Controllers
{
    public class AlunosController : ApiController
    {
        private IRepositorioTreinaWeb<Aluno, int> _repositorioAlunos = new RepositorioAlunos(new MinhaApiDbContext());

        //public IEnumerable<Aluno> Get()
        public IHttpActionResult Get()
        {
            //var alunos = _repositorioAlunos.Selecionar();
            //return Ok(alunos);

            List<Aluno> alunos = _repositorioAlunos.Selecionar();
            List<AlunoDTO> dtos = AutoMapperManager.Instance.Mapper.Map<List<Aluno>, List<AlunoDTO>>(alunos);

            return Ok(dtos);
        }

        //public HttpResponseMessage Get(int? id)
        public IHttpActionResult Get(int? id)
        {
            //if(!id.HasValue)            
            //    return Request.CreateResponse(HttpStatusCode.BadRequest);           

            //var aluno = _repositorioAlunos.SelecionarPorID(id.Value);

            //if (aluno == null)
            //    return Request.CreateResponse(HttpStatusCode.NotFound);

            //return Request.CreateResponse(HttpStatusCode.Found, aluno);


            if (!id.HasValue)
                return BadRequest();

            var aluno = _repositorioAlunos.SelecionarPorID(id.Value);

            if (aluno == null)
                return NotFound();

            AlunoDTO dto = AutoMapperManager.Instance.Mapper.Map<Aluno, AlunoDTO>(aluno);

            return Content(HttpStatusCode.Found, dto);
        }

        //public HttpResponseMessage Post([FromBody]Aluno aluno) // Não é mais feito desta maneira
        [ApplyModelValidation]
        public IHttpActionResult Post([FromBody]AlunoDTO dto) // Não é mais feito desta maneira
        {
            //if(ModelState.IsValid)
            // {
            try
            {
                Aluno aluno = AutoMapperManager.Instance.Mapper.Map<AlunoDTO, Aluno>(dto);
                _repositorioAlunos.Inserir(aluno);

                var url = $"{Request.RequestUri}/{aluno.Id}";
                return Created(url, aluno);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            // }
            // else
            // {
            //     return BadRequest(ModelState);
            // }



            //try
            //{
            //    _repositorioAlunos.Inserir(aluno);
            //    return Request.CreateResponse(HttpStatusCode.Created);
            //}
            //catch (Exception e)
            //{
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            //}

        }

        [ApplyModelValidation]
        public IHttpActionResult Put(int? id, AlunoDTO dto)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest();

                Aluno aluno = AutoMapperManager.Instance.Mapper.Map<AlunoDTO, Aluno>(dto);

                aluno.Id = id.Value;
                _repositorioAlunos.Atualizar(aluno);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest();

                var aluno = _repositorioAlunos.SelecionarPorID(id.Value);
                if (aluno == null)
                    return NotFound();

                _repositorioAlunos.ExlcuirPorID(id.Value);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}