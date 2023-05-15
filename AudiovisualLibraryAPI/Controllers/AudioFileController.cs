using AudiovisualLibraryAPI.Data;
using AudiovisualLibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AudiovisualLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioFileController : ControllerBase
    {
        private DataContext _context;
        private Regex nameRegex;
        public AudioFileController (DataContext context)
        {
            _context = context;
            nameRegex = new Regex("^[A-Z](\\w| )*$");
        }
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AudioFile>> Post(AudioFile audioFile)
        {
            try
            {
                if (audioFile == null)
                {
                    return BadRequest("Request is null");
                }
                if (audioFile.Id != 0)
                {
                    return BadRequest("Id must be 0");
                }
                if (!nameRegex.IsMatch(audioFile.Author))
                {
                    return BadRequest("Author name must start with a upper case letter, and cannot contain special characters");
                }
                if (!nameRegex.IsMatch(audioFile.Name))
                {
                    return BadRequest("File name must start with a upper case letter, and cannot contain special characters");
                }
                _context.AudioFiles.Add(audioFile);
                await _context.SaveChangesAsync();
                return Created("AudioFile", audioFile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<AudioFile>>> Get()
        {
            try { return Ok(await _context.AudioFiles.ToListAsync()); }
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
        public async Task<ActionResult<AudioFile>> GetId(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var result = await _context.AudioFiles.FindAsync(id);
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
        public async Task<ActionResult<List<AudioFile>>> GetAuthor(string author)
        {
            try
            {
                List<AudioFile> result = new List<AudioFile>();
                var allAudioFiles = await _context.AudioFiles.ToArrayAsync();
                foreach (var audioFile in allAudioFiles)
                {
                    if (audioFile.Author == author) result.Add(audioFile);
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
        public async Task<ActionResult<AudioFile>> Put(AudioFile audioFile)
        {
            try
            {
                var updated = await _context.AudioFiles.FindAsync(audioFile.Id);
                if (updated == null)
                {
                    return NotFound();
                }
                if (!nameRegex.IsMatch(audioFile.Author))
                {
                    return BadRequest("Author name must start with a upper case letter, and cannot contain special characters");
                }
                if (!nameRegex.IsMatch(audioFile.Name))
                {
                    return BadRequest("File name must start with a upper case letter, and cannot contain special characters");
                }
                updated.Id = audioFile.Id;
                updated.Author = audioFile.Author;
                updated.Name = audioFile.Name;
                updated.Genre = audioFile.Genre;
                await _context.SaveChangesAsync();
                return Created("AudioFile", audioFile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("Id")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AudioFile>> Delete(int id)
        {
            try
            {
                var result = await _context.AudioFiles.FindAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                _context.AudioFiles.Remove(result);
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
