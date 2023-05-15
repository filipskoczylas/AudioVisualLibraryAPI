using AudiovisualLibraryAPI.Data;
using AudiovisualLibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AudiovisualLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoFileController : ControllerBase
    {
        private DataContext _context;
        private Regex nameRegex;
        public VideoFileController(DataContext context)
        {
            _context = context;
            nameRegex = new Regex("^[A-Z](\\w| )*$");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VideoFile>> Post(VideoFile videoFile)
        {
            try
            {
                if (videoFile == null)
                {
                    return BadRequest("Request is null");
                }
                if (videoFile.Id != 0)
                {
                    return BadRequest("Id must be 0");
                }
                if (!nameRegex.IsMatch(videoFile.Author))
                {
                    return BadRequest("Author name must start with a upper case letter, and cannot contain special characters");
                }
                if (!nameRegex.IsMatch(videoFile.Name))
                {
                    return BadRequest("File name must start with a upper case letter, and cannot contain special characters");
                }
                _context.VideoFiles.Add(videoFile);
                await _context.SaveChangesAsync();
                return Created("VideoFile", videoFile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<VideoFile>>> Get()
        {
            try { return Ok(await _context.VideoFiles.ToListAsync()); }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VideoFile>> GetId(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var result = await _context.VideoFiles.FindAsync(id);
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
                List<VideoFile> result = new List<VideoFile>();
                var allVideoFiles = await _context.VideoFiles.ToArrayAsync();
                foreach (var videoFile in allVideoFiles)
                {
                    if (videoFile.Author == author) result.Add(videoFile);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VideoFile>> Put(VideoFile videoFile)
        {
            try
            {
                var updated = await _context.VideoFiles.FindAsync(videoFile.Id);
                if (updated == null)
                {
                    return NotFound();
                }
                if (!nameRegex.IsMatch(videoFile.Author))
                {
                    return BadRequest("Author name must start with a upper case letter, and cannot contain special characters");
                }
                if (!nameRegex.IsMatch(videoFile.Name))
                {
                    return BadRequest("File name must start with a upper case letter, and cannot contain special characters");
                }
                updated.Id = videoFile.Id;
                updated.Author = videoFile.Author;
                updated.Name = videoFile.Name;
                updated.Type = videoFile.Type;
                await _context.SaveChangesAsync();
                return Created("VideoFile", videoFile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<VideoFile>> Delete(int id)
        {
            try
            {
                var result = await _context.VideoFiles.FindAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                _context.VideoFiles.Remove(result);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

