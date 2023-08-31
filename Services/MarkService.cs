using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Admin_Panel.Data;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
using AutoMapper;
namespace Admin_Panel.Services
{
    public class MarkService : IMarkService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MarkService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CancellationToken cancellationToken = default
        public async Task<IEnumerable<Mark>> GetAllAsync(CancellationToken cancellationToken = default)
        {
// cancellationToken
            var result = await _context.Marks.ToListAsync();
            var resultOut = _mapper.Map<IEnumerable<Mark>>(result);

            return resultOut;
        }

        public async Task<Mark> GetByIdAsync(int id, CancellationToken cancellationToken = default)

        {
            var mark = await _context.Marks.FirstOrDefaultAsync(e => e.Id == id);


            return mark;
        }

        public async Task<Mark> Create(Mark mark)
        {
            var res = await _context.Marks
          .Where(result => result.Lang == mark.Lang)
         .OrderByDescending(result => result.Position)
    .Select(result => new { result.Id, result.Position })
    .FirstOrDefaultAsync();
            mark.Position = (res != null && res.Position.HasValue) ? res.Position.Value + 1 : 1;
            mark.Status = 3;
            _context.Add(mark);
            await _context.SaveChangesAsync();
            return mark;
        }

        public async Task Update(Mark mark)
        {
            var existingMark = await _context.Marks
              .FirstOrDefaultAsync(m => m.Id == mark.Id);

            if (existingMark != null)
            {
                mark.Lang = existingMark.Lang;

                var originId = mark.OriginId;
                if (originId.HasValue && originId != 0)
                {
                    var originMark = await _context.Marks
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originMark != null)
                    {
                        originMark.Color = mark.Color.Trim();
                        originMark.Status = mark.Status;
                        _context.Update(originMark);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originMark = await _context.Marks
                      .FirstOrDefaultAsync(m => m.OriginId == mark.Id);

                    if (originMark != null)
                    {
                        originMark.OriginId = mark.Id;
                        originMark.Color = mark.Color.Trim();
                        originMark.Status = mark.Status;
                        _context.Update(originMark);
                        await _context.SaveChangesAsync();
                    }
                }


                existingMark.UpdatedAt = DateTime.Now;

                // Update the properties of the existingMark object
                _context.Entry(existingMark).CurrentValues.SetValues(mark);
                _context.Update(existingMark);
                await _context.SaveChangesAsync();
            }



            // mark.UpdatedAt = DateTime.Now;
            // _context.Update(mark);
            // await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {



            var mark = await _context.Marks
               .FirstOrDefaultAsync(m => m.Id == id);

            if (mark != null)
            {
                var originId = mark.OriginId;

                if (originId.HasValue && originId != 0)
                {
                    var originMark = await _context.Marks
                        .FirstOrDefaultAsync(m => m.Id == originId.Value);

                    if (originMark != null)
                    {
                        _context.Marks.Remove(originMark);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                    var originMark = await _context.Marks
                      .FirstOrDefaultAsync(m => m.OriginId == id);

                    if (originMark != null)
                    {
                        _context.Marks.Remove(originMark);

                        await _context.SaveChangesAsync();
                    }
                }

                _context.Marks.Remove(mark);
                await _context.SaveChangesAsync();

            }
        }
    }
}