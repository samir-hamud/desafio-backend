namespace App.Utils;

public static class CnhUtils
{
    public static async Task<string> SalvarImagem(string idEntregador, string imagem)
    {
        if (string.IsNullOrEmpty(idEntregador) || string.IsNullOrEmpty(imagem))
            return string.Empty;

        var dir = new DirectoryInfo($"/imagens/{idEntregador}/");
        var fileName = $"{dir.FullName}/{Guid.NewGuid()}.png";

        try
        {
            if (!dir.Exists)
                dir.Create();

            if (File.Exists(fileName))
                File.Delete(fileName);

            var file = Convert.FromBase64String(imagem);

            await File.WriteAllBytesAsync(fileName, file);
        }
        catch (Exception)
        {
            return string.Empty;
        }

        return fileName;
    }
}