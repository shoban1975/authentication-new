using System.Net;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;
using webapi.Utils;

namespace webapi.Controllers;

[ApiController]
[ApiAuth]
[Route("[controller]")]
public class PatientController : WebApiBaseController
{
    private readonly IPatientService patientService;

    public PatientController(IPatientService ps)
    {
        patientService = ps;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(patientService.GetAllPatients());
    }

    [HttpGet("{id}")]
    [ETagFilter]
    public IActionResult GetById(int id)
    {
        return Ok(patientService.GetPatientById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(Patient patient)
    {
        var result = await patientService.CreatePatient(patient);
        return Created(nameof(Post), patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Patient patient)
    {
        return IsValidUpdate(patientService.GetPatientById(id)) ?
                Ok(await patientService.ModifyPatient(id, patient))
                : StatusCode((int)HttpStatusCode.PreconditionFailed);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await patientService.DeletePatient(id));
    }
}