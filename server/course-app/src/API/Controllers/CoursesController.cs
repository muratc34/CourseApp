﻿using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
    {
        var result = await _courseService.Create(courseCreateDto);
        return result.IsSuccess ? Created(nameof(result.Data.Id), result) : result.ToProblemDetails();
    }

    [HttpPut]
    [Route("{courseId}")]
    public async Task<IActionResult> Update(Guid courseId, CourseUpdateDto courseUpdateDto)
    {
        var result = await _courseService.Update(courseId, courseUpdateDto);
        return result.IsSuccess ?  NoContent() : result.ToProblemDetails();
    }

    [HttpDelete]
    [Route("{courseId}")]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        var result = await _courseService.Delete(courseId);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetById(Guid courseId)
    {
        var result = await _courseService.GetCourseById(courseId);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _courseService.GetCourses(cancellationToken));
    }

    [HttpGet]
    [Route("Categories/{categoryId}")]
    public async Task<IActionResult> Get(Guid categoryId, CancellationToken cancellationToken)
    {
        var result = await _courseService.GetCourseByCategoryId(categoryId, cancellationToken);
        return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
    }
}
