﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ecoit.Data;
using Ecoit.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ecoit.Pages.Instructor.Formations
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Ecoit.Data.DataContext _context;

        public DeleteModel(Ecoit.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Formation Formation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Formation = await _context.Formation.FirstOrDefaultAsync(m => m.FormID == id);

            if (Formation == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Formation = await _context.Formation.FindAsync(id);

            if (Formation != null)
            {
                _context.Formation.Remove(Formation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
