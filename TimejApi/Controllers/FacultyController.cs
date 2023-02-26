﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [Route("api/faculty")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly ScheduleDbContext _context;

        public FacultyController(ScheduleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Faculty>> Get(Guid id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// [IMPLEMENTED]
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<Faculty[]>> GetAll()
        {
            return Ok(_context.Faculties.ToArray());
        }

        /// <summary>
        /// [IMPLEMENTED]
        /// </summary>
        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Faculty>> Post(FacultyCreation faculty)
        {
            var entry = _context.Faculties.Add(new Faculty(faculty.Name));
            await _context.SaveChangesAsync();
            return Ok(entry.Entity);
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Faculty>> Put(Guid id, FacultyCreation faculty)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}