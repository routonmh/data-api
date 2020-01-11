using System;
using System.Threading.Tasks;
using DataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace
{
    [Route("plants")]
    [ApiController]
    public class PlantController
    {
        [HttpGet("{plantId}")]
        public async Task<ActionResult<PlantEntity>> GetPlantByID(string plantId)
        {
            PlantEntity plant = null;
            Guid id;
            if (Guid.TryParse(plantId, out id))
            {
                plant = await PlantsModel.GetPlantByID(id);
            }

            return plant;
        }
    }
}