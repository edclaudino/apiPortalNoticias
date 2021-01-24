﻿using System.Collections.Generic;
using System.Linq;
using PortalDeNoticias.Data;
using PortalDeNoticias.Models;
using Microsoft.EntityFrameworkCore;
using PortalDeNoticias.Services.Exceptions;
using System.Threading.Tasks;

namespace PortalDeNoticias.Services
{
    public class NoticiaService
    {
        private readonly PortalDeNoticiasContext _context;

        public NoticiaService(PortalDeNoticiasContext context)
        {
            _context = context;
        }

        public async Task<List<Noticia>> FindAllAsync()
        {
            return await _context.Noticia.ToListAsync();
        }

        public async Task InsertAsync(Noticia obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Noticia> FindByIdAsync(int id)
        {
            return await _context.Noticia.Include(obj => obj.Autor).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Noticia.FindAsync(id);
                _context.Noticia.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Noticia obj)
        {
            bool hasAny = await _context.Noticia.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Notícia não encontrada");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
