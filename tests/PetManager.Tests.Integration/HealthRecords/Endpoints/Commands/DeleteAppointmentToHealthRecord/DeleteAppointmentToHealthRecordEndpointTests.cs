using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Commands.DeleteAppointmentToHealthRecord;

public class DeleteAppointmentToHealthRecordEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly AppointmentTestFactory _appointmentFactory = new();

    [Fact]
    public async Task delete_appointment_to_health_record_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _healthRecordFactory.DeleteAppointmentToHealthRecordCommand();

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteAppointment.Replace("{healthRecordId:guid}", command.HealthRecordId.ToString())
                .Replace("{appointmentId:guid}", command.AppointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task delete_appointment_to_health_record_with_valid_data_should_return_204_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());
        
        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);
        
        var healthRecord = _healthRecordFactory.CreateHealthRecord(pet.PetId);
        await AddAsync(healthRecord);

        var appointment = _appointmentFactory.CreateAppointment(healthRecord.HealthRecordId);
        await AddAsync(appointment);

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteAppointment.Replace("{healthRecordId:guid}", healthRecord.HealthRecordId.ToString())
                .Replace("{appointmentId:guid}", appointment.AppointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task delete_appointment_to_health_record_given_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var nonExistingHealthRecordId = Guid.NewGuid();
        var appointmentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteAppointment.Replace("{healthRecordId:guid}", nonExistingHealthRecordId.ToString())
                .Replace("{appointmentId:guid}", appointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task delete_appointment_to_health_record_given_non_existing_appointment_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());
        
        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);
        
        var healthRecord = _healthRecordFactory.CreateHealthRecord(pet.PetId);
        await AddAsync(healthRecord);

        var nonExistingAppointmentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteAppointment.Replace("{healthRecordId:guid}", healthRecord.HealthRecordId.ToString())
                .Replace("{appointmentId:guid}", nonExistingAppointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

}