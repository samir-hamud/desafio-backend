using App.Utils;
using Domain.Entities;
using Domain.Interfaces;

namespace App.Services;

public class EntregadorService(IEntregadorRepository repo, ILogger<EntregadorService> logger) : IEntregadorService
{
    public async Task AddAsync(Entregador? entity)
    {
        try
        {
            if (entity is null)
            {
                logger.LogError("Tentando adicionar entregador nulo");
                return;
            }
            
            logger.LogInformation("Adicionando entregador {Identificador}", entity.Identificador);
            await repo.AddAsync(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar entregador {Identificador}", entity?.Identificador);
            throw;
        }
    }

    public async Task<Entregador?> GetByIdAsync(string id)
    {
        logger.LogInformation("Buscando entregador por Id {Id}", id);
        return await repo.GetByIdAsync(id);
    }

    public async Task UpdateAsync(Entregador entity)
    {
        try
        {
            logger.LogInformation("Atualizando entregador {Identificador}", entity.Identificador);
            await repo.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar entregador {Identificador}", entity.Identificador);
            throw;
        }
    }

    public async Task<string?> SalvarImagemCnhAsync(string entregadorId, string imagemBase64)
    {
        logger.LogInformation("Salvando imagem da CNH para entregador {Id}", entregadorId);
        var path = await CnhUtils.SalvarImagem(entregadorId, imagemBase64);
        if (string.IsNullOrEmpty(path))
        {
            logger.LogWarning("Falha ao salvar imagem CNH para entregador {Id}", entregadorId);
            return null;
        }
        logger.LogInformation("Imagem CNH salva em {Path} para entregador {Id}", path, entregadorId);
        return path;
    }
}