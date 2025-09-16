using Aquasys.App.Core.DependencyServices;
using Aquasys.Core.Entities;
using Aquasys.App.Core.Utils;
using DevExpress.Maui.Core.Internal;

namespace Aquasys.App.Controls.Editors
{
    public enum TipoArquivoEnum
    {
        Audio,
        Video,
        Documento,
        Imagem
    }

    public enum TipoObtencaoItemEnum
    {
        [EnumDescription("Tirar foto")]
        TirarFoto,
        [EnumDescription("Escolher da galeria")]
        EscolherGaleria,
        [EnumDescription("Documentos")]
        Documentos
    }

    public class DCFileSelector
    {
        public static async Task<List<DCArquivoAbstrato>> GetImagens(int limit = 1)
        {
            List<DCArquivoAbstrato> listaDeArquivos = new();
            List<TipoObtencaoItemEnum> opcoes = new List<TipoObtencaoItemEnum>() { TipoObtencaoItemEnum.EscolherGaleria, TipoObtencaoItemEnum.TirarFoto };
            int? escolha = await DCPopup.ShowOptionsList(opcoes.Select(o => o.GetEnumDescription()));
            if (escolha.HasValue)
                await ObtenhaDados(listaDeArquivos, opcoes[escolha.Value], limit);

            return listaDeArquivos;
        }

        public static async Task<List<DCArquivoAbstrato>> GetArquivos(int LimiteAnexo = 7, string title = "")
        {
            List<DCArquivoAbstrato> listaDeArquivos = new();

            List<TipoObtencaoItemEnum> opcoes = new List<TipoObtencaoItemEnum>() { TipoObtencaoItemEnum.Documentos, TipoObtencaoItemEnum.EscolherGaleria, TipoObtencaoItemEnum.TirarFoto };
            int? escolha = await DCPopup.ShowOptionsList(opcoes.Select(o => o.GetEnumDescription()));
            if (escolha.HasValue)
                await ObtenhaDados(listaDeArquivos, opcoes[escolha.Value], LimiteAnexo);

            return listaDeArquivos;
        }

        public static async Task<List<DCArquivoAbstrato>> GetApenasDocumentos(int LimiteAnexo = 7)
        {
            List<DCArquivoAbstrato> listaDeArquivos = new();
            await ObtenhaDados(listaDeArquivos, TipoObtencaoItemEnum.Documentos, LimiteAnexo);

            if (listaDeArquivos.Any(x => x.Categoria != TipoArquivoEnum.Documento))
            {
                //await DCToast.ShowWarning(TZ.MSG_PERMITIDO_ANEXAR_APENAS_DOCUMENTOS(), 3);
                return listaDeArquivos.Where(x => x.Categoria == TipoArquivoEnum.Documento).ToList();
            }

            return listaDeArquivos;
        }

        private static async Task ObtenhaDados(List<DCArquivoAbstrato> listaDeArquivos, TipoObtencaoItemEnum result, int LimiteAnexo = 7)
        {
            bool fromCamera = false;

            switch (result)
            {
                case TipoObtencaoItemEnum.TirarFoto:
                    fromCamera = true;
                    DCArquivoAbstrato foto = await ObtenhaETrateFotoObtidaDaCamera();
                    listaDeArquivos.Add(foto);
                    break;
                case TipoObtencaoItemEnum.EscolherGaleria:
                    fromCamera = false;
                    List<DCArquivoAbstrato> fotos = await ObtenhaETrateFotosObtidosDaGaleria(LimiteAnexo);
                    listaDeArquivos.AddRange(fotos);
                    listaDeArquivos = listaDeArquivos.Take(LimiteAnexo).ToList();
                    break;
                case TipoObtencaoItemEnum.Documentos:
                    fromCamera = false;
                    List<DCArquivoAbstrato> documentos = await ObtenhaETrateArquivos(LimiteAnexo == 1);
                    listaDeArquivos.AddRange(documentos);
                    break;
                default:
                    return;
            }


            List<DCImagem> imagens = listaDeArquivos.OfType<DCImagem>().ToList();
            if (imagens == null)
                return;

            foreach (var imagem in imagens)
            {
                await CropImage(imagem);
            }


            if (imagens != null && imagens.Count > 0)
            {
                int countResizeRemovedImage = 0;
                foreach (DCImagem item in imagens)
                {
                    if (item != null && item.Content != null)
                    {
                        item.Content = await DCImageUtils.ResizeImageAsync(item.Content, fromCamera);
                        if (item.Content == null)
                            countResizeRemovedImage++;
                    }
                }

                //if (countResizeRemovedImage == 1)
                //    await DCToast.Show(TZ.MSG_IMAGEM_VALIDACAO_TAMANHO(), 3);
                //if (countResizeRemovedImage > 1)
                //    await DCToast.Show(TZ.MSG_IMAGENS_VALIDACAO_TAMANHO(), 3);

                listaDeArquivos.RemoveAll(x => x == null || x.Content == null);
            }
        }

        private static async Task<DCImagem> CropImage(DCImagem dcImagem)
        {
            DCCropImagePage dcCropImagePage = new DCCropImagePage(dcImagem);

            await NavigationUtils.PushAsync(dcCropImagePage);

            return await dcCropImagePage.WaitForResultAsync();
        }

        private static async Task<List<DCArquivoAbstrato>> ObtenhaETrateFotosObtidosDaGaleria(int LimiteAnexo = 7)
        {
            List<DCArquivoAbstrato> imagens = new List<DCArquivoAbstrato>();


            await Task.Delay(100);//Delay Necessário para abrir as fotos no iOS

            PickOptions pickOptions = new PickOptions();
            pickOptions.FileTypes = FilePickerFileType.Images;
            pickOptions.PickerTitle = "Images";//TZ.TXT_IMAGENS();

            IEnumerable<FileResult> selecao = await FilePicker.PickMultipleAsync(pickOptions);

            if ((selecao?.Count() ?? 0) > LimiteAnexo)
            {
                //await DCMessages.DisplaySnackBarAsync($"{TZ.TXT_QUANTIDADEMAXIMAIMAGENS()} : {LimiteAnexo}");
                return imagens;
            }

            if (selecao?.Count() == null)
                return imagens;

            foreach (FileResult arquivo in selecao)
            {

                DCImagem imagem = new DCImagem();
                imagem.FileName = Path.GetFileNameWithoutExtension(arquivo.FileName);
                imagem.Extension = Path.GetExtension(arquivo.FileName);
                imagem.CaminhoDoArquivo = arquivo.FullPath;

                Stream stream = await arquivo.OpenReadAsync();
                imagem.ImageSource = ImageSource.FromStream(() => stream);


                if (imagens.Count < LimiteAnexo)
                    imagens.Add(imagem);
            }


            return imagens;
        }

        public static async Task<DCArquivoAbstrato> ObtenhaETrateFotoObtidaDaCamera()
        {
            await Task.Delay(100); //Delay Necessário para abrir a camera no iOS

            if (!MediaPicker.Default.IsCaptureSupported)
                return null;

            FileResult photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions() { });

            if (photo == null)
                return null;

            DCImagem imagem = new DCImagem();
            imagem.FileName = Path.GetFileNameWithoutExtension(photo.FileName);
            imagem.Extension = Path.GetExtension(photo.FileName);
            imagem.CaminhoDoArquivo = photo.FullPath;
            imagem.Content = await File.ReadAllBytesAsync(photo.FullPath);
            imagem.ImageSource = ImageSource.FromFile(photo.FullPath);

            return imagem;
        }


        public static async Task<DCArquivoAbstrato> ObtenhaETrateFotoObtidaDaCameraOld()
        {
            await Task.Delay(100); //Delay Necessário para abrir a camera no iOS

            if (!MediaPicker.Default.IsCaptureSupported)
                return null;

            var photoReturn = await Application.Current!.MainPage!.Handler.MauiContext.Services.GetService<IPhotoPickerService>().CapturePhotoAsync();

            if (photoReturn.Photo == null)
                return null;

            DCImagem imagem = new DCImagem();
            imagem.FileName = Path.GetFileNameWithoutExtension(photoReturn.Path);
            imagem.Extension = Path.GetExtension(photoReturn.Path);
            imagem.CaminhoDoArquivo = photoReturn.Path;
            imagem.Content = photoReturn.Photo;// ToArray();
            imagem.ImageSource = ImageSource.FromFile(photoReturn.Path); // FromStream(() => photo);

            return imagem;
        }

        private static async Task<List<DCArquivoAbstrato>> ObtenhaETrateArquivos(bool apenasUmArquivo = false)
        {
            List<DCArquivoAbstrato> documentos = new List<DCArquivoAbstrato>();

            await Task.Delay(100); //Delay Necessário para abrir o picker no iOS

            List<FileResult> documentosSelecionados = new List<FileResult>();
            if (apenasUmArquivo)
            {
                FileResult file = await FilePicker.PickAsync();
                if (file != null)
                    documentosSelecionados = new List<FileResult>() { file };
            }
            else
                documentosSelecionados = (await FilePicker.PickMultipleAsync()).ToList();

            if (documentosSelecionados == null)
                return documentos;

            foreach (FileResult arquivoSelecionado in documentosSelecionados)
            {
                string formato = arquivoSelecionado.FileName.Split('.').Last();
                TipoArquivoEnum classificacaoMedia = ObtenhaTypoMedia(formato);

                DCArquivoAbstrato arquivo;

                using Stream stream = await arquivoSelecionado.OpenReadAsync();
                using MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);

                switch (classificacaoMedia)
                {
                    case TipoArquivoEnum.Imagem:
                        DCImagem imagem = new DCImagem();
                        imagem.ImageSource = ImageSource.FromStream(() => ms);
                        arquivo = imagem;
                        break;
                    case TipoArquivoEnum.Audio:
                        arquivo = new DCAudio();
                        break;
                    case TipoArquivoEnum.Video:
                        arquivo = new DCVideo();
                        break;
                    case TipoArquivoEnum.Documento:
                    default:
                        arquivo = new DCDocumento();
                        break;
                }
                arquivo.FileName = arquivoSelecionado.FileName;
                arquivo.Extension = formato;
                arquivo.Size = new FileInfo(arquivoSelecionado.FullPath).Length;
                arquivo.DataAnexo = DateTime.Now;
                arquivo.CaminhoDoArquivo = arquivoSelecionado.FullPath;
                arquivo.Content = ms.ToArray();
                arquivo.TirouFoto = false;

                documentos.Add(arquivo);
            }

            return documentos;
        }

        public static readonly string MEDIA_AUDIO = "mp3 wmv aac flac alac aiff ape dsd mqa";
        public static readonly string MEDIA_IMAGE = "png jpg bmp tiff psd exif raw jpeg";
        public static readonly string MEDIA_VIDEO = "mp4 3gp mov mpeg-1 mpeg-2 mpg avi wmv";

        public static TipoArquivoEnum ObtenhaTypoMedia(string formato)
        {
            if (MEDIA_AUDIO.Contains(formato))
                return TipoArquivoEnum.Audio;

            if (MEDIA_IMAGE.Contains(formato))
                return TipoArquivoEnum.Imagem;

            if (MEDIA_VIDEO.Contains(formato))
                return TipoArquivoEnum.Video;

            return TipoArquivoEnum.Documento;
        }

        private static string SaveTempToDisk(byte[] data)
        {
            string pathTofileName = Path.Combine(FileSystem.CacheDirectory, Guid.NewGuid().ToString());
            File.WriteAllBytes(pathTofileName, data);
            return pathTofileName;
        }

        private static void DeleteTempToDisk(string pathTofileName)
        {
            if (File.Exists(pathTofileName))
                File.Delete(pathTofileName);
        }
    }

    public abstract class DCArquivoAbstrato
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string CaminhoDoArquivo { get; set; }
        public DateTime DataAnexo { get; set; }
        public byte[] Content { get; set; }
        public long Size { get; set; }
        public TipoArquivoEnum Categoria { get; set; }
        public ImageSource RepresentacaoIlustrada { get; set; }
        public bool TirouFoto { get; set; }
    }

    public class DCImagem : DCArquivoAbstrato
    {
        public DCImagem() : base()
        {
            Categoria = TipoArquivoEnum.Imagem;
        }
        public ImageSource ImageSource { get; set; }
    }

    public class DCDocumento : DCArquivoAbstrato
    {
        public DCDocumento() : base()
        {
            Categoria = TipoArquivoEnum.Documento;
            RepresentacaoIlustrada = ImageSource.FromFile("icon_document.png");
        }
    }

    public class DCAudio : DCArquivoAbstrato
    {
        public DCAudio() : base()
        {
            Categoria = TipoArquivoEnum.Audio;
            RepresentacaoIlustrada = ImageSource.FromFile("icon_audio.png");
        }
    }

    public class DCVideo : DCArquivoAbstrato
    {
        public DCVideo() : base()
        {
            Categoria = TipoArquivoEnum.Video;
            RepresentacaoIlustrada = ImageSource.FromFile("icon_video.png");
        }
    }
}
