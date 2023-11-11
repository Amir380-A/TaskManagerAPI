using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.File;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class TaskController : ControllerBase
{
 private static List<TaskModel> tasks = new List<TaskModel>();

    [HttpGet]
    public IActionResult GetTasks()
    {
        
        return Ok(tasks);
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskModel model)
    {
        tasks.Add(model);
        return CreatedAtAction(nameof(GetTaskById), new { id = model.Id }, model);
    }

    [HttpGet("{id}")]
    public IActionResult GetTaskById(int id)
    {
        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] TaskModel updatedTask)
    { 
        var taskIndex = tasks.FindIndex(t => t.Id == id);
        if (taskIndex == -1)
        {
            return NotFound();
        }
        tasks[taskIndex] = updatedTask;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    { 
        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            return NotFound();
        }
        tasks.Remove(task);
        return NoContent();
    }
}

public class TaskModel
{
        public int Id { get; internal set; }
}