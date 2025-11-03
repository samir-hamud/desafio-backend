using Domain.Entities;
using Domain.Interfaces;

namespace App.Services;

public class MensagemService(IMensagemRepository repo,
    ILogger<MensagemService> logger) : IMensagemService
{
    public async Task<Mensagem?> GetByIdAsync(string id)
    {
        logger.LogInformation("Buscando mensagem por Id {Id}", id);
        return await repo.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Mensagem>> GetAllAsync()
    {
        logger.LogInformation("Listando todas mensagens");
        return await repo.GetAllAsync();   
    }

    public async Task AddAsync(Mensagem entity)
    {
        try
        {
            logger.LogInformation("Adicionando mensagem {Identificador}", entity.Identificador);
            await repo.AddAsync(entity);
            logger.LogInformation("Mensagem adicionada com sucesso {Identificador}",
                entity.Identificador);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar mensagem {Identificador}", entity.Identificador);
            throw;
        }
    }

    public async Task UpdateAsync(Mensagem entity)
    {
        try
        {
            logger.LogInformation("Atualizando mensagem {Identificador}", entity.Identificador);
            await repo.UpdateAsync(entity);
            logger.LogInformation("Mensagem atualizada com sucesso {Identificador}",
                entity.Identificador);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar mensagem {Identificador}", entity.Identificador);
            throw;
        }
    }

    public async Task DeleteAsync(Mensagem entity)
    {
        try
        {
            logger.LogInformation("Removendo mensagem {Identificador}", entity.Identificador);
            await repo.DeleteAsync(entity);
            logger.LogInformation("Mensagem removida com sucesso {Identificador}",
                entity.Identificador);       
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover mensagem {Identificador}", entity.Identificador);
            throw;
        }
    }
}