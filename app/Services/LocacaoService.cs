using App.DTO;
using Domain.Entities;
using Domain.Interfaces;

namespace App.Services;

public class LocacaoService(
    ILocacaoRepository repo,
    ILocacaoMappingService mappingService,
    ILogger<LocacaoService> logger) : ILocacaoService
{
    public async Task<Locacao?> GetByIdAsync(string id)
    {
        logger.LogInformation("Buscando locação por Id {Id}", id);
        return await repo.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Locacao>> GetAllAsync()
    {
        logger.LogInformation("Listando todas locações");
        return await repo.GetAllAsync();   
    }

    public async Task AddFromDTOAsync(LocacaoDTO dto)
    {
        var entity = await mappingService.MapFromDTOAsync(dto);
        await AddAsync(entity);
    }
    
    public async Task AddAsync(Locacao? entity)
    {
        try
        {
            if (entity is null)
            {
                logger.LogError("Tentando adicionar locação nula");
                throw new ArgumentNullException(nameof(entity));
            }

            logger.LogInformation("Adicionando locação {Identificador}", entity.Identificador);

            if (!entity.Entregador.TipoCnh.Contains('A'))
            {
                logger.LogError(
                    "Entregador {EntityIdentificador} não possui CNH A para realizar locações", entity.Identificador);
                throw new EntregadorCNHException();
            }
            
            if (entity.DataInicio != DateTime.Now.Date.AddDays(1))
            {
                logger.LogError(
                    "Data de inicio deve ser obrigatoriamente o primeiro dia após a criação");
                throw new DataInicioException();
            }
            
            await repo.AddAsync(entity);
            logger.LogInformation("Locação adicionada com sucesso {Identificador}", entity.Identificador);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao adicionar locação {Identificador}", entity?.Identificador);
            throw;
        }
    }

    public async Task UpdateAsync(Locacao entity)
    {
        try
        {
            logger.LogInformation("Atualizando locação {Identificador}", entity.Identificador);
            await repo.UpdateAsync(entity);
            logger.LogInformation("Locação atualizada com sucesso {Identificador}", entity.Identificador);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro atualizando locação {Identificador}", entity.Identificador);
            throw;
        }
    }

    public async Task DeleteAsync(Locacao entity)
    {
        try
        {
            logger.LogInformation("Removendo locação {Identificador}", entity.Identificador);
            await repo.DeleteAsync(entity);
            logger.LogInformation("Locação removida com sucesso {Identificador}", entity.Identificador);       
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover locação {Identificador}", entity.Identificador);
        }
    }

    public async Task InformarDevolucao(string id, DateTime devolucao)
    {
        try
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
            {
                logger.LogError("Locação {id} não encontrada", id);
                throw new LocacaoNaoEncontrada();
            }

            logger.LogInformation("Atualizando data de devolução de locação {Identificador}",
                entity.Identificador);

            await BeginUpdate(entity);
            
            entity.DataDevolucao = devolucao;
            await UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar data de devolução");
            throw;
        }
    }

    public async Task BeginUpdate(Locacao entity)
    {
        await repo.BeginUpdate(entity);   
    }
}

public class EntregadorCNHException() : Exception("Entregador sem CNH A");

public class DataInicioException() : Exception("Data de inicio inválida");

public class LocacaoNaoEncontrada() : Exception("Locação não encontrada");
