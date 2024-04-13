﻿namespace WebAPI.Models
{
    public class Filter
    {
        public string? Brand { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public string? Model { get; set; }
        public bool? SortByDate { get; set; }

    }
}
