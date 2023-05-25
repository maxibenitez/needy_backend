﻿using needy_dto;

namespace needy_dataAccess.Interfaces
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetSkillsAsync();

        Task<Skill> GetSkillByIdAsync(int skillId);
    }
}
