﻿namespace CarRentingSystem.Controllers.Api
{
    using CarRentingSystem.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/statistics")]

    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;
        public StatisticsApiController(IStatisticsService statistics)
        {
            this.statistics = statistics;
        }
        [HttpGet]
        public StatisticsServiceModel GetStatistics()
        {
           return this.statistics.Total();
        }
    }
}
