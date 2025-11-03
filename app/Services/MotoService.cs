using App.Mappings;
using App.MessageBroker.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Services;

public class MotoService(IMotoRepository repo, RabbitMqPublisher publisher, ILogger<MotoService> logger) : IMotoService
{
    public async Task<IEnumerable<Moto>> GetAllAsync()
    {
        logger.LogInformation("Listando todas as motos");
        return await repo.GetAllAsync();
    }

    public async Task<Moto?> GetByIdAsync(string id)
    {
        logger.LogInformation("Buscando moto por Id {Id}", id);
        return await repo.GetByIdAsync(id);
    }

    public async Task<Moto?> GetByLicensePlateAsync(string licensePlate)
    {
        logger.LogInformation("Buscando moto por placa {Placa}", licensePlate);
        return await repo.GetByLicensePlateAsync(licensePlate);
    }

    public async Task<Moto?> GetByIdentificationAsync(string id)
    {
        logger.LogInformation("Buscando moto por identificador {Identificador}", id);
        return await repo.GetByIdentificationAsync(id);
    }

    public async Task AddAsync(Moto entity)
    {
        try
        {
            logger.LogInformation("Adicionando moto {Identificador}", entity.Identificador);
            await repo.AddAsync(entity);
            logger.LogInformation("Moto adicionada com sucesso {Identificador}", entity.Identificador);

            if (entity.Ano == 2024)
            {
                await publisher.PublishMessageAsync("Moto2024", entity.ToDTO());
                logger.LogInformation("Moto com ano 2024 publicada");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar moto {Identificador}", entity.Identificador);
            throw;
        }
    }

    public async Task UpdateAsync(Moto entity)
    {
        try
        {
            logger.LogInformation("Atualizando moto {Identificador}", entity.Identificador);
            await repo.UpdateAsync(entity);
            logger.LogInformation("Moto atualizada com sucesso {Identificador}", entity.Identificador);       
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar moto {Identificador}", entity.Identificador);
            throw;
        }
    }

    public async Task DeleteAsync(Moto entity)
    {
        try
        {
            logger.LogInformation("Removendo moto {Identificador}", entity.Identificador);
            await repo.DeleteAsync(entity);
            logger.LogInformation("Moto removida com sucesso {Identificador}", entity.Identificador);       
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover moto {Identificador}", entity.Identificador);
            throw;
        }
    }
}