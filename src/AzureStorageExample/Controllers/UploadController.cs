using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorageExample.Controllers
{
    public class UploadController : Controller
    {
        /*
        https://mussak.core.windows.net/documentos/cliente1.pdf
        */
        private readonly CloudStorageAccount _storageAccount;

        public UploadController(IConfigurationRoot configurationRoot)
        {
            //Recupera a conta de armazenamento pela connection string
            _storageAccount = CloudStorageAccount.Parse(configurationRoot["StorageConnectionString"]);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload(IFormFile arquivo)
        {
            //Cria um blob client
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            //Recupera a referencia do container documentos
            CloudBlobContainer container = blobClient.GetContainerReference("documentos");

            //Caso não exista, ele cria
            container.CreateIfNotExists();

            //Setar permissão de acesso para 'público'
            container.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
            );

            //Recupera a referência de um blob chamado 'cliente01'
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("cliente01.pdf");

            Stream streamFile = arquivo.OpenReadStream();

            //Cria ou substitui o blob com o conteúdo do upload
            blockBlob.UploadFromStream(streamFile);

            return RedirectToAction("VerArquivos");
        }

        public IActionResult VerArquivos()
        {

            //Cria um blob client
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            //Recupera a referencia do container documentos
            CloudBlobContainer container = blobClient.GetContainerReference("documentos");

            //Caso não exista, ele cria
            container.CreateIfNotExists();

            //Setar permissão de acesso para 'público'
            container.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
            );

            //Recupera os arquivos de um container
            var arquivos = container.ListBlobs();

            return View("Documentos", arquivos);
        }

        public IActionResult Excluir()
        {
            //Cria um blob client
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            //Recupera a referencia do container documentos
            CloudBlobContainer container = blobClient.GetContainerReference("documentos");

            //Caso não exista, ele cria
            container.CreateIfNotExists();

            //Setar permissão de acesso para 'público'
            container.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
            );

            //Recupera a referência de um blob chamado 'cliente01'
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("cliente01.pdf");

            //Cria ou substitui o blob com o conteúdo do upload
            blockBlob.DeleteIfExists();

            return RedirectToAction("VerArquivos");
        }
    }
}