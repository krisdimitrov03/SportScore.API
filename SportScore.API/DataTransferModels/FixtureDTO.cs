﻿using SportScore.API.DataTransferModels;

namespace SportScore.API.DataTransferModels
{
    public class FixtureDTO
    {
        public string? League { get; set; }

        public List<MatchDTO> Matches { get; set; }
    }
}
