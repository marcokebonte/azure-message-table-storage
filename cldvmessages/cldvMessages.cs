using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Queue;
using System;
using System.Linq;

namespace cldvmessages
{
    public  class cldvMessages
    {

        const string StorageAccountName = "";
        const string StorageAccountKey = "";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Message Queue Class");
        }



        public void MessQueueNormal(string IDi, string VacCentrei, string VacDatei, string VacSeriali)
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);

            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("");//Enter your queue name as it is on Azure site
            queue.CreateIfNotExists();

            try
            {
                
                string userInput = IDi + ":" + VacCentrei + ":" + VacDatei + ":" + VacSeriali;
                int repeatAtLeast = 4;
                bool appears = userInput.Where(c => c == ':').Skip(repeatAtLeast - 1).Any();

                if (appears = true)
                {
                    queue.AddMessage(new CloudQueueMessage(userInput));
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Successful!");
                    Console.WriteLine("----------------------------------------");
                }
                else
                {
                    Console.WriteLine("INCORRECT FORMAT, PLEASE TRY AGAIN");
                }

            }
            catch (Exception)
            {

                Console.WriteLine("Error");
            }

        }



        public void MessQueueReverse(string IDu, string VacCentreu, string VacDateu, string VacSerialu)
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);

            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("cldv-queue");
            queue.CreateIfNotExists();

            try
            {

                string userInput = VacSerialu + ":" + VacDateu + ":" + VacCentreu + ":" + IDu; ;
                int repeatAtLeast = 4;
                bool appears = userInput.Where(c => c == ':').Skip(repeatAtLeast - 1).Any();

                if (appears = true)
                {
                    queue.AddMessage(new CloudQueueMessage(userInput));
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Successful!");
                    Console.WriteLine("----------------------------------------");
                }
                else
                {
                    Console.WriteLine("INCORRECT FORMAT, PLEASE TRY AGAIN");
                }

            }
            catch (Exception)
            {

                Console.WriteLine("Error");
            }

        }
    }



    
}
