using AudiovisualLibraryAPI.Data;
using AudiovisualLibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AudiovisualLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicAlbumController : ControllerBase
    {
        private DataContext _context;
        private Regex nameRegex;
        public MusicAlbumController(DataContext context)
        {
            _context = context;
            nameRegex = new Regex("^[A-Z](\\w| )*$");
        }
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MusicAlbum>> Post(MusicAlbum musicAlbum)
        {
            try
            {
                if (musicAlbum == null)
                {
                    return BadRequest("Request is null");
                }
                if (musicAlbum.Id != 0)
                {
                    return BadRequest("Id must be 0");
                }
                if (!nameRegex.IsMatch(musicAlbum.Author))
                {
                    return BadRequest("Author name must start with a upper case letter, and cannot contain special characters");
                }
                if (!nameRegex.IsMatch(musicAlbum.Name))
                {
                    return BadRequest("Album name must start with a upper case letter, and cannot contain special characters");
                }
                _context.MusicAlbums.Add(musicAlbum);
                await _context.SaveChangesAsync();
                return Created("MusicAlbum", musicAlbum);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<MusicAlbum>>> Get()
        {
            try { return Ok(await _context.MusicAlbums.ToListAsync()); }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MusicAlbum>> GetId(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var result = await _context.MusicAlbums.FindAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Author")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<MusicAlbum>>> GetAuthor(string author)
        {
            try
            {
                List<MusicAlbum> result = new List<MusicAlbum>();
                var allMusicAlbums = await _context.MusicAlbums.ToArrayAsync();
                foreach (var musicAlbum in allMusicAlbums)
                {
                    if (musicAlbum.Author == author) result.Add(musicAlbum);
                }
                if (result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MusicAlbum>> Put(MusicAlbum musicAlbum)
        {
            try
            {
                var updated = await _context.MusicAlbums.FindAsync(musicAlbum.Id);
                if (updated == null)
                {
                    return NotFound();
                }
                if (!nameRegex.IsMatch(musicAlbum.Author))
                {
                    return BadRequest("Author name must start with a upper case letter, and cannot contain special characters");
                }
                if (!nameRegex.IsMatch(musicAlbum.Name))
                {
                    return BadRequest("Album name must start with a upper case letter, and cannot contain special characters");
                }
                updated.Id = musicAlbum.Id;
                updated.Author = musicAlbum.Author;
                updated.Name = musicAlbum.Name;
                updated.Genre = musicAlbum.Genre;
                updated.NumberOfRecords = musicAlbum.NumberOfRecords;
                await _context.SaveChangesAsync();
                return Created("MusicAlbum", musicAlbum);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("Id")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MusicAlbum>> Delete(int id)
        {
            try
            {
                var result = await _context.MusicAlbums.FindAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                _context.MusicAlbums.Remove(result);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

