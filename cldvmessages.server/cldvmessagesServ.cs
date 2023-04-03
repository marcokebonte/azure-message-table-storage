using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Queue;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace cldvmessages.server
{
    internal class cldvmessagesServ
    {

        const string StorageAccountName = "";
        const string StorageAccountKey = "";
        static void Main(string[] args)
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);

            var client = storageAccount.CreateCloudQueueClient();

            var queue = client.GetQueueReference("");//Enter your message queue name here as it is on Azure site
            queue.CreateIfNotExists();

          

            Console.WriteLine("Waiting for messages.....");

            while (true)
            {
                var message = queue.GetMessage();

                if (message != null)
                {
                    var display = message.AsString;
                    
                    Console.WriteLine(display);

                    queue.DeleteMessage(message);
                }


               

                Thread.Sleep(1000);
            }

        }
    }
}
