﻿namespace SnappetChallenge.Models
{
    public class SubjectOverviewDto
    {
        public string Subject { get; set; }
        public int UniqueExercises { get; set; }
        public int TotalAnswers { get; set; }
        public decimal AssessedSkillLevelChange { get; set; }
        public int TotalReanswered { get; set; }
        public decimal? TotalReansweredPercentage { get; set; }
    }
}