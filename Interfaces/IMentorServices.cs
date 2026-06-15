using TraineeManagementApi.Mentors.DTOs;

namespace TraineeManagementApi.Mentors.ServiceInterface;

public interface IMentorServices
{
    public Task<IEnumerable<MentorResponseDto>> GetMentorsAsync();

    public Task<MentorResponseDto> GetMentorByIdAsync(int id);

    public Task<MentorResponseDto> CreateMentorAsync(MentorCreateDto createMentor);

    public Task DeleteMentorByIdAsync(int id);

    public Task<MentorResponseDto> UpdateMentorByIdAsync(int id, MentorUpdateDto updateMentor);
}