// No projeto Aquasys.Core, crie a classe SyncableEntity.cs
using System.ComponentModel.DataAnnotations;

public abstract class SyncableEntity
{
    /// <summary>
    /// Identificador Único Global. A chave principal para a sincronização.
    /// É gerado no cliente no momento da criação.
    /// </summary>
    [Required]
    public Guid GlobalId { get; set; }

    /// <summary>
    /// Data da última modificação do registro. Essencial para a sincronização 'delta' (pull).
    /// Deve ser atualizada (com DateTime.UtcNow) a cada alteração.
    /// </summary>
    [Required]
    public DateTime LastModifiedAt { get; set; }

    /// <summary>
    /// [APENAS NO CLIENTE/MOBILE] Flag que indica se o registro local foi modificado
    /// e precisa ser enviado para o servidor. O servidor não precisa deste campo.
    /// </summary>
    public bool IsSynced { get; set; } = false;
}