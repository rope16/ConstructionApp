﻿namespace ConstructionApp.Dtos.ConstructionSite
{
    public class ConstructionSiteFilterDto
    {
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
